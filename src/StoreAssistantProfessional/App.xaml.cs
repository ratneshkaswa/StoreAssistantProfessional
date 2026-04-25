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

        try
        {
            var services = new ServiceCollection();
            services.AddWpfBlazorWebView();
            services.AddMudServices();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<ISetupService, SetupService>();

            services.AddDbContextFactory<AppDbContext>(opt =>
                opt.UseSqlite($"Data Source={AppDbContext.DefaultDbPath}"));
            services.AddSingleton<IOnboardingService, OnboardingService>();
            services.AddSingleton<IShellState, ShellState>();
            services.AddSingleton<IBackupService, BackupService>();
            services.AddSingleton<IAppSettingsService, AppSettingsService>();
            services.AddSingleton<IPermissionService, PermissionService>();
            services.AddSingleton<IActivityService, ActivityService>();
            services.AddSingleton<ICounterPresenceService, CounterPresenceService>();
            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<ITallyExportService, TallyExportService>();
            services.AddSingleton<IBackgroundScheduler, BackgroundScheduler>();

            Services = services.BuildServiceProvider();

            using (var db = Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext())
            {
                db.Database.EnsureCreated();
                SchemaUpdater.ApplyAsync(db).GetAwaiter().GetResult();
            }

            _ = Services.GetRequiredService<IBackgroundScheduler>();
            _ = Services.GetRequiredService<ICounterPresenceService>();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Store Assistant Professional could not start:\n\n{ex.Message}\n\n" +
                "Check that %LocalAppData%\\StoreAssistantProfessional is writable and that the database file is not locked by another process.",
                "Startup error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(1);
            return;
        }

        base.OnStartup(e);

        new MainWindow().Show();
    }
}
