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
                return new Rect();

            if (platformView == null)
                return new Rect();

            var rootView = platformView.XamlRoot.Content;
            if (platformView == rootView)
            {
                if (rootView is not uiXaml.FrameworkElement el)
                    return new Rect();

                return new Rect(0, 0, el.ActualWidth, el.ActualHeight);
            }

            var topLeft = platformView.TransformToVisual(rootView).TransformPoint(new global::Windows.Foundation.Point());
            var topRight = platformView.TransformToVisual(rootView).TransformPoint(new global::Windows.Foundation.Point(platformView.ActualWidth, 0));
            var bottomLeft = platformView.TransformToVisual(rootView).TransformPoint(new global::Windows.Foundation.Point(0, platformView.ActualHeight));
            var bottomRight = platformView.TransformToVisual(rootView).TransformPoint(new global::Windows.Foundation.Point(platformView.ActualWidth, platformView.ActualHeight));

            var x1 = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Min();
            var x2 = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Max();
            var y1 = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Min();
            var y2 = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Max();
            result = new Rect();
#endif
            return result;
        }

    }
}
