using System.Windows;
using Microsoft.AspNetCore.Components.WebView;

namespace StoreAssistantProfessional;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Blazor.Services = App.Services;
        Blazor.BlazorWebViewInitialized += OnBlazorWebViewInitialized;
    }

    private static void OnBlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
    {
        var settings = e.WebView.CoreWebView2.Settings;
        settings.AreDevToolsEnabled = false;
        settings.AreDefaultContextMenusEnabled = false;
    }
}
