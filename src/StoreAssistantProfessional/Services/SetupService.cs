using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoreAssistantProfessional.Services;

public enum SetupStatus
{
    NotSetup,
    Valid,
    Corrupt,
    Inaccessible,
}

public interface ISetupService
{
    SetupStatus Status { get; }
    bool IsComplete { get; }
    SetupData? Current { get; }
    void Save(string firmName, string adminPin, string masterPin);
    bool VerifyAdmin(string? pin);
    bool VerifyMaster(string? pin);
}

public sealed class SetupService : ISetupService
{
    public const int AdminPinLength = 4;
    public const int MasterPinLength = 6;
    public const int MinFirmNameLength = 2;
    public const int MaxFirmNameLength = 100;
    public const int CurrentSchemaVersion = 1;

    private const int SaltBytes = 16;
    private const int HashBytes = 32;
    public const int CurrentPbkdf2Iterations = 600_000;
    private const int LegacyPbkdf2Iterations = 600_000;
    private const long MaxFileBytes = 64 * 1024;

    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = false,
    };

    private static readonly JsonSerializerOptions ReadOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    // setup.json contents are PBKDF2 hashes (not the PIN itself), but the file is plaintext
    // on disk. Anyone who can read the file can mount an offline brute-force attack against
    // 4-/6-digit PINs; the 600k SHA-256 iterations slow CPU attacks but not GPU farms.
    // Mitigation today: NTFS ACLs default to user-only on LocalAppData. Future: wrap with
    // ProtectedData (DPAPI) so the file is unusable on a different Windows account / machine.

    private readonly ISessionService _session;
    private readonly object _lock = new();
    private readonly string? _path;
    private readonly string? _tmpPath;
    private readonly string? _bakPath;
    private SetupData? _cached;
    private SetupStatus _status;

    public SetupService(ISessionService session) : this(session, appDataDir: null) { }

    // appDataDir lets tests redirect storage; production passes null and uses LocalApplicationData.
    public SetupService(ISessionService session, string? appDataDir)
    {
        _session = session;
        try
        {
            var dir = appDataDir ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "StoreAssistantProfessional");
            Directory.CreateDirectory(dir);
            _path = Path.Combine(dir, "setup.json");
            _tmpPath = _path + ".tmp";
            _bakPath = _path + ".bak";
            (_cached, _status) = Load();
        }
        catch (Exception ex) when (
            ex is IOException or
            UnauthorizedAccessException or
            SecurityException or
            ArgumentException or
            NotSupportedException)
        {
            _path = null;
            _tmpPath = null;
            _bakPath = null;
            _cached = null;
            _status = SetupStatus.Inaccessible;
        }
    }

    public SetupStatus Status
    {
        get { lock (_lock) return _status; }
    }

    public bool IsComplete
    {
        get { lock (_lock) return _status == SetupStatus.Valid; }
    }

    public SetupData? Current
    {
        get { lock (_lock) return _cached; }
    }

    public void Save(string firmName, string adminPin, string masterPin)
    {
        if (_path is null || _tmpPath is null || _bakPath is null)
            throw new IOException("Setup storage is not accessible on this machine.");

        var name = (firmName ?? "").Trim();
        if (name.Length is < MinFirmNameLength or > MaxFirmNameLength)
            throw new ArgumentException(
                $"Firm name must be {MinFirmNameLength}–{MaxFirmNameLength} characters.",
                nameof(firmName));

        ValidatePin(adminPin, AdminPinLength, "Admin", nameof(adminPin));
        ValidatePin(masterPin, MasterPinLength, "Master", nameof(masterPin));

        if (masterPin.Contains(adminPin, StringComparison.Ordinal))
            throw new ArgumentException(
                "Master PIN must not contain the Admin PIN.",
                nameof(masterPin));

        var adminSalt = RandomNumberGenerator.GetBytes(SaltBytes);
        var masterSalt = RandomNumberGenerator.GetBytes(SaltBytes);
        var adminHash = Pbkdf2(adminPin, adminSalt, CurrentPbkdf2Iterations);
        var masterHash = Pbkdf2(masterPin, masterSalt, CurrentPbkdf2Iterations);

        try
        {
            var data = new SetupData(
                name,
                Convert.ToBase64String(adminHash),
                Convert.ToBase64String(adminSalt),
                Convert.ToBase64String(masterHash),
                Convert.ToBase64String(masterSalt),
                CurrentPbkdf2Iterations,
                DateTime.UtcNow,
                CurrentSchemaVersion);

            var json = JsonSerializer.Serialize(data, WriteOptions);
            var bytes = Encoding.UTF8.GetBytes(json);

            try { File.Delete(_tmpPath); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }

            using (var fs = new FileStream(_tmpPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush(flushToDisk: true);
            }

            if (File.Exists(_path))
                File.Replace(_tmpPath, _path, _bakPath, ignoreMetadataErrors: true);
            else
                File.Move(_tmpPath, _path);

            lock (_lock)
            {
                _cached = data;
                _status = SetupStatus.Valid;
            }
        }
        finally
        {
            CryptographicOperations.ZeroMemory(adminHash);
            CryptographicOperations.ZeroMemory(masterHash);
        }
    }

    public bool VerifyAdmin(string? pin) => Verify(pin, AdminPinLength, admin: true);
    public bool VerifyMaster(string? pin) => Verify(pin, MasterPinLength, admin: false);

    private bool Verify(string? pin, int expectedLength, bool admin)
    {
        if (_session.UnlockLockoutRemaining is not null) return false;

        SetupData? cached;
        lock (_lock) cached = _cached;
        if (cached is null) return false;
        if (pin is null || pin.Length != expectedLength || !IsAsciiDigits(pin)) return false;

        byte[]? actual = null;
        try
        {
            var saltStr = admin ? cached.AdminPinSalt : cached.MasterPinSalt;
            var hashStr = admin ? cached.AdminPinHash : cached.MasterPinHash;
            var salt = Convert.FromBase64String(saltStr);
            var expected = Convert.FromBase64String(hashStr);
            if (salt.Length != SaltBytes || expected.Length != HashBytes) return false;

            var iterations = cached.Iterations > 0 ? cached.Iterations : LegacyPbkdf2Iterations;
            actual = Pbkdf2(pin, salt, iterations);
            var ok = CryptographicOperations.FixedTimeEquals(actual, expected);
            if (!ok) _session.RecordFailedUnlock();
            return ok;
        }
        catch (FormatException)
        {
            return false;
        }
        finally
        {
            if (actual is not null) CryptographicOperations.ZeroMemory(actual);
        }
    }

    private static void ValidatePin(string pin, int length, string label, string paramName)
    {
        if (pin is null || pin.Length != length || !IsAsciiDigits(pin))
            throw new ArgumentException($"{label} PIN must be {length} digits.", paramName);
        if (PinRules.IsWeak(pin))
            throw new ArgumentException($"{label} PIN is too predictable.", paramName);
    }

    public static bool IsAsciiDigits(string s)
    {
        for (var i = 0; i < s.Length; i++)
            if (s[i] is < '0' or > '9') return false;
        return true;
    }

    private (SetupData? data, SetupStatus status) Load()
    {
        if (_path is null || _bakPath is null) return (null, SetupStatus.Inaccessible);

        var primary = TryLoadFile(_path);
        if (primary is not null) return (Migrate(primary), SetupStatus.Valid);

        var backup = TryLoadFile(_bakPath);
        if (backup is not null)
        {
            try { File.Copy(_bakPath, _path, overwrite: true); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            return (Migrate(backup), SetupStatus.Valid);
        }

        var primaryExists = SafeExists(_path);
        var backupExists = SafeExists(_bakPath);
        return primaryExists || backupExists
            ? (null, SetupStatus.Corrupt)
            : (null, SetupStatus.NotSetup);
    }

    private static bool SafeExists(string path)
    {
        try { return File.Exists(path); }
        catch (IOException) { return false; }
        catch (UnauthorizedAccessException) { return false; }
        catch (SecurityException) { return false; }
    }

    private static SetupData? TryLoadFile(string path)
    {
        try
        {
            if (!File.Exists(path)) return null;
            var info = new FileInfo(path);
            if (info.Length > MaxFileBytes) return null;
            var data = JsonSerializer.Deserialize<SetupData>(File.ReadAllText(path), ReadOptions);
            return IsDataComplete(data) ? data : null;
        }
        catch (JsonException) { return null; }
        catch (IOException) { return null; }
        catch (UnauthorizedAccessException) { return null; }
        catch (SecurityException) { return null; }
    }

    // Schema migrations land here. Today only v1 exists; if CurrentSchemaVersion is bumped,
    // map older shapes onto the current SetupData record before returning.
    private static SetupData Migrate(SetupData data)
    {
        if (data.Iterations <= 0)
            data = data with { Iterations = LegacyPbkdf2Iterations };
        return data;
    }

    private static bool IsDataComplete(SetupData? data)
    {
        if (data is null) return false;
        if (data.SchemaVersion != CurrentSchemaVersion) return false;
        if (string.IsNullOrWhiteSpace(data.FirmName)) return false;
        return TryDecodeBytes(data.AdminPinHash, HashBytes) &&
               TryDecodeBytes(data.AdminPinSalt, SaltBytes) &&
               TryDecodeBytes(data.MasterPinHash, HashBytes) &&
               TryDecodeBytes(data.MasterPinSalt, SaltBytes);
    }

    private static bool TryDecodeBytes(string? s, int expectedLength)
    {
        if (string.IsNullOrEmpty(s)) return false;
        Span<byte> buffer = stackalloc byte[expectedLength + 4];
        return Convert.TryFromBase64String(s, buffer, out var bytesWritten) &&
               bytesWritten == expectedLength;
    }

    private static byte[] Pbkdf2(string input, byte[] salt, int iterations) =>
        Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, HashAlgorithmName.SHA256, HashBytes);
}

public sealed record SetupData(
    // JSON name kept as "storeName" to read v1 setup.json files unchanged.
    [property: JsonPropertyName("storeName")] string FirmName,
    [property: JsonPropertyName("adminPinHash")] string AdminPinHash,
    [property: JsonPropertyName("adminPinSalt")] string AdminPinSalt,
    [property: JsonPropertyName("masterPinHash")] string MasterPinHash,
    [property: JsonPropertyName("masterPinSalt")] string MasterPinSalt,
    [property: JsonPropertyName("iterations")] int Iterations,
    [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
    [property: JsonPropertyName("schemaVersion")] int SchemaVersion = 1);
