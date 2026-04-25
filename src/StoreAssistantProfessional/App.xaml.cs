using System.Globalization;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using StoreAssistantProfessional.Data;
using StoreAssistantProfessional.Services;

namespace StoreAssistantProfessional;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        var inIN = new CultureInfo("en-IN");
        CultureInfo.DefaultThreadCurrentCulture = inIN;
        CultureInfo.DefaultThreadCurrentUICulture = inIN;

        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
        services.AddMudServices();
        services.AddSingleton<ISetupService, SetupService>();
        services.AddSingleton<ISessionService, SessionService>();

        services.AddDbContextFactory<AppDbContext>(opt =>
            opt.UseSqlite($"Data Source={AppDbContext.DefaultDbPath}"));
        services.AddSingleton<IOnboardingService, OnboardingService>();
        services.AddSingleton<IShellState, ShellState>();
        services.AddSingleton<IBackupService, BackupService>();
        services.AddScoped<IAppSettingsService, AppSettingsService>();
        services.AddSingleton<IActivityService, ActivityService>();
        services.AddSingleton<ICounterPresenceService, CounterPresenceService>();
        services.AddSingleton<ICartService, CartService>();
        services.AddScoped<ITallyExportService, TallyExportService>();

        Services = services.BuildServiceProvider();

        using (var db = Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext())
        {
            db.Database.EnsureCreated();
        }

        base.OnStartup(e);

        new MainWindow().Show();
    }
}
