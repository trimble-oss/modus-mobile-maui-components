using DemoApp.Views;
using Trimble.Modus.Components;

namespace DemoApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
