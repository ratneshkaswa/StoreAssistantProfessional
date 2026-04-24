using System.Security.Cryptography;
using System.Text.Json;

namespace StoreAssistantProfessional.Services;

public interface ISetupService
{
    bool IsComplete { get; }
    SetupData? Current { get; }
    void Save(string storeName, string adminPin, string masterPin);
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
        _cached = Load();
    }

    public bool IsComplete => _cached is not null;

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

        File.WriteAllText(_path, JsonSerializer.Serialize(data, JsonOptions));
        _cached = data;
    }

    private SetupData? Load()
    {
        if (!File.Exists(_path)) return null;
        try
        {
            return JsonSerializer.Deserialize<SetupData>(File.ReadAllText(_path), JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
        catch (IOException)
        {
            return null;
        }
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
