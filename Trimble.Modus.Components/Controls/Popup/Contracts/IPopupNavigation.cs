using Trimble.Modus.Components.Popup.Events;
using Trimble.Modus.Components;

namespace Trimble.Modus.Components.Popup.Interfaces;

public interface IPopupNavigation
{
    event EventHandler<PopupNavigationEventArgs> Presenting;

    event EventHandler<PopupNavigationEventArgs> Presented;

    event EventHandler<PopupNavigationEventArgs> Dismissing;

    event EventHandler<PopupNavigationEventArgs> Dismissed;

    IReadOnlyList<PopupPage> PopupStack { get; }

    Task PresentAsync(PopupPage page, bool animate = true);

    Task DismissAsync(bool animate = true);

    Task PresentAllAsync(bool animate = true);

    Task RemovePageAsync(PopupPage page, bool animate = true);
}
