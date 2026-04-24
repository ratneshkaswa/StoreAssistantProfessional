using System.Globalization;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
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

        Services = services.BuildServiceProvider();

        base.OnStartup(e);

        new MainWindow().Show();
    }
}
