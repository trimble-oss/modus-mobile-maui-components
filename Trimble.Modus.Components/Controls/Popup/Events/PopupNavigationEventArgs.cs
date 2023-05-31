using Trimble.Modus.Components.Popup.Pages;

namespace Trimble.Modus.Components.Popup.Events;

public class PopupNavigationEventArgs : EventArgs
{
    public PopupPage Page { get; }

    public bool IsAnimated { get; }

    public PopupNavigationEventArgs(PopupPage page, bool isAnimated)
    {
        Page = page;
        IsAnimated = isAnimated;
    }
}
