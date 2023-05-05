
using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components
{

    public class TMButton : ContentView
    {   
        private const int _iconWidth = 24 , _iconHeight = 24;
        private const int _borderRadius = 4;
        private readonly Label _titleLabel;
        private readonly Image _iconImage;
        private readonly TapGestureRecognizer _tapGestureRecognizer;
        private bool imageSet = false;
        private bool isTextSet = false;
        public event EventHandler _clicked;
        private Border frame;
        private StackLayout stackLayout;
        private bool sizeSet = false;
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(TMButton), propertyChanged: OnTitleChanged);

        public static readonly BindableProperty IconSourceProperty =
            BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(TMButton), propertyChanged: OnIconSourceChanged);

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

        public new static readonly BindableProperty BackgroundColorProperty = 
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(TMButton), (Color)BaseComponent.colorsDictionary()["TrimbleBlue"],propertyChanged:onBackgroundColorChanged);

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(TMButton), (Color)BaseComponent.colorsDictionary()["TrimbleBlue"], propertyChanged: onBorderColorChanged);
       
        public static readonly BindableProperty TextColorProperty = 
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TMButton), defaultValue: Colors.White, propertyChanged: onTextColorChanged);
       
        public static readonly BindableProperty ButtonRadiusProperty =
            BindableProperty.Create(nameof(Radius), typeof(int), typeof(TMButton), _borderRadius, propertyChanged: onButtonRadiusChanged);

        public static readonly BindableProperty ClickedEventProperty =
            BindableProperty.Create(nameof(Clicked), typeof(EventHandler), typeof(TMButton));

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

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
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


        public int Radius
        {
            get { return (int)GetValue(ButtonRadiusProperty); }
            set { SetValue(ButtonRadiusProperty, value); }
        }


        public TMButton()
        {

            _titleLabel = new Label
            {
                TextColor = Colors.White,
            };

            _iconImage = new Image
            {
                WidthRequest = _iconWidth,
                HeightRequest = _iconHeight,

            };
            HorizontalOptions = LayoutOptions.Start;
            setDefault(this);

            stackLayout = new StackLayout();
            _iconImage.SetBinding(Image.SourceProperty, new Binding(nameof(IconSource), source: this));
            _titleLabel.VerticalOptions = LayoutOptions.Center;
            stackLayout.Orientation = StackOrientation.Horizontal;
            _titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(Title), source: this));
            stackLayout.Children.Add(_iconImage);
            stackLayout.Children.Add(_titleLabel);
            frame = new Border
            {
                Padding = 0,

                Content = stackLayout,
                BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"],
                HorizontalOptions = LayoutOptions.Start,
                StrokeShape = new Rectangle
                {
                    RadiusX = 4,
                    RadiusY = 4
                }
            };
            Content = frame;
            GestureRecognizers.Add(_tapGestureRecognizer = new TapGestureRecognizer());
            _tapGestureRecognizer.Tapped += OnTapped; 
 }
        

        private void setDefault(TMButton tmButton)
        {
            tmButton._titleLabel.FontSize = (double)Enums.FontSize.Default;
            if (tmButton.imageSet)
            {
                tmButton._titleLabel.Padding = new Thickness(0, 16, 24, 16);
                tmButton._iconImage.Margin = new Thickness(16, 16, 8, 16);
                tmButton._iconImage.IsVisible = true;
            }
            else
            {
                tmButton._titleLabel.Padding = new Thickness(24, 16, 24, 16);
                tmButton._iconImage.IsVisible = false;
            }
        }
        private void setFloatingButton(TMButton tmButton)
        {
            tmButton._titleLabel.FontSize = (double)Enums.FontSize.Default;
            if (tmButton.imageSet && !tmButton.isTextSet)
            {

                tmButton._iconImage.HorizontalOptions = LayoutOptions.Center;
                tmButton._iconImage.IsVisible = true;
                tmButton._titleLabel.IsVisible = false;
                tmButton._iconImage.Margin = new Thickness(16);

            }
            if (tmButton.isTextSet && !tmButton.imageSet)
            {
                tmButton._titleLabel.Padding = new Thickness(24, 16, 24, 16);
                tmButton._iconImage.IsVisible = false;
                tmButton._titleLabel.IsVisible = true;
            }
            if (tmButton.imageSet && tmButton.isTextSet)
            {
                tmButton._titleLabel.Padding = new Thickness(0, 16, 24, 16);
                tmButton._iconImage.Margin = new Thickness(16, 16, 8, 16);
                tmButton._iconImage.IsVisible = true;
            }

        }

        private void OnTapped(object sender, EventArgs e)
        {
            Command?.Execute(CommandParameter);
            _clicked?.Invoke(this, e);
            frame.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlueDark"];
        
            this.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                if (BackgroundColor != null)
                {
                    frame.BackgroundColor = BackgroundColor;

                }
                return false;
            });
        }
    
        private void UpdateButtonStyle()
        {

            var hasText = isTextSet;
            var hasIcon = imageSet;

            if (IsFloatingButton)
            {
                frame.Shadow = new Shadow
                {
                    Radius = 50,
                    Opacity = 100

                };
                frame.ZIndex = 1;
                setFloatingButton(this);
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
                        tmButton.setDefault(tmButton);
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
                        if (tmButton.imageSet)
                        {
                            tmButton._titleLabel.Padding = new Thickness(0, 8, 12, 8);
                            tmButton._iconImage.Margin = new Thickness(8, 8, 8, 8);
                            tmButton._iconImage.IsVisible = true;
                        }
                        else
                        {
                            tmButton._titleLabel.Padding = new Thickness(12, 8, 12, 8);
                            tmButton._iconImage.IsVisible = false;
                        }

                        break;
                    case Enums.Size.Small:
                        tmButton._titleLabel.FontSize = (double)Enums.FontSize.Small;
                        if (tmButton.imageSet)
                        {
                            tmButton._titleLabel.Padding = new Thickness(0, 8, 16, 8);
                            tmButton._iconImage.Margin = new Thickness(12, 8, 8, 8);
                            tmButton._iconImage.IsVisible = true;


                        }
                        else
                        {
                            tmButton._titleLabel.Padding = new Thickness(16, 8, 16, 8);
                            tmButton._iconImage.IsVisible = false;

                            break;
                        }


                        break;
                    case Enums.Size.Default:
                    case Enums.Size.Large:
                        tmButton._titleLabel.FontSize = (double)Enums.FontSize.Default;
                        if (tmButton.imageSet)
                        {
                            tmButton._titleLabel.Padding = new Thickness(0, 16, 24, 16);
                            tmButton._iconImage.Margin = new Thickness(16, 16, 8, 16);
                            tmButton._iconImage.IsVisible = true;
                        }
                        else
                        {
                            tmButton._titleLabel.Padding = new Thickness(24, 16, 24, 16);
                            tmButton._iconImage.IsVisible = false;

                        }
                        break;

                }
                tmButton.sizeSet = true;
            }
        }

        private static void OnIsDisabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var button = (TMButton)bindable;

            if ((bool)newValue)
            {
                button.frame.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleDisabledColor"];

                button._iconImage.Opacity = 0.5;
                button.GestureRecognizers.Clear();
            }
            else
            {

                button.frame.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
                button._titleLabel.TextColor = Colors.White;
                button._iconImage.Opacity = 1;
                button.GestureRecognizers.Add(button._tapGestureRecognizer);
            }
        }
        private static void onButtonRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMButton tmButton)
            {
                tmButton.frame.StrokeShape = new Rectangle { RadiusX = (int)newValue, RadiusY = (int)newValue };
            }
        }

        private static void OnIsFloatingButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var button = (TMButton)bindable;
            button.UpdateButtonStyle();
        }
        private static void onBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMButton tmButton)
            {
                tmButton.frame.Stroke = (Color)newValue;
            }
        }
        private static void onTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMButton tmButton)
            {
                tmButton._titleLabel.TextColor = (Color)newValue;
            }
        }
        private static void onBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMButton tmButton)
                tmButton.frame.BackgroundColor = (Color)newValue;
        }

    }
}