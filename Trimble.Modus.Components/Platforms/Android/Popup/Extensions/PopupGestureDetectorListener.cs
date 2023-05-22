using Android.Views;

namespace Trimble.Modus.Components.Droid.Gestures;

internal class PopupGestureDetectorListener : GestureDetector.SimpleOnGestureListener
{
    public event EventHandler<MotionEvent>? Clicked;

    public override bool OnSingleTapUp(MotionEvent? e)
    {
        if (e != null) Clicked?.Invoke(this, e);

        return false;
    }
}
