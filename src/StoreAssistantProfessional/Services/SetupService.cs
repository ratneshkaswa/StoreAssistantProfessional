using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace StoreAssistantProfessional.Services;

public enum SetupStatus
{
    NotSetup,
    Valid,
    Corrupt
}

public interface ISetupService
{
    SetupStatus Status { get; }
    bool IsComplete { get; }
    SetupData? Current { get; }
    void Save(string storeName, string adminPin, string masterPin);
    bool VerifyAdmin(string pin);
    bool VerifyMaster(string pin);
}

public sealed class SetupService : ISetupService
{
    public const int AdminPinLength = 4;
    public const int MasterPinLength = 6;
    public const int MinStoreNameLength = 2;
    public const int MaxStoreNameLength = 100;
    public const int CurrentSchemaVersion = 1;

    private const int SaltBytes = 16;
    private const int HashBytes = 32;
    private const int Pbkdf2Iterations = 600_000;

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly object _lock = new();
    private readonly string _path;
    private SetupData? _cached;
    private SetupStatus _status;

    public SetupService()
    {
        var appData = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StoreAssistantProfessional");
        Directory.CreateDirectory(appData);
        _path = Path.Combine(appData, "setup.json");
        (_cached, _status) = Load();
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

    public void Save(string storeName, string adminPin, string masterPin)
    {
        var name = (storeName ?? "").Trim();
        if (name.Length is < MinStoreNameLength or > MaxStoreNameLength)
            throw new ArgumentException(
                $"Store name must be {MinStoreNameLength}–{MaxStoreNameLength} characters.",
                nameof(storeName));

        ValidatePin(adminPin, AdminPinLength, nameof(adminPin));
        ValidatePin(masterPin, MasterPinLength, nameof(masterPin));

        if (masterPin.StartsWith(adminPin, StringComparison.Ordinal))
            throw new ArgumentException(
                "Master PIN must not start with the admin PIN.",
                nameof(masterPin));

        var adminSalt = RandomNumberGenerator.GetBytes(SaltBytes);
        var masterSalt = RandomNumberGenerator.GetBytes(SaltBytes);

        var data = new SetupData(
            name,
            Convert.ToBase64String(Pbkdf2(adminPin, adminSalt)),
            Convert.ToBase64String(adminSalt),
            Convert.ToBase64String(Pbkdf2(masterPin, masterSalt)),
            Convert.ToBase64String(masterSalt),
            DateTime.UtcNow,
            CurrentSchemaVersion);

        var json = JsonSerializer.Serialize(data, JsonOptions);
        var tmp = _path + ".tmp";

        lock (_lock)
        {
            File.WriteAllText(tmp, json);

            if (File.Exists(_path))
            {
                File.Replace(tmp, _path, _path + ".bak");
            }
            else
            {
                File.Move(tmp, _path);
            }

            _cached = data;
            _status = SetupStatus.Valid;
        }
    }

    public bool VerifyAdmin(string pin)
    {
        SetupData? cached;
        lock (_lock) cached = _cached;
        if (cached is null || pin.Length != AdminPinLength || !pin.All(char.IsDigit)) return false;
        var salt = Convert.FromBase64String(cached.AdminPinSalt);
        var expected = Convert.FromBase64String(cached.AdminPinHash);
        return CryptographicOperations.FixedTimeEquals(Pbkdf2(pin, salt), expected);
    }

    public bool VerifyMaster(string pin)
    {
        SetupData? cached;
        lock (_lock) cached = _cached;
        if (cached is null || pin.Length != MasterPinLength || !pin.All(char.IsDigit)) return false;
        var salt = Convert.FromBase64String(cached.MasterPinSalt);
        var expected = Convert.FromBase64String(cached.MasterPinHash);
        return CryptographicOperations.FixedTimeEquals(Pbkdf2(pin, salt), expected);
    }

    private static void ValidatePin(string pin, int length, string paramName)
    {
        if (pin is null || pin.Length != length || !pin.All(char.IsDigit))
            throw new ArgumentException($"PIN must be {length} digits.", paramName);
        if (PinRules.IsWeak(pin))
            throw new ArgumentException("PIN is too predictable.", paramName);
    }

    private (SetupData? data, SetupStatus status) Load()
    {
        var primary = TryLoadFile(_path);
        if (primary is not null) return (primary, SetupStatus.Valid);

        var bakPath = _path + ".bak";
        var backup = TryLoadFile(bakPath);
        if (backup is not null)
        {
            try { File.Copy(bakPath, _path, overwrite: true); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            return (backup, SetupStatus.Valid);
        }

        return File.Exists(_path) || File.Exists(bakPath)
            ? (null, SetupStatus.Corrupt)
            : (null, SetupStatus.NotSetup);
    }

    private static SetupData? TryLoadFile(string path)
    {
        if (!File.Exists(path)) return null;
        try
        {
            var data = JsonSerializer.Deserialize<SetupData>(File.ReadAllText(path), JsonOptions);
            return IsDataComplete(data) ? data : null;
        }
        catch (JsonException) { return null; }
        catch (IOException) { return null; }
    }

    private static bool IsDataComplete(SetupData? data) =>
        data is not null &&
        data.SchemaVersion == CurrentSchemaVersion &&
        !string.IsNullOrWhiteSpace(data.StoreName) &&
        IsValidBase64(data.AdminPinHash) &&
        IsValidBase64(data.AdminPinSalt) &&
        IsValidBase64(data.MasterPinHash) &&
        IsValidBase64(data.MasterPinSalt);

    private static bool IsValidBase64(string? s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        var buffer = new byte[((s.Length + 3) / 4) * 3];
        return Convert.TryFromBase64String(s, buffer, out _);
    }

    private static byte[] Pbkdf2(string input, byte[] salt) =>
        Rfc2898DeriveBytes.Pbkdf2(input, salt, Pbkdf2Iterations, HashAlgorithmName.SHA256, HashBytes);
}

public sealed record SetupData(
    string StoreName,
    string AdminPinHash,
    string AdminPinSalt,
    string MasterPinHash,
    string MasterPinSalt,
    DateTime CreatedAtUtc,
    int SchemaVersion = 1);
