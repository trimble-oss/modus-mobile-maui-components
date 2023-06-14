using System.Windows.Input;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components.Controls.Button;

public partial class CustomButton : ContentView
{
    #region Private Properties

    private readonly TapGestureRecognizer _tapGestureRecognizer;
    private EventHandler _clicked;
    private Color tempColor;

    #endregion

    #region Bindable Properties

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomButton));

    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(CustomButton));

    public static readonly BindableProperty RightIconSourceProperty =
       BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(CustomButton));

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomButton), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomButton), null);

    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(Enums.Size), typeof(CustomButton), propertyChanged: OnSizeChanged);

    public static readonly BindableProperty ButtonStyleProperty =
       BindableProperty.Create(nameof(ButtonStyle), typeof(Enums.ButtonStyle), typeof(CustomButton), Enums.ButtonStyle.Fill, propertyChanged: OnButtonStyleChanged);

    public static readonly BindableProperty ButtonColorProperty =
   BindableProperty.Create(nameof(ButtonColor), typeof(ButtonColor), typeof(CustomButton), Enums.ButtonColor.Primary, propertyChanged: OnButtonColorChanged);

    public static readonly BindableProperty IsFloatingButtonProperty =
        BindableProperty.Create(nameof(IsFloatingButton), typeof(bool), typeof(CustomButton), false);

    public static readonly BindableProperty IsDisabledProperty =
        BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(CustomButton), false, propertyChanged: OnIsDisabledChanged);

    public static readonly BindableProperty ClickedEventProperty =
            BindableProperty.Create(nameof(Clicked), typeof(EventHandler), typeof(CustomButton));
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
    public CustomButton()
    {
        InitializeComponent();
        SetPadding(this);
        CheckButtonStyle(this);
    }

    #region Private Methods
    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomButton customButton)
        {
            SetPadding(customButton);
        }
    }
    private static void OnButtonStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomButton customButton && !customButton.IsFloatingButton)
        {
            CheckButtonStyle(customButton);
        }
    }

    private static void OnIsDisabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomButton customButton && !customButton.IsFloatingButton)
        {
            if ((bool)newValue)
            {
                customButton.Opacity = 0.5;
                customButton.GestureRecognizers.Clear();
            }
            else
            {
                CheckButtonStyle(customButton);
                customButton.Opacity = 1;
                customButton.GestureRecognizers.Add(customButton._tapGestureRecognizer);
            }
        }
    }

    private static void OnButtonColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomButton customButton && !customButton.IsFloatingButton)
        {
            CheckButtonStyle(customButton);
        }
    }

    private static void CheckButtonStyle(CustomButton customButton)
    {
        switch (customButton.ButtonStyle)
        {
            case Enums.ButtonStyle.BorderLess:
                customButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary("Transparent");
                customButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary("Transparent");
                customButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary("TrimbleBlue");
                customButton.tempColor = customButton.buttonFrame.BackgroundColor;
                break;
            case Enums.ButtonStyle.Fill:
                UpdateFillStyleColors(customButton);
                break;
            case Enums.ButtonStyle.Outline:
                UpdateOutlineStyleColors(customButton);
                break;
            default:
                break;
        }
    }

    private static void SetPadding(CustomButton customButton)
    {
        if (!customButton.IsFloatingButton)
        {
            switch (customButton.Size)
            {
                case Enums.Size.XSmall:
                    customButton.buttonLabel.FontSize = (double)Enums.FontSize.XSmall;
                    customButton.buttonStackLayout.Padding = new Thickness(16, 6);
                    customButton.HeightRequest = 32;
                    break;
                case Enums.Size.Small:
                    customButton.buttonLabel.FontSize = (double)Enums.FontSize.Small;
                    customButton.buttonStackLayout.Padding = new Thickness(16, 8);
                    customButton.HeightRequest = 40;
                    break;
                case Enums.Size.Default:
                    customButton.buttonLabel.FontSize = (double)Enums.FontSize.Default;
                    customButton.buttonStackLayout.Padding = new Thickness(16, 12);
                    customButton.HeightRequest = 48;
                    break;
                case Enums.Size.Large:
                    customButton.buttonLabel.FontSize = (double)Enums.FontSize.Large;
                    customButton.buttonStackLayout.Padding = new Thickness(16, 12);
                    customButton.HeightRequest = 56;
                    break;
                default:
                    break;

            }
        }
        else
        {
            if (customButton.Title != null)
            {
                customButton.buttonStackLayout.Padding = new Thickness(16, 0);
            }
            else
            {
                customButton.buttonStackLayout.Padding = new Thickness(16);
            }
        }

    }

    private static void UpdateFillStyleColors(CustomButton customButton)
    {
        switch (customButton.ButtonColor)
        {
            case ButtonColor.Secondary:
                customButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary("SecondaryButton");
                customButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary("TrimbleBlue");
                customButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary("White");
                customButton.tempColor = customButton.buttonFrame.BackgroundColor;

                break;

            case ButtonColor.Tertiary:
                customButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary("TertiaryButton");
                customButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary("Transparent");
                customButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary("TrimbleGray");
                customButton.tempColor = customButton.buttonFrame.BackgroundColor;
                break;

            case ButtonColor.Danger:
                customButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary("DangerRed");
                customButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary("Transparent");
                customButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary("White");
                customButton.tempColor = customButton.buttonFrame.BackgroundColor;
                break;
            default:
                customButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary("TrimbleBlue");
                customButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary("TrimbleBlue");
                customButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary("White");
                customButton.tempColor = customButton.buttonFrame.BackgroundColor;
                break;
        }
    }

    private static void UpdateOutlineStyleColors(CustomButton customButton)
    {
        customButton.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary("Transparent");
        customButton.tempColor = customButton.buttonFrame.BackgroundColor;
        switch (customButton.ButtonColor)
        {
            case ButtonColor.Primary:
                customButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary("TrimbleBlue");
                customButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary("TrimbleBlue");
                break;
            case ButtonColor.Secondary:
                customButton.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary("SecondaryButton");
                customButton.buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary("SecondaryButton");
                break;
            default:
                break;
        }
    }

    private Color getOnClickColor(Color color)
    {
        if (ButtonStyle == ButtonStyle.Outline)
        {
            if (ButtonColor == ButtonColor.Primary)
            {
                return (Color)BaseComponent.colorsDictionary()["BluePale"];
            }
            if (ButtonColor == ButtonColor.Secondary)
            {
                return (Color)BaseComponent.colorsDictionary()["NeutralGrey"];
            }


        }
        if (ButtonStyle == ButtonStyle.BorderLess)
        {
            return (Color)BaseComponent.colorsDictionary()["BluePale"];
        }

        if (color.Equals((Color)BaseComponent.colorsDictionary()["TrimbleBlue"]))
        {
            return (Color)BaseComponent.colorsDictionary()["TrimbleBlueClicked"];
        }
        else if (color.Equals((Color)BaseComponent.colorsDictionary()["SecondaryButton"]))
        {
            return (Color)BaseComponent.colorsDictionary()["SecondaryButtonClicked"];
        }
        else if (color.Equals((Color)BaseComponent.colorsDictionary()["TertiaryButton"]))
        {
            return (Color)BaseComponent.colorsDictionary()["TertiaryButtonClicked"];
        }
        else if (color.Equals((Color)BaseComponent.colorsDictionary()["DangerRed"]))
        {
            return (Color)BaseComponent.colorsDictionary()["DangerRedClicked"];
        }
        else if (color.Equals(Colors.Transparent))
        {
            return (Color)BaseComponent.colorsDictionary()["DangerRedClicked"];
        }

        return color;
    }
    #endregion
    #region Public Methods

    public void RaisePressed()
    {
        buttonFrame.BackgroundColor = getOnClickColor(buttonFrame.BackgroundColor);
    }
    public void RaiseReleased()
    {
        buttonFrame.BackgroundColor = tempColor;
        Command?.Execute(CommandParameter);
        _clicked?.Invoke(this, EventArgs.Empty);
    }
    public void RaiseCancel()
    {
        buttonFrame.BackgroundColor = tempColor;
    }

    #endregion
}
