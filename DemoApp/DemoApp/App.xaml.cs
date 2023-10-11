using Trimble.Modus.Components.Hosting;

namespace DemoApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        AppBuilderExtensions.UseModusTheme();
        MainPage = new AppShell();
    }
}
