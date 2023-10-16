using CommunityToolkit.Maui.Behaviors;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMButton : ContentView

{
    #region Private Properties

    protected EventHandler _clicked;
    protected Color activeColor;
    protected Border _buttonFrame;
    protected Label _buttonLabel;

    #endregion

    #region Bindable Properties

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(TMButton));

    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(TMButton));

    public static readonly BindableProperty RightIconSourceProperty =
       BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(TMButton));

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TMButton), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(TMButton), null);

    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(Enums.Size), typeof(TMButton), defaultValue: Enums.Size.Default, propertyChanged: OnSizeChanged);

    public static readonly BindableProperty ButtonStyleProperty =
       BindableProperty.Create(nameof(ButtonStyle), typeof(Enums.ButtonStyle), typeof(TMButton), Enums.ButtonStyle.Fill, propertyChanged: OnButtonStyleChanged);

    public static readonly BindableProperty ButtonColorProperty =
       BindableProperty.Create(nameof(ButtonColor), typeof(ButtonColor), typeof(TMButton), Enums.ButtonColor.Primary, propertyChanged: OnButtonColorChanged);

    public static readonly BindableProperty IsFloatingButtonProperty =
        BindableProperty.Create(nameof(IsFloatingButton), typeof(bool), typeof(TMButton), false, propertyChanged: FloatingProperyChanged);

    public static readonly BindableProperty IsDisabledProperty =
        BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(TMButton), false, propertyChanged: OnIsDisabledChanged);

    public static readonly BindableProperty ClickedEventProperty =
            BindableProperty.Create(nameof(Clicked), typeof(EventHandler), typeof(TMButton));
    #endregion

    #region Public Properties


    public event EventHandler Clicked
    {
        add { _clicked += value; }
        remove { _clicked -= value; }
    }

    public bool IsFloatingButton
    {
        get { return (bool)GetValue(IsFloatingButtonProperty); }
        set { SetValue(IsFloatingButtonProperty, value); }
    }
    public Enums.Size Size
    {
        get => (Enums.Size)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
    public Enums.ButtonStyle ButtonStyle
    {
        get => (Enums.ButtonStyle)GetValue(ButtonStyleProperty);
        set => SetValue(ButtonStyleProperty, value);
    }
    public ButtonColor ButtonColor
    {
        get => (ButtonColor)GetValue(ButtonColorProperty);
        set => SetValue(ButtonColorProperty, value);
    }
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public ImageSource LeftIconSource
    {
        get => (ImageSource)GetValue(LeftIconSourceProperty);
        set => SetValue(LeftIconSourceProperty, value);
    }

    public ImageSource RightIconSource
    {
        get => (ImageSource)GetValue(RightIconSourceProperty);
        set => SetValue(RightIconSourceProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public bool IsDisabled
    {
        get { return (bool)GetValue(IsDisabledProperty); }
        set { SetValue(IsDisabledProperty, value); }
    }

    public new Color BackgroundColor
    {
        get { return (Color)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }

    #endregion
    public TMButton()
    {
        InitializeComponent();
        _buttonFrame = buttonFrame;
        _buttonLabel = buttonLabel;
        SetPadding(this);
        UpdateButtonStyle(this);
        UpdateButtonIconColor();
    }

    private void UpdateButtonIconColor()
    {
        leftIcon.Behaviors.Clear();
        rightIcon.Behaviors.Clear();
        var behavior = new IconTintColorBehavior
        {
            TintColor = buttonLabel.TextColor
        };
        leftIcon.Behaviors.Add(behavior);
        rightIcon.Behaviors.Add(behavior);
    }

    #region Private Methods

    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton button)
        {
            SetPadding(button);
        }
    }
    private static void OnButtonStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton button)
        {
            UpdateButtonStyle(button);
        }
    }

    private static void OnIsDisabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton button)
        {
            if ((bool)newValue)
            {
                button.Opacity = 0.5;
                button.GestureRecognizers.Clear();
            }
            else
            {
                UpdateButtonStyle(button);
                button.Opacity = 1;
            }
        }
    }

    private static void OnButtonColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton button)
        {
            UpdateButtonStyle(button);
        }
    }

    private static void FloatingProperyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton button)
        {
            UpdateButtonStyle(button);
        }
    }

    private static void UpdateButtonStyle(TMButton tmButton)
    {
        if (tmButton.IsFloatingButton)
        {
            UpdateFillStyleColors(tmButton);
        }
        else
        {
            switch (tmButton.ButtonStyle)
            {
                case Enums.ButtonStyle.BorderLess:
                    tmButton.buttonFrame.SetDynamicResource(StyleProperty, ThemeColorConstants.DefaultBorderless);
                    tmButton.buttonLabel.SetDynamicResource(StyleProperty, ThemeColorConstants.BlueText);
                    break;
                case Enums.ButtonStyle.Fill:
                    UpdateFillStyleColors(tmButton);
                    break;
                case Enums.ButtonStyle.Outline:
                    UpdateOutlineStyleColors(tmButton);
                    break;
                default:
                    break;
            }
        }
        tmButton.UpdateButtonIconColor();
    }

    private static void SetPadding(TMButton tmButton)
    {
        if (!tmButton.IsFloatingButton)
        {
            switch (tmButton.Size)
            {
                case Enums.Size.XSmall:
                    tmButton.buttonLabel.FontSize = (double)Enums.FontSize.XSmall;
                    tmButton.buttonStackLayout.Padding = new Thickness(16, 6);
                    tmButton.HeightRequest = 32;
                    break;
                case Enums.Size.Small:
                    tmButton.buttonLabel.FontSize = (double)Enums.FontSize.Small;
                    tmButton.buttonStackLayout.Padding = new Thickness(16, 8);
                    tmButton.HeightRequest = 40;
                    break;
                case Enums.Size.Large:
                    tmButton.buttonLabel.FontSize = (double)Enums.FontSize.Large;
                    tmButton.buttonStackLayout.Padding = new Thickness(16, 12);
                    tmButton.HeightRequest = 56;
                    break;
                default:
                    tmButton.buttonLabel.FontSize = (double)Enums.FontSize.Default;
                    tmButton.buttonStackLayout.Padding = new Thickness(16, 12);
                    tmButton.HeightRequest = 48;
                    break;

            }
        }
        else
        {
            if (tmButton.Text != null)
            {
                tmButton.buttonStackLayout.Padding = new Thickness(16, 0);
            }
            else
            {
                tmButton.buttonStackLayout.Padding = new Thickness(16);
            }
        }

    }

    private static void UpdateFillStyleColors(TMButton tmButton)
    {

        if (tmButton.IsFloatingButton && (tmButton.ButtonColor == ButtonColor.Tertiary || tmButton.ButtonColor == ButtonColor.Danger))
        {
            tmButton.ButtonColor = ButtonColor.Primary;
        }
        switch (tmButton.ButtonColor)
        {
            case ButtonColor.Secondary:
                if (tmButton.IsFloatingButton)
                {
                    tmButton.buttonFrame.SetDynamicResource(StyleProperty, ThemeColorConstants.SecondaryBorder);
                    tmButton.buttonLabel.SetAppThemeColor(StyleProperty,
                        ResourcesDictionary.LightThemeColor(ThemeColorConstants.Black),
                        ResourcesDictionary.DarkThemeColor(ThemeColorConstants.White));
                }
                else
                {
                    tmButton.buttonFrame.SetDynamicResource(StyleProperty, ThemeColorConstants.SecondaryBorder);
                    tmButton.buttonLabel.SetDynamicResource(StyleProperty, ThemeColorConstants.WhiteText);
                }

                break;

            case ButtonColor.Tertiary:
                tmButton.buttonFrame.SetDynamicResource(StyleProperty, ThemeColorConstants.TertiaryBorder);
                tmButton.buttonLabel.SetDynamicResource(StyleProperty, ThemeColorConstants.GrayText);
                break;

            case ButtonColor.Danger:
                tmButton.buttonFrame.SetDynamicResource(StyleProperty, ThemeColorConstants.DangerBorder);
                tmButton.buttonLabel.SetDynamicResource(StyleProperty, ThemeColorConstants.WhiteText);
                break;
            default:
                tmButton.buttonFrame.SetDynamicResource(StyleProperty, ThemeColorConstants.PrimaryBorder);
                tmButton.buttonLabel.SetDynamicResource(StyleProperty, ThemeColorConstants.WhiteText);
                break;
        }
    }

    private static void UpdateOutlineStyleColors(TMButton tmButton)
    {
        switch (tmButton.ButtonColor)
        {
            case ButtonColor.Secondary:
                tmButton.buttonLabel.SetDynamicResource(StyleProperty, ThemeColorConstants.GrayText);
                tmButton.buttonFrame.SetDynamicResource(StyleProperty, ThemeColorConstants.SecondaryOutlineBorder);
                break;
            case ButtonColor.Tertiary:
            case ButtonColor.Danger:
                tmButton.buttonFrame.SetDynamicResource(StyleProperty, ThemeColorConstants.DefaultOutlineBorder);
                tmButton.buttonLabel.SetDynamicResource(StyleProperty, ThemeColorConstants.GrayText);
                break;
            default:
                tmButton.buttonFrame.SetDynamicResource(StyleProperty, ThemeColorConstants.PrimaryOutlineBorder);
                tmButton.buttonLabel.SetDynamicResource(StyleProperty, ThemeColorConstants.BlueText);
                break;
        }
    }

    private string GetOnClickColor()
    {
        return ButtonStyle switch
        {
            ButtonStyle.Outline => GetOnClickOutline(),
            ButtonStyle.BorderLess => ThemeColorConstants.LightBlue,
            _ => GetOnClickFill(),
        };
    }

    private string GetOnClickOutline()
    {
        return ButtonColor switch
        {
            ButtonColor.Secondary => ThemeColorConstants.LightGray,
            _ => ThemeColorConstants.LightBlue,
        };
    }

    private string GetOnClickFill()
    {
        return ButtonColor switch
        {
            ButtonColor.Secondary => ThemeColorConstants.Black,
            ButtonColor.Tertiary => ThemeColorConstants.Gray,
            ButtonColor.Danger => ThemeColorConstants.DarkRed,
            _ => ThemeColorConstants.DarkBlue,
        };
    }
    #endregion
    #region Public Methods

    public void RaisePressed()
    {
        if (buttonFrame.BackgroundColor != null)
        {
            activeColor = buttonFrame.BackgroundColor;
            buttonFrame.SetDynamicResource(BackgroundColorProperty, GetOnClickColor());
        }
    }
    public void RaiseReleased()
    {
        if (activeColor != null)
        {
            buttonFrame.BackgroundColor = activeColor;
        }
        Command?.Execute(CommandParameter);
        _clicked?.Invoke(this, EventArgs.Empty);
    }
    public void RaiseCancel()
    {
        if (activeColor != null)
        {
            buttonFrame.BackgroundColor = activeColor;
        }
    }

    #endregion
}
