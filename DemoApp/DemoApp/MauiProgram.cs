using Microsoft.Extensions.Logging;
using Trimble.Modus.Components.Popup.Hosting;
using Trimble.Modus.Components;

namespace DemoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseTrimbleModus()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
