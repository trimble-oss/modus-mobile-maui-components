using System.Transactions;
using Trimble.Modus.Components.Popup.Animations.Base;

namespace Trimble.Modus.Components.Popup.Animations;

public class RevealAnimation : BaseAnimation
{
    private double desiredHeight;
    private double _defaultOpacity;

    public bool HasBackgroundAnimation { get; set; } = true;

    public RevealAnimation(double desiredHeight)
    {
        this.desiredHeight = desiredHeight;
    }
    public override void Preparing(View content, PopupPage page)
    {
        if (HasBackgroundAnimation)
        {
            _defaultOpacity = page.Opacity;
            page.HeightRequest = 0;
        }
        else if (content != null)
        {
            _defaultOpacity = content.Opacity;
            content.Opacity = 1;
        }
    }

    public override void Disposing(View content, PopupPage page)
    {
        if (HasBackgroundAnimation || content != null)
        {
            page.Opacity = _defaultOpacity;
        }
    }

    public override Task Appearing(View content, PopupPage page)
    {
        if (HasBackgroundAnimation)
        {

            return RevealAnimations.HeightTo(content, desiredHeight);
        }
        if (content != null)
        {
            return RevealAnimations.HeightTo(content, desiredHeight);
        }

        return Task.CompletedTask;
    }

    public override Task Disappearing(View content, PopupPage page)
    {
        _defaultOpacity = page.Opacity;
        if (double.IsNaN(_defaultOpacity))
            _defaultOpacity = 1;

        if (HasBackgroundAnimation)
        {
            return RevealAnimations.HeightTo(content, 0, 0);
        }
        if (content != null)
        {
            return RevealAnimations.HeightTo(content, 0, 0);
        }

        return Task.CompletedTask;
    }
}
public static class RevealAnimations
{
    public static async Task<bool> HeightTo(this View view, double height, uint duration = 250, Easing easing = null)
    {
        var tcs = new TaskCompletionSource<bool>();

        var heightAnimation = new Animation(x => view.HeightRequest = x, view.Height, height);
        heightAnimation.Commit(view, "HeightAnimation", 10, duration, easing, (finalValue, finished) => { tcs.SetResult(finished); });

        return await tcs.Task;
    }

    public static async Task<bool> WidthTo(this View view, double width, uint duration = 250, Easing easing = null)
    {
        var tcs = new TaskCompletionSource<bool>();

        var heightAnimation = new Animation(x => view.WidthRequest = x, view.Height, width);
        heightAnimation.Commit(view, "WidthAnimation", 10, duration, easing, (finalValue, finished) => { tcs.SetResult(finished); });

        return await tcs.Task;
    }
}
