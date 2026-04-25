using System.IO;
using StoreAssistantProfessional.Services;
using Xunit;

namespace StoreAssistantProfessional.Tests;

public class SetupServiceTests
{
    [Theory]
    [InlineData("0123", true)]
    [InlineData("9876", true)]
    [InlineData("", true)]
    [InlineData("abc", false)]
    [InlineData("12 4", false)]
    [InlineData("१२३४", false)]   // Devanagari १२३४
    [InlineData("１２３４", false)]   // Fullwidth １２３４
    public void IsAsciiDigits_OnlyAcceptsAscii09(string input, bool expected)
    {
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

    [Fact]
    public void Save_RejectsShortStoreName()
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
    public void Load_MarksFileCorrupt_OnInvalidJson()
    {
        using var dir = new TempDir();
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
