#if WINDOWS
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.UI.Xaml.Input;
using Trimble.Modus.Components.Controls.Button;

namespace Trimble.Modus.Components.Handlers
{
    public class TMButtonWindowsTouchHandler : VisualElementRenderer<TMButton, Microsoft.UI.Xaml.FrameworkElement>
    {
        public TMButtonWindowsTouchHandler()
        {
            PointerPressed += Control_PointerPressed;
            PointerReleased += Control_PointerReleased;
            PointerExited += Control_PointerCanceled;
        }

        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!Element.IsDisabled)
            {
                Element?.RaisePressed();
            }
            
        }

        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!Element.IsDisabled)
            {
                Element?.RaiseReleased();
            }
        }

        private void Control_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            if (!Element.IsDisabled)
            {
                Element?.RaiseCancel();
            }

        }
        protected override void Dispose(bool disposing)
        {
            PointerPressed -= Control_PointerPressed;
            PointerReleased -= Control_PointerReleased;
            PointerExited -= Control_PointerCanceled;

            base.Dispose(disposing);
        }

    }
}
#endif
