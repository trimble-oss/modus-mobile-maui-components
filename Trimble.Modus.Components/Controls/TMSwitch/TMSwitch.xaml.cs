using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public partial class TMSwitch : ContentView
{
    private int circleMargin = 6;
    private bool hasLoaded = false;
    protected EventHandler<TMSwitchEventArgs> _clicked;
    public static readonly BindableProperty SwitchSizeProperty =
        BindableProperty.Create(nameof(SwitchSize), typeof(SwitchSize), typeof(TMSwitch), SwitchSize.Medium, propertyChanged: OnSwitchSizeChanged);
    public static readonly BindableProperty IsToggledProperty =
       BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(TMSwitch), false, propertyChanged: OnSwitchToggleChanged);
    public new static readonly BindableProperty IsEnabledProperty =
      BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMSwitch), true, propertyChanged: OnIsEnabledChanged);

    public static readonly BindableProperty SwitchLabelPositionProperty =
     BindableProperty.Create(nameof(SwitchLabelPosition), typeof(TitlePosition), typeof(TMSwitch), TitlePosition.Right, propertyChanged: OnSwitchLabelPositionChanged);

    public static readonly BindableProperty ToggledCommandProperty =
      BindableProperty.Create(nameof(ToggledCommand), typeof(ICommand), typeof(TMSwitch), null);
    public static readonly BindableProperty TextProperty =
      BindableProperty.Create(nameof(Text), typeof(string), typeof(TMSwitch), null, propertyChanged: OnTextPropertyChanged
          );

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TMSwitch), Colors.Transparent, propertyChanged: OnTextColorChanged);

    public new static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(TMSwitch), Colors.Transparent, propertyChanged: OnBackgroundColorChanged);

    public static readonly BindableProperty ThumbColorProperty =
       BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(TMSwitch), Colors.Transparent, propertyChanged: OnThumbColorChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public TitlePosition SwitchLabelPosition
    {
        get => (TitlePosition)GetValue(SwitchLabelPositionProperty);
        set => SetValue(SwitchLabelPositionProperty, value);
    }
    internal Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    internal new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    internal Color ThumbColor
    {
        get => (Color)GetValue(ThumbColorProperty);
        set => SetValue(ThumbColorProperty, value);
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
        UpdateSwitchSize(this);
        this.SetDynamicResource(StyleProperty, "SwitchStyle");
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

        UpdateTextBasedOnPosition(this);

    }

    private void UpdateTextBasedOnPosition(TMSwitch tmSwitch)
    {
        if (string.IsNullOrEmpty(tmSwitch.Text))
        {
            tmSwitch.switchLeftText.IsVisible = false;
            tmSwitch.switchRightText.IsVisible = false;
            return;
        }
        if (tmSwitch.SwitchLabelPosition == TitlePosition.Left)
        {
            tmSwitch.container.ColumnDefinitions[0].Width = new GridLength(100, GridUnitType.Star);
            tmSwitch.container.ColumnDefinitions[2].Width = new GridLength(0);
            tmSwitch.switchLeftText.IsVisible = true;
            tmSwitch.switchRightText.IsVisible = false;
        }
        else
        {
            tmSwitch.container.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
            tmSwitch.container.ColumnDefinitions[0].Width = new GridLength(0);
            tmSwitch.switchLeftText.IsVisible = false;
            tmSwitch.switchRightText.IsVisible = true;
        }
    }
    private static void OnIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSwitch tMSwitch)
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

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSwitch tmSwitch)
        {
            tmSwitch.UpdateTextBasedOnPosition(tmSwitch);
        }
    }

    private static void OnSwitchSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSwitch tMSwitch)
        {
            UpdateSwitchSize(tMSwitch);
        }
    }

    private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSwitch tmSwitch)
        {
            tmSwitch.switchLeftText.TextColor = (Color)newValue;
            tmSwitch.switchRightText.TextColor = (Color)newValue;
        }
    }

    private static void OnSwitchLabelPositionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSwitch tmSwitch)
        {
            tmSwitch.UpdateTextBasedOnPosition(tmSwitch);
        }
    }

    private static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSwitch tmSwitch)
        {
            tmSwitch.border.Color = (Color)newValue;
        }
    }

    private static void OnThumbColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSwitch tmSwitch)
        {
            tmSwitch.circle.Color = (Color)newValue;
        }
    }

    private static void OnSwitchToggleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSwitch tMSwitch)
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
        if (tMSwitch.IsToggled)
        {
            OnSwitchSelected(tMSwitch);
        }
        else
        {
            OnSwitchUnSelected(tMSwitch);
        }
    }

    private void OnSwitchTapped(object sender, TappedEventArgs e)
    {
        if (!IsEnabled)
            return;
        IsToggled = !IsToggled;
    }

    private static void OnSwitchSelected(TMSwitch tMSwitch)
    {
        if (!tMSwitch.hasLoaded)
            return;

        tMSwitch._clicked?.Invoke(tMSwitch, new TMSwitchEventArgs(true));
        tMSwitch.ToggledCommand?.Execute(new TMSwitchEventArgs(true));

        tMSwitch.Animate("SelectedAnimation",
            new Animation
            {
                { 0, 1, new Animation(v => tMSwitch.circle.Scale = v, 1.0, 1.36) },
                { 0, 1, new Animation(v => tMSwitch.circle.TranslationX = v, tMSwitch.circleMargin,tMSwitch.border.WidthRequest - tMSwitch.circle.WidthRequest - tMSwitch.circleMargin) }
            },
            length: 250, easing: Easing.CubicIn);
        VisualStateManager.GoToState(tMSwitch, "On");
    }

    private static void OnSwitchUnSelected(TMSwitch tMSwitch)
    {
        if (!tMSwitch.hasLoaded)
            return;

        tMSwitch._clicked?.Invoke(tMSwitch, new TMSwitchEventArgs(false));
        tMSwitch.ToggledCommand?.Execute(new TMSwitchEventArgs(false));
        tMSwitch.Animate("UnselectedAnimation",
            new Animation
            {
                { 0, 1, new Animation(v => tMSwitch.circle.Scale = v, 1.3, 1.0) },
                { 0, 1, new Animation(v => tMSwitch.circle.TranslationX = v, tMSwitch.border.WidthRequest - tMSwitch.circle.WidthRequest - tMSwitch.circleMargin, tMSwitch.circleMargin) }
            },
            length: 250, easing: Easing.CubicInOut);
        VisualStateManager.GoToState(tMSwitch, "Off");
    }
}

public class TMSwitchEventArgs : EventArgs
{
    public TMSwitchEventArgs(bool value) => Value = value;
    public bool Value { get; internal set; }
}
