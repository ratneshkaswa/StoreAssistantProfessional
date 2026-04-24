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

    private void OnBlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
    {
#if !DEBUG
        var settings = e.WebView.CoreWebView2.Settings;
        settings.AreDevToolsEnabled = false;
        settings.AreDefaultContextMenusEnabled = false;
#endif

        e.WebView.CoreWebView2.DocumentTitleChanged += (_, _) =>
        {
            var docTitle = e.WebView.CoreWebView2.DocumentTitle;
            if (!string.IsNullOrWhiteSpace(docTitle))
                Dispatcher.Invoke(() => Title = docTitle);
        };
    }
}
