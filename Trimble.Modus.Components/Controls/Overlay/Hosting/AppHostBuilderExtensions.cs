using Microsoft.Maui.LifecycleEvents;
using Trimble.Modus.Components.Overlay.Pages;

namespace Trimble.Modus.Components.Overlay.Hosting;

/// <summary>
/// Represents application host extension, that used to configure handlers defined in Popups.
/// </summary>
public static class AppHostBuilderExtensions
{
    /// <summary>
    /// Automatically sets up lifecycle events and Maui Handlers
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static MauiAppBuilder ConfigurePopups(this MauiAppBuilder builder)
    {
        builder
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                lifecycle.AddAndroid(d =>
                {
                    d.OnBackPressed(activity => Droid.Implementation.AndroidPopups.SendBackPressed());
                });

#endif
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler(typeof(PopupPage), typeof(PopupPageHandler));
#endif
#if IOS
                handlers.AddHandler(typeof(PopupPage), typeof(Platforms.iOS.PopupPageHandler));
#endif
#if WINDOWS
                handlers.AddHandler(typeof(PopupPage), typeof(Platforms.Windows.PopupPageHandler));
#endif
            });
        return builder;
    }


    /// <summary>
    /// Automatically sets up lifecycle events and maui handlers, with the additional option to have additional back press logic
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="backPressHandler"></param>
    /// <returns></returns>
    public static MauiAppBuilder ConfigurePopups(this MauiAppBuilder builder, Action? backPressHandler)
    {
        builder
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                lifecycle.AddAndroid(d =>
                {
                    
                    d.OnBackPressed(activity => Droid.Implementation.AndroidPopups.SendBackPressed(backPressHandler));
                });
#endif
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler(typeof(PopupPage), typeof(PopupPageHandler));
#endif
            });
        return builder;
    }

}
