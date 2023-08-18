using System;
namespace DemoApp.Helpers
{
    public class ServiceProviderUtils
    {
        public static TService GetService<TService>() =>
                    Current.GetService<TService>();

        public static IServiceProvider Current =>
#if ANDROID
    MauiApplication.Current.Services;
#elif IOS || MACCATALYST
			MauiUIApplicationDelegate.Current.Services;
#else
            _current;
#endif    
    }
}

