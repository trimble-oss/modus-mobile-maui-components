
using System.Windows.Input;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Constant;

namespace Trimble.Modus.Components;

public partial class TMButton : ContentView

{
    #region Private Properties

    private readonly TapGestureRecognizer _tapGestureRecognizer;

    private EventHandler _clicked;
    private Color activeColor;

    #endregion

    #region Bindable Properties

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TMButton));

    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(TMButton));

    public static readonly BindableProperty RightIconSourceProperty =
       BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(TMButton));

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TMButton), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(TMButton), null);

    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(Enums.Size), typeof(TMButton), propertyChanged: OnSizeChanged);

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
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
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
        SetPadding(this);
        CheckButtonStyle(this);
    }

    #region Private Methods
    
    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton)
        {
            SetPadding(tmButton);
        }
    }
    private static void OnButtonStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton && !tmButton.IsFloatingButton)
        {
            CheckButtonStyle(tmButton);
        }
    }

    private static void OnIsDisabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton && !tmButton.IsFloatingButton)
        {
            if ((bool)newValue)
            {
                tmButton.Opacity = 0.5;
                tmButton.GestureRecognizers.Clear();
            }
            else
            {
                CheckButtonStyle(tmButton);
                tmButton.Opacity = 1;
                tmButton.GestureRecognizers.Add(tmButton._tapGestureRecognizer);
            }
        }
    }

    private static void OnButtonColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton && !tmButton.IsFloatingButton)
        {
            CheckButtonStyle(tmButton);
        }
    }

    private static void CheckButtonStyle(TMButton tmButton)
    {
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
                case Enums.Size.Default:
                    tmButton.buttonLabel.FontSize = (double)Enums.FontSize.Default;
                    tmButton.buttonStackLayout.Padding = new Thickness(16, 12);
                    tmButton.HeightRequest = 48;
                    break;
                case Enums.Size.Large:
                    tmButton.buttonLabel.FontSize = (double)Enums.FontSize.Large;
                    tmButton.buttonStackLayout.Padding = new Thickness(16, 12);
                    tmButton.HeightRequest = 56;
                    break;
                default:
                    break;

            }
        }
        else
        {
            if (tmButton.Title != null)
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
        switch (tmButton.ButtonColor)
        {
            case ButtonColor.Secondary:
                tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.SecondaryButton);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);

                break;

            case ButtonColor.Tertiary:
                tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TertiaryButton);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
                break;

            case ButtonColor.Danger:
                tmButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.DangerRed);
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
            case ButtonColor.Primary:
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                break;
            case ButtonColor.Secondary:
                tmButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.SecondaryButton);
                tmButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.SecondaryButton);
                break;
            default:
                break;
        }
    }

    private Color GetOnClickColor(Color color)
    {
        switch (ButtonStyle)
        {
            case ButtonStyle.Outline:
                color = GetOnClickOutline(color);
                break;
            case ButtonStyle.BorderLess:
                color = ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
                break;
            case ButtonStyle.Fill:
                color = GetOnClickFill();
                break;
            default:
                break;
        }

        return color;
    }

    private Color GetOnClickOutline(Color color)
    {
        switch (ButtonColor)
        {
            case ButtonColor.Primary:
                return ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
            case ButtonColor.Secondary:
                return ResourcesDictionary.ColorsDictionary(ColorsConstants.NeutralGray);
            default:
                return color;
        }
    }

    private Color GetOnClickFill()
    {
        switch (ButtonColor)
        {
            case ButtonColor.Secondary:
                return ResourcesDictionary.ColorsDictionary(ColorsConstants.SecondaryButtonClicked);
            case ButtonColor.Tertiary:
                return ResourcesDictionary.ColorsDictionary(ColorsConstants.TertiaryButtonClicked);
            case ButtonColor.Danger:
                return ResourcesDictionary.ColorsDictionary(ColorsConstants.DangerRedClicked);
            default:
                return ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlueClicked);
        }
    }
    #endregion
    #region Public Methods

    public void RaisePressed()
    {
        if (buttonFrame.BackgroundColor != null)
        {
            activeColor = buttonFrame.BackgroundColor;
            buttonFrame.BackgroundColor = GetOnClickColor(buttonFrame.BackgroundColor);
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
