using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMSwitch : ContentView
{
    protected EventHandler<TMSwitchEventArgs> _clicked;
    private TapGestureRecognizer tapGestureRecognizer;
    public static readonly BindableProperty SwitchSizeProperty =
        BindableProperty.Create(nameof(SwitchSize), typeof(SwitchSize), typeof(TMSwitch), SwitchSize.Medium, propertyChanged: OnSwitchSizeChanged);
    public static readonly BindableProperty IsToggledProperty =
       BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(TMSwitch), false, propertyChanged: OnSwitchToggleChanged);
    public new static readonly BindableProperty IsEnabledProperty =
      BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMSwitch), true, propertyChanged: OnIsEnabledChanged);
    public static readonly BindableProperty ToggledCommandProperty =
      BindableProperty.Create(nameof(ToggledCommand), typeof(ICommand), typeof(TMSwitch), null);
    public static readonly BindableProperty LabelTextProperty =
      BindableProperty.Create(nameof(Text), typeof(string), typeof(TMSwitch), "", propertyChanged: OnLabelTextChanged);

    public string Text
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }
    public ICommand ToggledCommand
    {
        get => (ICommand)GetValue(ToggledCommandProperty);
        set => SetValue(ToggledCommandProperty, value);
    }
    public event EventHandler<TMSwitchEventArgs> Toggled
    {
        add { _clicked += value; }
        remove { _clicked -= value; }
    }
    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }
    public SwitchSize SwitchSize
    {
        get => (SwitchSize)GetValue(SwitchSizeProperty);
        set => SetValue(SwitchSizeProperty, value);
    }
    public bool IsToggled
    {
        get => (bool)GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
    }
    public TMSwitch()
    {
        InitializeComponent();
        UpdateSwitch(this);
        UpdateSwitchSize(this);
    }
    private static void OnIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMSwitch tMSwitch)
        {
            if ((bool)newValue)
            {
                tMSwitch.container.Opacity = 1;
                tMSwitch.border.GestureRecognizers.Add(tMSwitch.tapGestureRecognizer);
            }
            else
            {
                tMSwitch.container.Opacity = 0.5;
                tMSwitch.border.GestureRecognizers.Clear();
            }

        }
    }
    private static void OnLabelTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMSwitch tMSwitch)
        {
            if (string.IsNullOrEmpty((string)newValue))
            {
                tMSwitch.container.Spacing = 0;
                tMSwitch.switchText.IsVisible = false;
            }
            else
            {
                tMSwitch.container.Spacing = 8;
                tMSwitch.switchText.IsVisible = true;
                tMSwitch.switchText.Text = (string)newValue;
            }

        }
    }
    private static void OnSwitchSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMSwitch tMSwitch)
        {
            UpdateSwitchSize(tMSwitch);
        }
    }
    private static void OnSwitchToggleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMSwitch tMSwitch)
        {
            if ((bool)newValue)
            {
                OnSwitchSelected(tMSwitch);
            }
            else
            {
                OnSwitchUnSelected(tMSwitch);
            }
        }
    }
    private void UpdateSwitch(TMSwitch tMSwitch)
    {

        tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnSwitchTapped;
        border.GestureRecognizers.Add(tapGestureRecognizer);
        tMSwitch.border.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.SwitchUnselected);
        tMSwitch.border.Padding = 5;
    }

    private static void UpdateSwitchSize(TMSwitch tMSwitch)
    {
        tMSwitch.border.Padding = 6;
        if (tMSwitch.SwitchSize == SwitchSize.Medium)
        {
            tMSwitch.border.HeightRequest = 24;
            tMSwitch.border.WidthRequest = 48;
            tMSwitch.circle.HeightRequest = 12;
            tMSwitch.circle.WidthRequest = 12;
        }
        else
        {
            tMSwitch.border.HeightRequest = 32;
            tMSwitch.border.WidthRequest = 64;
            tMSwitch.circle.HeightRequest = 16;
            tMSwitch.circle.WidthRequest = 16;
        }
    }

    private void OnSwitchTapped(object sender, TappedEventArgs e)
    {
        IsToggled = !IsToggled;
    }
    private static void OnSwitchSelected(TMSwitch tMSwitch)
    {
        tMSwitch._clicked?.Invoke(tMSwitch, new TMSwitchEventArgs(true));
        tMSwitch.ToggledCommand?.Execute(new TMSwitchEventArgs(true));

        if (tMSwitch.SwitchSize == SwitchSize.Medium)
        {
            tMSwitch.Animate("SelectedAnimation",
                new Animation
                {
            { 0, 1, new Animation(v => tMSwitch.circle.Scale = v, 1.0, 1.36) },
            { 0, 1, new Animation(v => tMSwitch.circle.TranslationX = v, 0,tMSwitch.border.Width - (tMSwitch.circle.Width +13.6) ) }
                },
                length: 250, easing: Easing.CubicIn);
        }
        else
        {
            tMSwitch.Animate("SelectedAnimation",
                new Animation
                {
            { 0, 1, new Animation(v => tMSwitch.circle.Scale = v, 1.0, 1.38) },
            { 0, 1, new Animation(v => tMSwitch.circle.TranslationX = v, 0,tMSwitch.border.Width - (tMSwitch.circle.Width +14.8) ) }
                },
                length: 250, easing: Easing.CubicIn);
        }
        tMSwitch.border.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.SwitchSelected);
        tMSwitch.border.Padding = 6;
    }
    private static void OnSwitchUnSelected(TMSwitch tMSwitch)
    {
        tMSwitch._clicked?.Invoke(tMSwitch, new TMSwitchEventArgs(false));
        tMSwitch.ToggledCommand?.Execute(new TMSwitchEventArgs(false));
        tMSwitch.Animate("UnselectedAnimation",
            new Animation
            {
            { 0, 1, new Animation(v => tMSwitch.circle.Scale = v, 1.3, 1.0) },
            { 0, 1, new Animation(v => tMSwitch.circle.TranslationX = v, tMSwitch.border.Width - (tMSwitch.circle.Width +15), 0) }
            },
            length: 250, easing: Easing.CubicInOut);
        VisualStateManager.GoToState(tMSwitch, "Unselected");
        tMSwitch.border.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.SwitchUnselected);
        tMSwitch.border.Padding = 6;
    }
}
public class TMSwitchEventArgs : EventArgs
{
    public TMSwitchEventArgs(bool value) => Value = value;
    public bool Value { get; internal set; }
}
