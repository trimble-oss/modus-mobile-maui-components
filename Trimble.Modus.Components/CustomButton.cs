using Microsoft.Maui.Controls.Shapes;

namespace Trimble.Modus.Components
{

    public class CustomButton : ContentView
    {
        private readonly Label _titleLabel;
        private readonly Image _iconImage;
        private readonly TapGestureRecognizer _tapGestureRecognizer;
        private bool imageSet = false;

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomButton), propertyChanged: OnTitleChanged);


        public static readonly BindableProperty IconSourceProperty =
            BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(CustomButton), propertyChanged: OnIconSourceChanged);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(Command), typeof(CustomButton), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomButton), null);

        public static readonly BindableProperty SizeProperty =
            BindableProperty.Create(nameof(Size), typeof(ButtonSize), typeof(CustomButton), propertyChanged: OnSizeChanged);


        public ButtonSize Size
        {
            get => (ButtonSize)GetValue(SizeProperty);
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

        public CustomButton()
        {
         
            _titleLabel = new Label
            {
                TextColor = Colors.White,
            };

            _iconImage = new Image
            {
                WidthRequest = 24,
                HeightRequest = 24,

            };
            var stackLayout = new StackLayout();
            _iconImage.SetBinding(Image.SourceProperty, new Binding(nameof(IconSource), source: this));
            _titleLabel.VerticalOptions = LayoutOptions.Center;
            stackLayout.Orientation = StackOrientation.Horizontal;
            _titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(Title), source: this));
            stackLayout.Children.Add(_iconImage);
            stackLayout.Children.Add(_titleLabel);
            Content = new Border
            {
                Padding = 0,
                Content = stackLayout,
                BackgroundColor = Color.FromHex("0063a3"),
                HorizontalOptions = LayoutOptions.Start,
                StrokeShape = new Rectangle
                {
                    RadiusX = 4,
                    RadiusY = 4
                }
            };

            GestureRecognizers.Add(_tapGestureRecognizer = new TapGestureRecognizer());
            _tapGestureRecognizer.Tapped += OnTapped;
          

        }

        private void OnTapped(object sender, EventArgs e)
        {
            Command?.Execute(CommandParameter);
        }

        private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomButton customButton)
            {
                customButton._titleLabel.Text = (string)newValue;
            }
        }

        private static void OnIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {

            if (bindable is CustomButton customButton)
            {

                customButton._iconImage.Source = (ImageSource)newValue;

                if (newValue != null)
                {
                    
                    customButton.imageSet = true;
                 
                }
                else
                {
                
                    customButton.imageSet = false;
                }
            }
        }

        private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomButton customButton)
            {
                var size = (ButtonSize)newValue;
                Console.WriteLine($"Size OnSize: {size}");
                switch (size)
                {
                    case ButtonSize.XSmall:
                        customButton._titleLabel.FontSize = 12;
                        if (customButton.imageSet)
                        {
                            customButton._titleLabel.Padding = new Thickness(0, 8, 12, 8);
                            customButton._iconImage.Margin = new Thickness(8, 8, 8, 8);
                        }
                        else
                        {
                            customButton._titleLabel.Padding = new Thickness(12, 8, 12, 8);
                            customButton._iconImage.IsVisible = false;
                        }
                     
                        break;
                    case ButtonSize.Small:
                        customButton._titleLabel.FontSize = 14;
                        if (customButton.imageSet)
                        {
                            customButton._titleLabel.Padding = new Thickness(0, 8, 16, 8);
                            customButton._iconImage.Margin = new Thickness(12, 8, 8, 8);
                        }
                        else
                        {
                            customButton._titleLabel.Padding = new Thickness(16, 8, 16, 8);
                            customButton._iconImage.IsVisible = false;
                            break;
                        }
                        
                        
                        break;
                
                        case ButtonSize.Default:
                        case ButtonSize.Large:
                        customButton._titleLabel.FontSize = 16;
                        if (customButton.imageSet)
                        {
                            customButton._titleLabel.Padding = new Thickness(0, 16, 24, 16);
                            customButton._iconImage.Margin = new Thickness(16, 16, 8, 16);
                        }
                        else
                        {
                            customButton._titleLabel.Padding = new Thickness(24, 16, 24, 16);
                            customButton._iconImage.IsVisible = false;
                        }


                  
                        break;
                }
            }
        }

    }
}