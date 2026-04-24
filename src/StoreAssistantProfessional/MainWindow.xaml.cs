using System.Windows;

namespace StoreAssistantProfessional;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Blazor.Services = App.Services;
    }
}
