using System.IO;
using System.Text;
using StoreAssistantProfessional.Services;
using Xunit;

namespace StoreAssistantProfessional.Tests;

public class SetupServiceTests
{
    [Theory]
    [InlineData("0123", true)]
    [InlineData("", true)]
    [InlineData("abc", false)]
    public void IsAsciiDigits_ForwardsToPinRules(string input, bool expected)
    {
        // SetupService.IsAsciiDigits is a thin forward to PinRules.IsAsciiDigits;
        // the canonical tests live on PinRules. This single case keeps the
        // forwarding contract honest.
        Assert.Equal(expected, SetupService.IsAsciiDigits(input));
    }

    [Fact]
    public void Verify_ReturnsFalse_WhenNoSetup()
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        Assert.Equal(SetupStatus.NotSetup, service.Status);
        Assert.False(service.VerifyAdmin("1234"));
        Assert.False(service.VerifyMaster("123456"));
    }

    [Fact]
    public void Verify_HandlesNullPin_WithoutThrowing()
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        Assert.False(service.VerifyAdmin(null));
        Assert.False(service.VerifyMaster(null));
    }

    [Fact]
    public void Save_RejectsMasterContainingAdmin()
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        Assert.Throws<ArgumentException>(() => service.Save("My Store", "5839", "915839"));
    }

    [Fact]
    public void Save_RejectsWeakAdminPin()
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        Assert.Throws<ArgumentException>(() => service.Save("My Store", "1234", "582143"));
    }

    [Theory]
    [InlineData("1990")]   // plausible year
    [InlineData("0007")]   // low-uniqueness
    [InlineData("1110")]   // low-uniqueness
    public void Save_RejectsNewlyClassifiedWeakAdminPins(string adminPin)
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        Assert.Throws<ArgumentException>(() => service.Save("My Store", adminPin, "582143"));
    }

    [Fact]
    public void Save_RejectsShortFirmName()
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        Assert.Throws<ArgumentException>(() => service.Save("a", "5839", "493827"));
    }

    [Fact]
    public void Save_Then_VerifyRoundtrip_Works()
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        service.Save("My Store", "5839", "493827");

        Assert.True(service.IsComplete);
        Assert.True(service.VerifyAdmin("5839"));
        Assert.False(service.VerifyAdmin("0000"));
        Assert.True(service.VerifyMaster("493827"));
        Assert.False(service.VerifyMaster("000000"));
    }

    [Fact]
    public void Save_Persists_AcrossInstances()
    {
        using var dir = new TempDir();
        {
            var s1 = new SetupService(new SessionService(), dir.Path);
            s1.Save("Persist Store", "5839", "493827");
        }

        var s2 = new SetupService(new SessionService(), dir.Path);
        Assert.Equal(SetupStatus.Valid, s2.Status);
        Assert.True(s2.VerifyAdmin("5839"));
    }

    [Fact]
    public void Save_WritesEncryptedFile_NotPlaintextJson()
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        service.Save("Encrypted Store", "5839", "493827");

        var bytes = File.ReadAllBytes(Path.Combine(dir.Path, "setup.json"));
        var asString = Encoding.UTF8.GetString(bytes);

        // The encrypted DPAPI blob shouldn't contain any of the JSON property
        // names a plaintext file would. If it did, encryption isn't happening.
        Assert.DoesNotContain("\"storeName\"", asString);
        Assert.DoesNotContain("\"adminPinHash\"", asString);
        Assert.DoesNotContain("Encrypted Store", asString);
    }

    [Fact]
    public void Verify_RecordsFailedAttempt_AndLocksOut()
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        service.Save("Lockout Store", "5839", "493827");

        for (var i = 0; i < SessionService.MaxUnlockAttempts; i++)
            Assert.False(service.VerifyAdmin("0000"));

        Assert.NotNull(session.UnlockLockoutRemaining);
        // While locked out, even the correct PIN must be refused.
        Assert.False(service.VerifyAdmin("5839"));
    }

    [Fact]
    public void Load_MarksFileCorrupt_OnInvalidContent()
    {
        using var dir = new TempDir();
        // Bytes that are neither valid DPAPI ciphertext nor valid JSON.
        File.WriteAllText(Path.Combine(dir.Path, "setup.json"), "{ this is not valid json");
        var service = new SetupService(new SessionService(), dir.Path);
        Assert.Equal(SetupStatus.Corrupt, service.Status);
        Assert.False(service.VerifyAdmin("5839"));
    }

    [Fact]
    public void Save_OverwritesCorruptFile_AndKeepsBackup()
    {
        using var dir = new TempDir();
        var path = Path.Combine(dir.Path, "setup.json");
        File.WriteAllText(path, "{ broken");

        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        Assert.Equal(SetupStatus.Corrupt, service.Status);

        service.Save("Recovered", "5839", "493827");
        Assert.True(service.VerifyAdmin("5839"));
        Assert.True(File.Exists(path + ".bak"));
    }

    [Fact]
    public void Reset_RemovesFile_AndReturnsToNotSetup()
    {
        using var dir = new TempDir();
        var path = Path.Combine(dir.Path, "setup.json");
        var service = new SetupService(new SessionService(), dir.Path);
        service.Save("Reset Store", "5839", "493827");
        Assert.True(File.Exists(path));
        Assert.True(service.IsComplete);

        service.Reset();

        Assert.Equal(SetupStatus.NotSetup, service.Status);
        Assert.False(service.IsComplete);
        Assert.False(File.Exists(path));
        Assert.False(service.VerifyAdmin("5839"));
    }

    [Fact]
    public void Reset_OnNeverSaved_IsIdempotent()
    {
        using var dir = new TempDir();
        var service = new SetupService(new SessionService(), dir.Path);
        Assert.Equal(SetupStatus.NotSetup, service.Status);

        // Should not throw even though there's nothing to delete.
        service.Reset();
        Assert.Equal(SetupStatus.NotSetup, service.Status);
    }

    [Fact]
    public void Reset_Then_NewSave_Works()
    {
        using var dir = new TempDir();
        var session = new SessionService();
        var service = new SetupService(session, dir.Path);
        service.Save("First", "5839", "493827");
        service.Reset();
        service.Save("Second", "7426", "829471");

        Assert.True(service.IsComplete);
        Assert.True(service.VerifyAdmin("7426"));
        Assert.False(service.VerifyAdmin("5839"));   // old PIN no longer accepted
        Assert.Equal("Second", service.Current?.FirmName);
    }

    private sealed class TempDir : IDisposable
    {
        public string Path { get; }

        public TempDir()
        {
            Path = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "sap-test-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path);
        }

        public void Dispose()
        {
            try { Directory.Delete(Path, recursive: true); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
}
