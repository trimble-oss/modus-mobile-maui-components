using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Trimble.Modus.Components.Platforms.Android.Renderers;

namespace Trimble.Modus.Components.Popup.Pages;

internal class PopupPageHandler : PageHandler
{
    public bool _disposed;

    public PopupPageHandler()
    {
        this.SetMauiContext(MauiApplication.Current.Application.Windows[0].Handler.MauiContext);
    }

    protected override void ConnectHandler(ContentViewGroup platformView)
    {
        (platformView as PopupPageRenderer).PopupHandler = this;
        base.ConnectHandler(platformView);
    }

    protected override ContentViewGroup CreatePlatformView()
    {
        return new PopupPageRenderer(Context);
    }


    protected override void DisconnectHandler(ContentViewGroup platformView)
    {
        base.DisconnectHandler(platformView);
    }
}

