using Trimble.Modus.Components.Popup.Events;
using Trimble.Modus.Components.Popup.Pages;

namespace Trimble.Modus.Components.Popup.Interfaces;

internal interface IPopupNavigation
{
    event EventHandler<PopupNavigationEventArgs> Pushing;

    event EventHandler<PopupNavigationEventArgs> Pushed;

    event EventHandler<PopupNavigationEventArgs> Popping;

    event EventHandler<PopupNavigationEventArgs> Popped;

    IReadOnlyList<PopupPage> PopupStack { get; }

    Task PushAsync(PopupPage page, bool animate = true);

    Task PopAsync(bool animate = true);

    Task PopAllAsync(bool animate = true);

    Task RemovePageAsync(PopupPage page, bool animate = true);
}
