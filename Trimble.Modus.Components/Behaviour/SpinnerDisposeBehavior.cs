#if WINDOWS
using Microsoft.UI.Xaml;

namespace Trimble.Modus.Components
{
    public class SpinnerDisposeBehavior : PlatformBehavior<TMSpinner>
    {
        public SpinnerDisposeBehavior() { }
        protected override void OnAttachedTo(TMSpinner bindable, FrameworkElement platformView)
        {
            base.OnAttachedTo(bindable, platformView);
            bindable.isDisposed = false;
        }
        protected override void OnDetachedFrom(TMSpinner bindable, FrameworkElement platformView)
        {
            bindable.isDisposed = true;
        }

    }

}
#endif
