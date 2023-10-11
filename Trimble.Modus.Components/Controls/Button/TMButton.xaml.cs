
using System.Windows.Input;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Constant;
using CommunityToolkit.Maui.Behaviors;

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
        BindableProperty.Create(nameof(IsFloatingButton), typeof(bool), typeof(TMButton), false);

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

    private static void UpdateButtonStyle(TMButton tmButton)
    {
        if (tmButton.IsFloatingButton && (tmButton.ButtonStyle == ButtonStyle.Outline || tmButton.ButtonStyle == ButtonStyle.BorderLess))
        {
            tmButton.ButtonStyle = ButtonStyle.Fill;
        }
        switch (tmButton.ButtonStyle)
        {
            case Enums.ButtonStyle.BorderLess:
                tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
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
                    tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                    tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                    tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                }
                else
                {
                    tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                    tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                    tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                }

                break;

            case ButtonColor.Tertiary:
                tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray1);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
                break;

            case ButtonColor.Danger:
                tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Red);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
            default:
                tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
        }
    }

    private static void UpdateOutlineStyleColors(TMButton tmButton)
    {
        tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
        switch (tmButton.ButtonColor)
        {
            case ButtonColor.Secondary:
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                break;
            case ButtonColor.Tertiary:
            case ButtonColor.Danger:
                tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray1);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
                break;
            default:
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                break;
        }
    }

    private Color GetOnClickColor()
    {
        return ButtonStyle switch
        {
            ButtonStyle.Outline => GetOnClickOutline(),
            ButtonStyle.BorderLess => ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale),
            _ => GetOnClickFill(),
        };
    }

    private Color GetOnClickOutline()
    {
        return ButtonColor switch
        {
            ButtonColor.Secondary => ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray0),
            _ => ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale),
        };
    }

    private Color GetOnClickFill()
    {
        return ButtonColor switch
        {
            ButtonColor.Secondary => ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray10),
            ButtonColor.Tertiary => ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray2),
            ButtonColor.Danger => ResourcesDictionary.ColorsDictionary(ColorsConstants.RedDark),
            _ => ResourcesDictionary.ColorsDictionary(ColorsConstants.BlueDark),
        };
    }
    #endregion
    #region Public Methods

    public void RaisePressed()
    {
        if (buttonFrame.BackgroundColor != null)
        {
            activeColor = buttonFrame.BackgroundColor;
            buttonFrame.BackgroundColor = GetOnClickColor();
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
