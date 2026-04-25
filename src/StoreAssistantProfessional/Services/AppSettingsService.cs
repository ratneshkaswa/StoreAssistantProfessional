using Microsoft.EntityFrameworkCore;
using StoreAssistantProfessional.Data;
using StoreAssistantProfessional.Models;

namespace StoreAssistantProfessional.Services;

public interface IAppSettingsService
{
    Task<AppSettings> LoadAsync();
    Task SaveAsync(AppSettings settings);
}

public sealed class AppSettingsService : IAppSettingsService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly IShellState _shell;

    public AppSettingsService(IDbContextFactory<AppDbContext> dbFactory, IShellState shell)
    {
        _dbFactory = dbFactory;
        _shell = shell;
    }

    public async Task<AppSettings> LoadAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var existing = await db.Settings.FirstOrDefaultAsync();
        if (existing is null)
        {
            existing = new AppSettings { Id = AppSettings.SingletonId };
            db.Settings.Add(existing);
            await db.SaveChangesAsync();
        }
        return existing;
    }

    public async Task SaveAsync(AppSettings settings)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var existing = await db.Settings.FirstOrDefaultAsync();
        if (existing is null)
        {
            settings.Id = AppSettings.SingletonId;
            settings.UpdatedAt = DateTime.UtcNow;
            db.Settings.Add(settings);
        }
        else
        {
            settings.Id = existing.Id;
            settings.UpdatedAt = DateTime.UtcNow;
            db.Entry(existing).CurrentValues.SetValues(settings);
        }
        await db.SaveChangesAsync();

        var mode = settings.CashMode == "PettyBox" ? CashMode.PettyBox : CashMode.SingleDrawer;
        _shell.SetCashMode(mode);
    }
}
