#if WINDOWS
using Microsoft.UI.Xaml;
#endif
namespace Trimble.Modus.Components
{
    public class SpinnerDisposeBehavior : PlatformBehavior<TMSpinner>
    {
        public SpinnerDisposeBehavior() { }
#if WINDOWS
        protected override void OnAttachedTo(TMSpinner bindable, FrameworkElement platformView)
        {
            base.OnAttachedTo(bindable, platformView);
            bindable.isDisposed = false;
            bindable.StartAnimation();
        }
        protected override void OnDetachedFrom(TMSpinner bindable, FrameworkElement platformView)
        {
            bindable.isDisposed = true;
        }
#endif
#if ANDROID
        protected override void OnAttachedTo(TMSpinner bindable, Android.Views.View platformView)
        {
            base.OnAttachedTo(bindable, platformView);
            bindable.isDisposed = false;
            bindable.StartAnimation();
        }
        protected override void OnDetachedFrom(TMSpinner bindable, Android.Views.View platformView)
        {
            bindable.isDisposed = true;
        }
#endif
#if IOS
        protected override void OnAttachedTo(TMSpinner bindable, UIKit.UIView platformView)
        {
            base.OnAttachedTo(bindable, platformView);
            bindable.isDisposed = false;
            bindable.StartAnimation();
        }
        protected override void OnDetachedFrom(TMSpinner bindable, UIKit.UIView platformView)
        {
            bindable.isDisposed = true;
        }
#endif


    }

}
