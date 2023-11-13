using Microsoft.Maui.LifecycleEvents;
using SkiaSharp.Views.Maui.Handlers;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Helpers;
#if ANDROID
using Trimble.Modus.Components.Popup.Pages;
#endif
using Trimble.Modus.Components.Handlers;

namespace Trimble.Modus.Components.Hosting;

/// <summary>
/// Extensions for MauiAppBuilder
/// </summary>
public static class AppBuilderExtensions
{
    /// <summary>
    /// Initializes the Trimble Modus Library
    /// </summary>
    /// <param name="builder"><see cref="MauiAppBuilder"/> generated by <see cref="MauiApp"/> </param>
    /// <param name="options"><see cref="Options"/></param>
    /// <returns><see cref="MauiAppBuilder"/> initialized for <see cref="CommunityToolkit.Maui"/></returns>
    public static MauiAppBuilder UseTrimbleModus(this MauiAppBuilder builder)
    {
        builder
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                lifecycle.AddAndroid(d =>
                {
                    d.OnApplicationCreate(del =>
                    {
                        ThemeManager.Initialize();
                    });
                    d.OnBackPressed(activity => Droid.Implementation.AndroidPopups.SendBackPressed());
                });
#elif IOS
                lifecycle.AddiOS(ios =>
                {
                    ios.FinishedLaunching((app, resources) =>
                    {
                        ThemeManager.Initialize();
                        return true;
                    });
                });
#elif MACCATALYST
                lifecycle.AddiOS(mac =>
                {
                    mac.FinishedLaunching((app, resources) =>
                    {
                        ThemeManager.Initialize();
                        return true;
                    });
                });
#elif WINDOWS
                    events.AddWindows(windows => {
                        windows.OnLaunched((window, args) =>
                        {
                            ThemeManager.Initialize();
                        });
                    });

#endif
            })
            .ConfigureMauiHandlers(handlers => SetHandlers(handlers))
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
                fonts.AddFont("OpenSans-ExtraBold.ttf", "OpenSansExtrabold");
                fonts.AddFont("OpenSans-Light.ttf", "OpenSansLight");
                fonts.AddFont("OpenSans-Medium.ttf", "OpenSansMedium");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemibold");
            });
        return builder;
    }

    /// <summary>
    /// Automatically sets up lifecycle events and maui handlers, with the additional option to have additional back press logic
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="backPressHandler"></param>
    /// <returns></returns>
    public static MauiAppBuilder UseTrimbleModus(this MauiAppBuilder builder, Action? backPressHandler)
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
            .ConfigureMauiHandlers(handlers => SetHandlers(handlers))
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
                fonts.AddFont("OpenSans-ExtraBold.ttf", "OpenSansExtrabold");
                fonts.AddFont("OpenSans-Light.ttf", "OpenSansLight");
                fonts.AddFont("OpenSans-Medium.ttf", "OpenSansMedium");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemibold");
            });

        return builder;
    }

    private static void SetHandlers(IMauiHandlersCollection handlers)
    {
        {
            handlers.AddHandler(typeof(BorderlessEntry), typeof(EntryHandler));

            handlers.AddHandler(typeof(Label), typeof(LabelHandler));

            handlers.AddHandler(typeof(TMSpinner), typeof(SpinnerHandler));

            handlers.AddHandler(typeof(BorderlessEditor), typeof(EditorHandler));
            handlers.AddHandler<BaseProgressBar, SKCanvasViewHandler>();


#if ANDROID
            handlers.AddHandler(typeof(PopupPage), typeof(PopupPageHandler));
            handlers.AddHandler(typeof(TMButton), typeof(TMButtonAndroidTouchHandler));
            handlers.AddHandler(typeof(TMFloatingButton), typeof(TMFloatingButtonAndroidTouchHandler));
#endif
#if IOS
                handlers.AddHandler(typeof(PopupPage), typeof(Platforms.iOS.PopupPageHandler));
                handlers.AddHandler(typeof(TMButton), typeof(TMButtoniOSTouchHandler));
                handlers.AddHandler(typeof(TMFloatingButton), typeof(TMFloatingButtoniOSTouchHandler));
#endif
#if WINDOWS
            handlers.AddHandler(typeof(PopupPage), typeof(Platforms.Windows.PopupPageHandler));
                handlers.AddHandler(typeof(TMButton), typeof(TMButtonWindowsTouchHandler));
                handlers.AddHandler(typeof(TMFloatingButton), typeof(TMFloatingButtonWindowsTouchHandler));
#endif
        }
    }
}
