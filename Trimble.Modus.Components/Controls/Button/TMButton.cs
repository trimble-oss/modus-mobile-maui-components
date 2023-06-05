using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public class TMButton : ContentView
{

    #region Fields
    internal int _iconWidth = 16, _iconHeight = 16;
    private const int _borderRadius = 4;
    internal readonly Label _titleLabel;
    private readonly Image _iconImage;
    private readonly TapGestureRecognizer _tapGestureRecognizer;
    private bool imageSet = false;
    private bool isTextSet = false;
    public event EventHandler _clicked;
    internal Border frame;
    private StackLayout stackLayout;
    private bool sizeSet = false;
    #endregion

    #region Bindable Properties
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TMButton), propertyChanged: OnTitleChanged);

    public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(TMButton), propertyChanged: OnIconSourceChanged);

    public static readonly BindableProperty ColorProperty =
        BindableProperty.Create(nameof(ButtonColor), typeof(ButtonColor), typeof(TMButton), ButtonColor.Primary, propertyChanged: OnColorOrButtonStyleChanged);

    public static readonly BindableProperty ButtonStyleProperty =
      BindableProperty.Create(nameof(ButtonStyle), typeof(ButtonStyle), typeof(TMButton), ButtonStyle.Fill, propertyChanged: OnColorOrButtonStyleChanged);

    public static readonly BindableProperty ImagePositionProperty =
    BindableProperty.Create(nameof(IconPosition), typeof(ImagePosition), typeof(TMButton), ImagePosition.Left, propertyChanged: OnImagePoistionChanged);

    public static readonly BindableProperty IsProcessingStyleChangeProperty =
        BindableProperty.Create(nameof(IsProcessingStyleChange), typeof(bool), typeof(TMButton), defaultValue: false);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(Command), typeof(TMButton), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(TMButton), null);

    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(Enums.Size), typeof(TMButton), propertyChanged: OnSizeChanged);

    public static readonly BindableProperty IsFloatingButtonProperty =
        BindableProperty.Create(nameof(IsFloatingButton), typeof(bool), typeof(TMButton), false, propertyChanged: OnIsFloatingButtonChanged);

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
    public bool IsProcessingStyleChange
    {
        get { return (bool)GetValue(IsProcessingStyleChangeProperty); }
        set { SetValue(IsProcessingStyleChangeProperty, value); }
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
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ImageSource IconSource
    {
        get => (ImageSource)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }
    public ImagePosition IconPosition
    {
        get => (ImagePosition)GetValue(ImagePositionProperty);
        set => SetValue(ImagePositionProperty, value);
    }

    public Command Command
    {
        get => (Command)GetValue(CommandProperty);
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

    public ButtonColor ButtonColor
    {
        get => (ButtonColor)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
    public ButtonStyle ButtonStyle
    {
        get => (ButtonStyle)GetValue(ButtonStyleProperty);
        set => SetValue(ButtonStyleProperty, value);
    }
    #endregion

    public TMButton()
    {
        _titleLabel = new Label
        {
            TextColor = Colors.White,
            VerticalOptions = LayoutOptions.Center
        };
        _titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(Title), source: this));

        _iconImage = new Image
        {
            WidthRequest = _iconWidth,
            HeightRequest = _iconHeight,

        };
        _iconImage.SetBinding(Image.SourceProperty, new Binding(nameof(IconSource), source: this));

        stackLayout = new StackLayout()
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 16,
            Padding = new Thickness(16, 0, 16, 0)
        };

        frame = new Border
        {
            Padding = 0,
            BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"],
            HorizontalOptions = LayoutOptions.Start,
            StrokeShape = new Rectangle
            {
                RadiusX = _borderRadius,
                RadiusY = _borderRadius
            }
        };

        SetDefault(this);
        SetStackLayout();

        HorizontalOptions = LayoutOptions.Start;
        GestureRecognizers.Add(_tapGestureRecognizer = new TapGestureRecognizer() { NumberOfTapsRequired = 1 });
        _tapGestureRecognizer.Tapped += OnTapped;
    }

    #region Private Methods

    private void SetStackLayout()
    {
        switch (IconPosition)
        {
            case ImagePosition.Left:
                stackLayout.Children.Clear();
                stackLayout.Children.Add(_iconImage);
                stackLayout.Children.Add(_titleLabel);
                break;
            case ImagePosition.Right:
                stackLayout.Children.Clear();
                stackLayout.Children.Add(_titleLabel);
                stackLayout.Children.Add(_iconImage);
                break;
            default:
                break;
        }
        frame.Content = stackLayout;
        Content = frame;
    }

    private static void SetDefault(TMButton tmButton)
    {
        tmButton._titleLabel.FontSize = (double)Enums.FontSize.Default;
        tmButton._titleLabel.VerticalOptions = LayoutOptions.Center;

        if (tmButton.imageSet && tmButton.isTextSet)
        {
            tmButton._iconImage.IsVisible = true;
        }
        else
        {
            tmButton._iconImage.IsVisible = false;
        }
        if (tmButton.imageSet && !tmButton.isTextSet)
        {
            tmButton._iconImage.HorizontalOptions = LayoutOptions.Center;
            tmButton._iconImage.IsVisible = true;
            tmButton._titleLabel.IsVisible = false;
        }
        tmButton.HeightRequest = 48;
    }

    private static void SetFloatingButton(TMButton tmButton)
    {
        tmButton.frame.StrokeShape = new Rectangle { RadiusX = 50, RadiusY = 50 };
        tmButton.frame.Stroke = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
        tmButton.frame.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
        tmButton._titleLabel.TextColor = Colors.White;
        tmButton._titleLabel.FontSize = (double)Enums.FontSize.Default;
        tmButton._titleLabel.VerticalOptions = LayoutOptions.Center;

        if (tmButton.imageSet && tmButton.isTextSet)
        {
            tmButton._iconImage.IsVisible = true;
        }
        else
        {
            tmButton._iconImage.IsVisible = false;
        }
        if (tmButton.imageSet && !tmButton.isTextSet)
        {
            tmButton._iconImage.HorizontalOptions = LayoutOptions.Center;
            tmButton._iconImage.IsVisible = true;
            tmButton._titleLabel.IsVisible = false;
        }
        tmButton.HeightRequest = 48;
    }

    private void OnTapped(object sender, EventArgs e)
    {
        Command?.Execute(CommandParameter);
        _clicked?.Invoke(this, e);
        var col = frame.BackgroundColor;
        frame.BackgroundColor = GetOnColorClicked(frame.BackgroundColor);

        this.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            if (col != null)
            {
                frame.BackgroundColor = col;

            }
            return false;
        });
    }

    private Color GetOnColorClicked(Color color)
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

    private void UpdateButtonStyle()
    {
        if (IsFloatingButton)
        {
            frame.Shadow = new Shadow
            {
                Radius = 15,
                Opacity = 100

            };
            frame.ZIndex = 1;
            SetFloatingButton(this);
            Content = frame;
        }
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton)
        {
            tmButton._titleLabel.Text = (string)newValue;
            tmButton.isTextSet = true;
        }
    }

    private static void OnIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {

        if (bindable is TMButton tmButton)
        {

            tmButton._iconImage.Source = (ImageSource)newValue;

            if (newValue != null)
            {

                tmButton.imageSet = true;
                if (!tmButton.sizeSet)
                {
                    SetDefault(tmButton);
                }

            }
            else
            {
                tmButton.imageSet = false;
            }
        }
    }

    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMButton tmButton && !tmButton.IsFloatingButton)
        {
            var size = (Enums.Size)newValue;

            switch (size)
            {
                case Enums.Size.XSmall:
                    tmButton._titleLabel.FontSize = (double)Enums.FontSize.XSmall;
                    tmButton._titleLabel.VerticalOptions = LayoutOptions.Center;

                    if (tmButton.imageSet && tmButton.isTextSet)
                    {
                        tmButton._iconImage.IsVisible = true;
                    }
                    else
                    {
                        tmButton._iconImage.IsVisible = false;
                    }
                    if (tmButton.imageSet && !tmButton.isTextSet)
                    {
                        tmButton._iconImage.HorizontalOptions = LayoutOptions.Center;
                        tmButton._iconImage.IsVisible = true;
                        tmButton._titleLabel.IsVisible = false;
                    }
                    tmButton.HeightRequest = 32;
                    break;
                case Enums.Size.Small:
                    tmButton._titleLabel.FontSize = (double)Enums.FontSize.Small;
                    tmButton._titleLabel.VerticalOptions = LayoutOptions.Center;

                    if (tmButton.imageSet && tmButton.isTextSet)
                    {
                        tmButton._iconImage.IsVisible = true;
                    }
                    else
                    {
                        tmButton._iconImage.IsVisible = false;

                    }
                    if (tmButton.imageSet && !tmButton.isTextSet)
                    {
                        tmButton._iconImage.HorizontalOptions = LayoutOptions.Center;
                        tmButton._iconImage.IsVisible = true;
                        tmButton._titleLabel.IsVisible = false;
                    }
                    tmButton.HeightRequest = 40;

                    break;
                case Enums.Size.Default:
                    tmButton._titleLabel.FontSize = (double)Enums.FontSize.Default;
                    tmButton._titleLabel.VerticalOptions = LayoutOptions.Center;

                    if (tmButton.imageSet && tmButton.isTextSet)
                    {
                        tmButton._iconImage.IsVisible = true;
                    }
                    else
                    {
                        tmButton._iconImage.IsVisible = false;
                    }
                    if (tmButton.imageSet && !tmButton.isTextSet)
                    {
                        tmButton._iconImage.HorizontalOptions = LayoutOptions.Center;
                        tmButton._iconImage.IsVisible = true;
                        tmButton._titleLabel.IsVisible = false;
                    }
                    tmButton.HeightRequest = 48;
                    break;

                case Enums.Size.Large:
                    tmButton._titleLabel.FontSize = (double)Enums.FontSize.Large;
                    tmButton._titleLabel.VerticalOptions = LayoutOptions.Center;

                    if (tmButton.imageSet && tmButton.isTextSet)
                    {
                        tmButton._iconImage.IsVisible = true;
                    }
                    else
                    {
                        tmButton._iconImage.IsVisible = false;

                    }
                    if (tmButton.imageSet && !tmButton.isTextSet)
                    {
                        tmButton._iconImage.HorizontalOptions = LayoutOptions.Center;
                        tmButton._iconImage.IsVisible = true;
                        tmButton._titleLabel.IsVisible = false;
                    }

                    tmButton.HeightRequest = 56;
                    break;


            }
            tmButton.sizeSet = true;
        }
    }

    private static void OnColorOrButtonStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var button = (TMButton)bindable;
        if (button.IsProcessingStyleChange)
        {
            button.IsProcessingStyleChange = false;
            return;
        }

        UpdateButtonAppearance(button);
    }

    private static void UpdateButtonAppearance(TMButton button)
    {
        if (button.ButtonStyle == ButtonStyle.BorderLess)
        {
            button.frame.BackgroundColor = Colors.Transparent;
            button.frame.Stroke = Colors.Transparent;
            button._titleLabel.TextColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
        }
        else if (button.ButtonStyle == ButtonStyle.Outline)
        {
            button.frame.BackgroundColor = Colors.Transparent;
            if (button.ButtonColor == ButtonColor.Primary)
            {
                button._titleLabel.TextColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
                button.frame.Stroke = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
            }
            else if (button.ButtonColor == ButtonColor.Secondary)
            {
                button._titleLabel.TextColor = (Color)BaseComponent.colorsDictionary()["SecondaryButton"];
                button.frame.Stroke = (Color)BaseComponent.colorsDictionary()["SecondaryButton"];
            }
        }
        else
        {
            switch (button.ButtonColor)
            {
                case ButtonColor.Secondary:
                    button.frame.BackgroundColor = (Color)BaseComponent.colorsDictionary()["SecondaryButton"];
                    button.frame.Stroke = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
                    button._titleLabel.TextColor = Colors.White;
                    break;

                case ButtonColor.Tertiary:
                    button.frame.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TertiaryButton"];
                    button.frame.Stroke = Colors.Transparent;
                    button._titleLabel.TextColor = (Color)BaseComponent.colorsDictionary()["TrimbleGray"];
                    break;

                case ButtonColor.Danger:
                    button.frame.BackgroundColor = (Color)BaseComponent.colorsDictionary()["DangerRed"];
                    button.frame.Stroke = Colors.Transparent;
                    button._titleLabel.TextColor = Colors.White;
                    break;

                case ButtonColor.Primary:
                default:
                    button.frame.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
                    button.frame.Stroke = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
                    button._titleLabel.TextColor = Colors.White;
                    break;
            }
        }
    }

    private static void OnIsDisabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var button = (TMButton)bindable;

        if ((bool)newValue)
        {
            button.Opacity = 0.5;
            button.GestureRecognizers.Clear();
        }
        else
        {

            UpdateButtonAppearance(button);
            button.Opacity = 1;
            button.GestureRecognizers.Add(button._tapGestureRecognizer);
        }
    }
    private static void OnIsFloatingButtonChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var button = (TMButton)bindable;
        button.UpdateButtonStyle();
    }

    private static void OnImagePoistionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var button = (TMButton)bindable;
        button.SetStackLayout();
    }

    #endregion
}
