using Trimble.Modus.Components;

namespace Trimble.Modus.Components.Popup.Interfaces;

internal interface IPopupPlatform
{
    Task AddAsync(PopupPage page);

    Task RemoveAsync(PopupPage page);

    public static IViewHandler GetOrCreateHandler<TPopupPageHandler>(VisualElement bindable) where TPopupPageHandler : IViewHandler, new()
    {
        return bindable.Handler ??= new TPopupPageHandler();
    }
}
