using Trimble.Modus.Components.Overlay.Pages;

namespace Trimble.Modus.Components.Overlay.Events;

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
