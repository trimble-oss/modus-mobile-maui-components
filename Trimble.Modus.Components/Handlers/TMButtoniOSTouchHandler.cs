#if IOS
using Foundation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Trimble.Modus.Components;
using UIKit;

namespace Trimble.Modus.Components.Handlers;

public class TMButtoniOSTouchHandler : VisualElementRenderer<TMButton>
{
    public TMButtoniOSTouchHandler()
    {    
       UserInteractionEnabled = true;
    }

    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        base.TouchesBegan(touches, evt);
        if (!Element.IsDisabled && !Element.IsLoading)
        {
            Element?.RaisePressed();
        }
    }

    public override void TouchesCancelled(NSSet touches, UIEvent evt)
    {
        base.TouchesCancelled(touches, evt);
        if (!Element.IsDisabled && !Element.IsLoading)
        {
            Element?.RaiseCancel();
        }
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
        base.TouchesEnded(touches, evt);
        if (!Element.IsDisabled && !Element.IsLoading)
        {
            Element?.RaiseReleased();
        }
    }
}
#endif
