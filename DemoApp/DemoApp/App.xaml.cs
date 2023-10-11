using Trimble.Modus.Components.Hosting;

namespace DemoApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        AppBuilderExtensions.UseModusTheme(new Resources.Styles.CustomThemeLight(), new Resources.Styles.CustomThemeDark());
        MainPage = new AppShell();
    }
}
