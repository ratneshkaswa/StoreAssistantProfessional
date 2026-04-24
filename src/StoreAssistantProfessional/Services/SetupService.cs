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
    private const int SaltBytes = 16;
    private const int HashBytes = 32;
    private const int Pbkdf2Iterations = 100_000;

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string _path;
    private SetupData? _cached;

    public SetupService()
    {
        var appData = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StoreAssistantProfessional");
        Directory.CreateDirectory(appData);
        _path = Path.Combine(appData, "setup.json");
        (_cached, Status) = Load();
    }

    public SetupStatus Status { get; private set; }

    public bool IsComplete => Status == SetupStatus.Valid;

    public SetupData? Current => _cached;

    public void Save(string storeName, string adminPin, string masterPin)
    {
        var adminSalt = RandomNumberGenerator.GetBytes(SaltBytes);
        var masterSalt = RandomNumberGenerator.GetBytes(SaltBytes);

        var data = new SetupData(
            storeName,
            Convert.ToBase64String(Pbkdf2(adminPin, adminSalt)),
            Convert.ToBase64String(adminSalt),
            Convert.ToBase64String(Pbkdf2(masterPin, masterSalt)),
            Convert.ToBase64String(masterSalt),
            DateTime.UtcNow);

        var json = JsonSerializer.Serialize(data, JsonOptions);
        var tmp = _path + ".tmp";
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
        Status = SetupStatus.Valid;
    }

    public bool VerifyAdmin(string pin)
    {
        if (_cached is null || pin.Length != 4 || !pin.All(char.IsDigit)) return false;
        var salt = Convert.FromBase64String(_cached.AdminPinSalt);
        var expected = Convert.FromBase64String(_cached.AdminPinHash);
        return CryptographicOperations.FixedTimeEquals(Pbkdf2(pin, salt), expected);
    }

    public bool VerifyMaster(string pin)
    {
        if (_cached is null || pin.Length != 6 || !pin.All(char.IsDigit)) return false;
        var salt = Convert.FromBase64String(_cached.MasterPinSalt);
        var expected = Convert.FromBase64String(_cached.MasterPinHash);
        return CryptographicOperations.FixedTimeEquals(Pbkdf2(pin, salt), expected);
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
            return JsonSerializer.Deserialize<SetupData>(File.ReadAllText(path), JsonOptions);
        }
        catch (JsonException) { return null; }
        catch (IOException) { return null; }
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
    DateTime CreatedAtUtc);
