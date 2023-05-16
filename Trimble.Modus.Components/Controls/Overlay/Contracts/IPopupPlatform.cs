using Trimble.Modus.Components.Overlay.Pages;

namespace Trimble.Modus.Components.Overlay.Interfaces;

public interface IPopupPlatform
{
    Task AddAsync(PopupPage page);

    Task RemoveAsync(PopupPage page);

    public static IViewHandler GetOrCreateHandler<TPopupPageHandler>(VisualElement bindable) where TPopupPageHandler : IViewHandler, new()
    {
        return bindable.Handler ??= new TPopupPageHandler();
    }
}
