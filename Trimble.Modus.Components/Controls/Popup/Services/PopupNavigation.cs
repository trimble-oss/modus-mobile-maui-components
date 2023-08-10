using Trimble.Modus.Components.Popup.Events;
using Trimble.Modus.Components.Popup.Interfaces;
using Trimble.Modus.Components;

namespace Trimble.Modus.Components.Popup.Services;

internal class PopupNavigation : IPopupNavigation
{
    private readonly object _locker = new();

    public IReadOnlyList<PopupPage> PopupStack => _popupStack;
    private readonly List<PopupPage> _popupStack = new();

    public event EventHandler<PopupNavigationEventArgs>? Presenting;

    public event EventHandler<PopupNavigationEventArgs>? Presented;

    public event EventHandler<PopupNavigationEventArgs>? Dismissing;

    public event EventHandler<PopupNavigationEventArgs>? Dismissed;

    private static readonly Lazy<IPopupPlatform> lazyImplementation = new(() => GeneratePopupPlatform(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    private readonly IPopupPlatform PopupPlatform = lazyImplementation.Value;

    private static IPopupPlatform GeneratePopupPlatform()
    {
        return PullPlatformImplementation();


        static IPopupPlatform PullPlatformImplementation()
        {
#if ANDROID
            return new Trimble.Modus.Components.Droid.Implementation.AndroidPopups();
#elif IOS
            return new Trimble.Modus.Components.iOS.Implementation.iOSPopups();
#elif MACCATALYST
            return new Trimble.Modus.Components.MacCatalyst.Implementation.MacOSMopups();
#elif WINDOWS
            return new Trimble.Modus.Components.Windows.Implementation.PopupPlatformWindows();
#endif

            throw new PlatformNotSupportedException();
        }
    }

    private void OnInitialized(object? sender, EventArgs e)
    {
        if (_popupStack.Count > 0)
        {
            PresentAllAsync();
        }
    }



    public Task PresentAsync(PopupPage page, bool animate = true)
    {
        Presenting?.Invoke(this, new PopupNavigationEventArgs(page, animate));
        _popupStack.Add(page);

        return MainThread.IsMainThread
            ? PushPage()
            : MainThread.InvokeOnMainThreadAsync(PushPage);

        async Task PushPage()
        {
            page.PreparingAnimation();
            await PopupPlatform.AddAsync(page);

            //Hack to make the popup to render within safe area
            if (page.HasSystemPadding)
            {
                page.Padding = new Thickness(page.SystemPadding.Left, page.SystemPadding.Top, page.SystemPadding.Right, page.SystemPadding.Bottom);
            }

            page.SendAppearing();
            await page.AppearingAnimation();
            Presented?.Invoke(this, new PopupNavigationEventArgs(page, animate));
        };
    }

    public async Task PresentAllAsync(bool animate = true)
    {
        while (PopupService.Instance.PopupStack.Count > 0)
        {
            await DismissAsync(animate);
        }
    }

    public Task DismissAsync(bool animate = true)
    {
        return _popupStack.Count <= 0
            ? throw new InvalidOperationException("PopupStack is empty")
            : RemovePageAsync(PopupStack[PopupStack.Count - 1], animate);
    }

    public Task RemovePageAsync(PopupPage page, bool animate = true)
    {
        if (page == null)
            throw new InvalidOperationException("Page can not be null");

        if (!_popupStack.Contains(page))
            throw new InvalidOperationException("The page has not been pushed yet or has been removed already");

        return (MainThread.IsMainThread
            ? RemovePage()
            : MainThread.InvokeOnMainThreadAsync(RemovePage));


        async Task RemovePage()
        {
            lock (_locker)
            {
                if (!_popupStack.Contains(page))
                {
                    return;
                }
            }

            Dismissing?.Invoke(this, new PopupNavigationEventArgs(page, animate));
            await page.DisappearingAnimation();
            page.SendDisappearing();
            await PopupPlatform.RemoveAsync(page);
            page.DisposingAnimation();

            _popupStack.Remove(page);
            Dismissed?.Invoke(this, new PopupNavigationEventArgs(page, animate));
        }
    }
}

