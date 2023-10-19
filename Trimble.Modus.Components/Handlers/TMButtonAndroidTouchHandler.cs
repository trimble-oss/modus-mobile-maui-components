#if ANDROID
using Trimble.Modus.Components;
using Android.Content;
using Android.Views;
using Microsoft.Maui.Controls.Handlers.Compatibility;

namespace Trimble.Modus.Components.Handlers;

public class TMButtonAndroidTouchHandler : VisualElementRenderer<TMButton>
{
    public TMButtonAndroidTouchHandler(Context context) : base(context)
    {
        Touch += Control_Touch;
    }

    private void Control_Touch(object sender, TouchEventArgs e)
    {
        if (!Element.IsDisabled && !Element.IsLoading)
        {
          switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    Element?.RaisePressed();
                    break;

                case MotionEventActions.Up:
                    Element?.RaiseReleased();
                    break;

                case MotionEventActions.Move:
                    Element?.RaiseCancel();
                    break;
                default:
                    break;
            }
        }

    }
    protected override void Dispose(bool disposing)
    {
        Touch -= Control_Touch;
        base.Dispose(disposing);
    }
}
#endif

