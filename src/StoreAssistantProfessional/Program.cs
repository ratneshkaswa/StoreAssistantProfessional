using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Photino.Blazor;
using StoreAssistantProfessional.Components;

namespace StoreAssistantProfessional;

public static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        var inIN = new CultureInfo("en-IN");
        CultureInfo.DefaultThreadCurrentCulture = inIN;
        CultureInfo.DefaultThreadCurrentUICulture = inIN;

        var builder = PhotinoBlazorAppBuilder.CreateDefault(args);
        builder.Services.AddMudServices();
        builder.RootComponents.Add<Routes>("#app");

        var app = builder.Build();
        app.MainWindow
            .SetTitle("Store Assistant Professional")
            .SetUseOsDefaultSize(false)
            .SetSize(1200, 800)
            .SetMaximized(true)
            .Center();

        app.Run();
        return 0;
    }
}
