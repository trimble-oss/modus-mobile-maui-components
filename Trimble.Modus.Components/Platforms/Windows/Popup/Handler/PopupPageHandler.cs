using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Trimble.Modus.Components.Platforms.Windows;

internal class PopupPageHandler : PageHandler
{
    public PopupPageHandler()
    {
    }

    protected override ContentPanel CreatePlatformView()
    {
        return new PopupPageRenderer(this);
    }
}
