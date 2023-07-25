#if ANDROID
using Android.Views;
#endif
using Microsoft.Maui.Handlers;
#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using System.Numerics;
#endif


namespace Trimble.Modus.Components.Handlers
{
    class ScrollViewHandler : Microsoft.Maui.Handlers.ScrollViewHandler
    {
        public ScrollViewHandler()
        {
            Mapper.AppendToMapping("TMScrollView", MapTmScrollView);
        }
        public void MapTmScrollView(IScrollViewHandler scrollViewHandler, IScrollView scrollView)
        {
#if IOS || MACCATALYST
#elif ANDROID
            scrollViewHandler.PlatformView.ScrollBarSize = 50;
            scrollViewHandler.PlatformView.ScrollbarFadingEnabled = false;
#elif WINDOWS
            scrollViewHandler.PlatformView.VerticalScrollBarVisibility = Microsoft.UI.Xaml.Controls.ScrollBarVisibility.Visible;
#endif
        }
    }
}
