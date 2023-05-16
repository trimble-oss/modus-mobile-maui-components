using Microsoft.Extensions.Logging;
using Trimble.Modus.Components.Overlay.Hosting;
using Trimble.Modus.Components;

namespace DemoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigurePopups()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers(handlers =>
             {
                 // TODO: Should add a common method to add all handlers
                 handlers.AddHandler(typeof(BorderlessEntry), typeof(BorderlessEntryHandler));
             });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
