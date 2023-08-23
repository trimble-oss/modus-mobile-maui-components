using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMSwitch : ContentView
{
    private int circleMargin = 6;
    private bool hasLoaded = false;
    protected EventHandler<TMSwitchEventArgs> _clicked;
    private bool _switchSelected;
    private TMSwitchEventArgs switchEventTrue, switchEventFalse;
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
        switchEventTrue = new TMSwitchEventArgs(true);
        switchEventFalse = new TMSwitchEventArgs(false);
        UpdateSwitchSize(this);

        Loaded += (sender, args) =>
        {
            hasLoaded = true;
            if (IsToggled)
            {
                OnSwitchSelected(this);
            }
            else
            {
                OnSwitchUnSelected(this);
            }
        };

    }
    private static void OnIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMSwitch tMSwitch)
        {
            if ((bool)newValue)
            {
                tMSwitch.container.Opacity = 1;
            }
            else
            {
                tMSwitch.container.Opacity = 0.5;
            }

        }
    }
    private static void OnLabelTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMSwitch tMSwitch)
        {
            if (String.IsNullOrEmpty((string)newValue))
            {
                tMSwitch.switchText.IsVisible = false;
                tMSwitch.switchText.Text = string.Empty;
            }
            else
            {
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
                tMSwitch._switchSelected = true;
                OnSwitchSelected(tMSwitch);
            }
            else
            {
                tMSwitch._switchSelected = false;
                OnSwitchUnSelected(tMSwitch);
            }
        }
    }    

    private static void UpdateSwitchSize(TMSwitch tMSwitch)
    {
        if (tMSwitch.SwitchSize == SwitchSize.Medium)
        {
            tMSwitch.border.HeightRequest = 24;
            tMSwitch.border.WidthRequest = 48;
            tMSwitch.circle.HeightRequest = 12;
            tMSwitch.circle.WidthRequest = 12;
            tMSwitch.circleMargin = 4;
        }
        else
        {
            tMSwitch.border.HeightRequest = 32;
            tMSwitch.border.WidthRequest = 64;
            tMSwitch.circle.HeightRequest = 16;
            tMSwitch.circle.WidthRequest = 16;
            tMSwitch.circleMargin = 6;
        }
        tMSwitch.border.CornerRadius = tMSwitch.border.HeightRequest / 2;
        tMSwitch.circle.CornerRadius = tMSwitch.circle.HeightRequest / 2;
        tMSwitch.circle.TranslationX = tMSwitch.IsToggled ? -tMSwitch.border.Width : tMSwitch.border.Width;
    }

    private void OnSwitchTapped(object sender, TappedEventArgs e)
    {
        if (!IsEnabled)
            return;
        _switchSelected = !_switchSelected;
        if (!_switchSelected)
        {
            OnSwitchUnSelected(this);
        }
        else
        {
            OnSwitchSelected(this);
        }
    }
    private static void OnSwitchSelected(TMSwitch tMSwitch)
    {
        if (!tMSwitch.hasLoaded)
            return;
        tMSwitch._clicked?.Invoke(tMSwitch, tMSwitch.switchEventTrue);
        tMSwitch.ToggledCommand?.Execute(tMSwitch.switchEventTrue);

        tMSwitch.Animate("SelectedAnimation",
            new Animation
            {
                { 0, 1, new Animation(v => tMSwitch.circle.Scale = v, 1.0, 1.36) },
                { 0, 1, new Animation(v => tMSwitch.circle.TranslationX = v, tMSwitch.border.WidthRequest - tMSwitch.circle.WidthRequest - tMSwitch.circleMargin, tMSwitch.circleMargin) }
            },
            length: 250, easing: Easing.CubicIn);
        tMSwitch.border.Color = ResourcesDictionary.ColorsDictionary(ColorsConstants.SwitchSelected);
    }

    private static void OnSwitchUnSelected(TMSwitch tMSwitch)
    {
        if (!tMSwitch.hasLoaded)
            return;

        tMSwitch._clicked?.Invoke(tMSwitch, tMSwitch.switchEventFalse);
        tMSwitch.ToggledCommand?.Execute(tMSwitch.switchEventFalse);
        tMSwitch.Animate("UnselectedAnimation",
            new Animation
            {
                { 0, 1, new Animation(v => tMSwitch.circle.Scale = v, 1.3, 1.0) },
                { 0, 1, new Animation(v => tMSwitch.circle.TranslationX = v, tMSwitch.circleMargin, tMSwitch.border.WidthRequest - tMSwitch.circle.WidthRequest - tMSwitch.circleMargin) }
            },
            length: 250, easing: Easing.CubicInOut);
        tMSwitch.border.Color = ResourcesDictionary.ColorsDictionary(ColorsConstants.SwitchUnselected);
    }
}

public class TMSwitchEventArgs : EventArgs
{
    public TMSwitchEventArgs(bool value) => Value = value;
    public bool Value { get; internal set; }
}
