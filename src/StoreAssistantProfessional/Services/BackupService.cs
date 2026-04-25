using System.IO;
using System.IO.Compression;

namespace StoreAssistantProfessional.Services;

public interface IBackupService
{
    string BackupDirectory { get; }
    string CreateBackup();
    DateTime? LastBackupAt { get; }
}

public sealed class BackupService : IBackupService
{
    public string BackupDirectory
    {
        get
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "StoreAssistantProfessional",
                "Backups");
            Directory.CreateDirectory(dir);
            return dir;
        }
    }

    public DateTime? LastBackupAt
    {
        get
        {
            try
            {
                var files = Directory.GetFiles(BackupDirectory, "backup-*.zip");
                if (files.Length == 0) return null;
                return files
                    .Select(File.GetLastWriteTime)
                    .Max();
            }
            catch (IOException) { return null; }
            catch (UnauthorizedAccessException) { return null; }
        }
    }

    public string CreateBackup()
    {
        var sourceDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StoreAssistantProfessional");

        var stamp = DateTime.Now.ToString("yyyy-MM-dd-HHmm");
        var target = Path.Combine(BackupDirectory, $"backup-{stamp}.zip");

        if (File.Exists(target)) File.Delete(target);

        if (!Directory.Exists(sourceDir))
            throw new DirectoryNotFoundException($"App data folder not found: {sourceDir}");

        var stagingZip = target + ".tmp";
        if (File.Exists(stagingZip)) File.Delete(stagingZip);

        using (var zip = ZipFile.Open(stagingZip, ZipArchiveMode.Create))
        {
            foreach (var file in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                var name = file.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                try
                {
                    zip.CreateEntryFromFile(file, name, CompressionLevel.Optimal);
                }
                catch (IOException) { }
            }
        }

        File.Move(stagingZip, target);
        return target;
    }
}
