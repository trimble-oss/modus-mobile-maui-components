using CommunityToolkit.Maui;
using DemoApp.ViewModels;
using DemoApp.Views;
using Microsoft.Extensions.Logging;
using Trimble.Modus.Components.Hosting;

namespace DemoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseTrimbleModus()
            .ConfigureControlStyling(new List<ResourceDictionary>
            {
            new Resources.Styles.LightThemeStyling(),
            new Resources.Styles.DarkThemeStyling()
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        RegisterViewModel(builder);
        RegisterPages(builder);

        return builder.Build();
    }
    public static void RegisterViewModel(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ProgressBarSamplePageViewModel>();
    }

    public static void RegisterPages(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ProgressBarSamplePage>();
    }
}
