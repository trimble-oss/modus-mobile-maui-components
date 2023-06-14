#if IOS
using Foundation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Trimble.Modus.Components.Controls.Button;
using UIKit;

namespace Trimble.Modus.Components.Handlers;

public class TMButtoniOSTouchHandler : VisualElementRenderer<CustomButton>
{
    public TMButtoniOSTouchHandler()
    {
        UserInteractionEnabled = true;
    }

    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        base.TouchesBegan(touches, evt);

        Element?.RaisePressed();
    }

    public override void TouchesCancelled(NSSet touches, UIEvent evt)
    {
        base.TouchesCancelled(touches, evt);

        Element?.RaiseCancel();
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
        base.TouchesEnded(touches, evt);

        Element?.RaiseReleased();
    }
}
#endif
