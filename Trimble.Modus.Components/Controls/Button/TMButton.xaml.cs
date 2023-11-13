using System.Windows.Input;
using CommunityToolkit.Maui.Behaviors;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public partial class TMButton : ContentView

{
    #region Private Properties

    protected EventHandler _clicked;
    protected Border _buttonFrame;
    protected Label _buttonLabel;

    #endregion

    #region Bindable Properties

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(TMButton), defaultValue:null, propertyChanged: OnTextPropertyChanged);

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
        BindableProperty.Create(nameof(IsFloatingButton), typeof(bool), typeof(TMButton), false, propertyChanged: IsFloatingButtonPropertyChanged);

    public static readonly BindableProperty IsDisabledProperty =
        BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(TMButton), false, propertyChanged: OnIsDisabledChanged);

    public static readonly BindableProperty IsLoadingProperty =
        BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(TMButton), false, propertyChanged: OnIsLoadingChanged);


    public static readonly BindableProperty ClickedEventProperty =
            BindableProperty.Create(nameof(Clicked), typeof(EventHandler), typeof(TMButton));

    public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor),
        typeof(Color),
        typeof(TMButton),
        Colors.White,
        BindingMode.Default,
        null,
        OnBackgroundColorPropertyChanged);

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
        typeof(Color),
        typeof(TMButton),
        Colors.Black,
        BindingMode.Default,
        null,
        OnTextColorPropertyChanged);

    public static readonly BindableProperty IconTintColorProperty = BindableProperty.Create(nameof(IconTintColor),
        typeof(Color),
        typeof(TMButton),
        Colors.Black,
        BindingMode.Default,
        null,
        OnIconTintColorPropertyChanged);

    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
        typeof(Color),
        typeof(TMButton),
        Colors.Transparent,
        BindingMode.Default,
        null,
        OnBorderColorPropertyChanged);


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

    public bool IsLoading
    {
        get { return (bool)GetValue(IsLoadingProperty); }
        set { SetValue(IsLoadingProperty, value); }
    }

    internal new Color BackgroundColor
    {
        get { return (Color)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }

    internal Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { this.SetValue(TextColorProperty, value); }
    }

    internal Color IconTintColor
    {
        get { return (Color)GetValue(IconTintColorProperty); }
        set { this.SetValue(IconTintColorProperty, value); }
    }

    internal Color BorderColor
    {
        get { return (Color)GetValue(BorderColorProperty); }
        set { this.SetValue(BorderColorProperty, value); }
    }

    #endregion
    public TMButton()
    {
        InitializeComponent();
        _buttonFrame = buttonFrame;
        _buttonLabel = buttonLabel;
        SetPadding(this);
        UpdateButtonStyle(this);
        OnTextChanged();
    }

    private void OnIconTintColorChanged()
    {
        leftIcon.Behaviors.Clear();
        rightIcon.Behaviors.Clear();
        if (IconTintColor != null)
        {
            var behavior = new IconTintColorBehavior
            {
                TintColor = IconTintColor
            };
            leftIcon.Behaviors.Add(behavior);
            rightIcon.Behaviors.Add(behavior);
        }
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
            SetDisabledState((bool)newValue, button);
        }
    }
    private static void OnIsLoadingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        bool isLoading = (bool)newValue;
        if (bindable is TMButton button)
        {
            if (isLoading)
            {
                // Make the first column visible by setting its width to Auto.
               button.buttonStackLayout.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);
            }
            else
            {
                // Hide the first column by setting its width to 0.
               button.buttonStackLayout.ColumnDefinitions[0].Width = new GridLength(0);
            }
            SetDisabledState(isLoading, button);
        }
    }

    private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton)
        {
            tmButton.OnBackgroundColorChanged();
        }
    }

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton)
        {
            tmButton.OnTextChanged();
        }
    }

    private static void OnTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton)
        {
            tmButton.OnTextColorChanged();
        }
    }

    private static void OnIconTintColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton)
        {
            tmButton.OnIconTintColorChanged();
        }
    }

    private static void OnBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton)
        {
            tmButton.OnBorderColorChanged();
        }
    }

    private static void SetDisabledState(bool disable, TMButton button)
    {
        if (disable)
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

    private static void OnButtonColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton button)
        {
            UpdateButtonStyle(button);
        }
    }

    private static void IsFloatingButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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
                    tmButton.SetDynamicResource(StyleProperty, "Borderless");
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
        tmButton.OnIconTintColorChanged();
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

        switch(tmButton.ButtonColor) {
            case ButtonColor.Secondary:
                tmButton.SetDynamicResource(StyleProperty, "SecondaryFill");
                break;
            case ButtonColor.Tertiary:
                tmButton.SetDynamicResource(StyleProperty, "TertiaryFill");
                break;
            case ButtonColor.Danger:
                tmButton.SetDynamicResource(StyleProperty, "DangerFill");
                break;
            default:
                tmButton.SetDynamicResource(StyleProperty, "PrimaryFill");
                break;

        }
    }

    private static void UpdateOutlineStyleColors(TMButton tmButton)
    {
        switch (tmButton.ButtonColor)
        {
            case ButtonColor.Secondary:
                tmButton.SetDynamicResource(StyleProperty, "SecondaryOutline");
                break;
            default:
                tmButton.SetDynamicResource(StyleProperty, "PrimaryOutline");
                break;
        }
    }

    private void OnBackgroundColorChanged()
    {
        buttonFrame.BackgroundColor = this.BackgroundColor;
    }

    private void OnTextChanged()
    {
        if (string.IsNullOrEmpty(Text))
        {
            buttonStackLayout.ColumnSpacing = 0;
        }
        else
        {
            buttonStackLayout.ColumnSpacing = 8;
        }
    }

    private void OnTextColorChanged()
    {
        if (this.buttonLabel != null)
        {
            this.buttonLabel.TextColor = this.TextColor;
        }
    }

    private void OnBorderColorChanged()
    {
        buttonFrame.Stroke = this.BorderColor;
    }

    #endregion
    #region Public Methods

    public void RaisePressed()
    {
        VisualStateManager.GoToState(this, "Pressed");
    }
    public void RaiseReleased()
    {
        VisualStateManager.GoToState(this, "Normal");
        Command?.Execute(CommandParameter);
        _clicked?.Invoke(this, EventArgs.Empty);
    }
    public void RaiseCancel()
    {
        VisualStateManager.GoToState(this, "Normal");
    }

    #endregion
}
