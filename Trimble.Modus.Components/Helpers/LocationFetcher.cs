#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;
#elif IOS
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UIKit;
#elif WINDOWS
using uiXaml = Microsoft.UI.Xaml;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml;
#endif

namespace Trimble.Modus.Components.Helpers
{
    internal class LocationFetcher
    {
        internal Rect GetCoordinates(VisualElement view)
        {
            Rect result = new Rect();
#if ANDROID
            if (view?.Handler?.PlatformView is not AView platformView)
                return new Rect();

            int[] location = new int[2];
            platformView.GetLocationOnScreen(location);
            var context = platformView.Context;

            result = new Rect(
                context.FromPixels(location[0]),
                context.FromPixels(location[1]),
                context.FromPixels(platformView.MeasuredWidth),
                context.FromPixels(platformView.MeasuredHeight));
#elif IOS
            if (view?.Handler?.PlatformView is not UIView platformView)
                return new Rect();

            var superview = platformView;

            while (superview.Superview is not null)
                superview = superview.Superview;

            var convertPoint = platformView.ConvertRectToView(platformView.Bounds, superview);

            result = new Rect(
                convertPoint.X,
                convertPoint.Y,
                convertPoint.Width,
                convertPoint.Height);
#elif WINDOWS
            if (view?.Handler?.PlatformView is not uiXaml.FrameworkElement platformView)
                return Rect.Zero; 

            var rootView = platformView.XamlRoot?.Content as uiXaml.FrameworkElement;
            if (rootView == null)
                return Rect.Zero;

            var position = platformView.TransformToVisual(rootView).TransformPoint(new global::Windows.Foundation.Point());

            result = new Rect(
              position.X,
              position.Y,
              platformView.ActualWidth,
              platformView.ActualHeight);

#endif
            return result;
        }
    }
}
