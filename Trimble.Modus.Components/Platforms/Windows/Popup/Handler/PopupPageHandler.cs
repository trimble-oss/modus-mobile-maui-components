using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Trimble.Modus.Components.Platforms.Windows
{
    public class PopupPageHandler : PageHandler
    {
        public PopupPageHandler()
        {
        }

        protected override ContentPanel CreatePlatformView()
        {
            return new PopupPageRenderer(this);
        }
    }
}
