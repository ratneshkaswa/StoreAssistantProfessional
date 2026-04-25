using System.IO;

namespace StoreAssistantProfessional;

// Single source of truth for the on-disk LocalAppData layout. Both the SQLite
// database (AppDbContext) and the setup file (SetupService) live under this
// folder; previously each owned its own literal "StoreAssistantProfessional".
public static class AppPaths
{
    public const string AppDataFolderName = "StoreAssistantProfessional";

    public static string AppDataDir
    {
        get
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                AppDataFolderName);
            Directory.CreateDirectory(dir);
            return dir;
        }
    }
}
