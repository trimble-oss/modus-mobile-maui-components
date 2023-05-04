
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components
{

    public class TMInput : ContentView
    {
        private readonly Label _titleLabel;
        private readonly Image _helperIcon;
        private readonly Label _helperLabel;
        private readonly Image _validationIcon;
        private readonly Label _validationLabel;
        private readonly StackLayout _labelContainer;
        private readonly BorderlessEntry _borderlessEntry;
        private readonly Border _border;
        private readonly StackLayout _helperText;
        private readonly StackLayout _validationContainer;
        private readonly ImageButton _leftIcon;
        private readonly ImageButton _rightIcon;
        private readonly Grid _entryGridContainer;

        private double _originalTitleHeight;

        public delegate Tuple<bool, string> InputValidationHandler(object sender);

        public new event EventHandler Focused;
        public new event EventHandler Unfocused;
        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event InputValidationHandler InputValidation;


        public static readonly BindableProperty TextProperty =
          BindableProperty.Create(
             propertyName: "Text",
              returnType: typeof(string),
              declaringType: typeof(TMInput),
              defaultBindingMode: BindingMode.TwoWay,
              propertyChanged: OnInputTextChanged);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create("TextColor", typeof(Color), typeof(TMInput), (Color)BaseComponent.colorsDictionary()["Black"], BindingMode.Default, null);

        public static new readonly BindableProperty IsEnabledProperty =
            BindableProperty.Create("IsEnabled", typeof(bool), typeof(TMInput), true, BindingMode.Default, null, OnEnabledPropertyChanged);

        public static readonly BindableProperty IsReadOnlyProperty =
            BindableProperty.Create("IsReadOnly", typeof(bool), typeof(TMInput), false, BindingMode.Default, null, OnReadOnlyPropertyChanged);

        public static readonly BindableProperty CursorPositionProperty =
            BindableProperty.Create("CursorPosition", typeof(int), typeof(TMInput), 0, BindingMode.Default, null, OnCursorPositionPropertyChanged);

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create("IsPassword", typeof(bool), typeof(TMInput), false, BindingMode.Default, null);

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create("Keyboard", typeof(Keyboard), typeof(TMInput), Keyboard.Default, BindingMode.Default, null);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create("Placeholder", typeof(string), typeof(TMInput), string.Empty, BindingMode.Default, null);

        public static readonly BindableProperty ReturnCommandProperty =
            BindableProperty.Create("ReturnCommand", typeof(ICommand), typeof(TMInput), null, BindingMode.OneWay, null);

        public static readonly BindableProperty ReturnCommandParameterProperty =
            BindableProperty.Create("ReturnCommandParameter", typeof(object), typeof(TMInput), null, BindingMode.OneWay, null);

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(TMInput), FontAttributes.None, BindingMode.TwoWay, null);

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create("FontFamily", typeof(string), typeof(TMInput), null, BindingMode.TwoWay, null);

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create("FontSize", typeof(double), typeof(TMInput), (double)Enums.FontSize.Small, BindingMode.TwoWay, null);

        public static readonly BindableProperty ReturnTypeProperty =
            BindableProperty.Create("ReturnType", typeof(ReturnType), typeof(TMInput), ReturnType.Default, BindingMode.TwoWay, null);

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create("PlaceholderColor", typeof(Color), typeof(TMInput), Colors.DarkGray, BindingMode.TwoWay, null);

        public static readonly BindableProperty MaxLengthProperty =
            BindableProperty.Create("MaxLength", typeof(int), typeof(TMInput), int.MaxValue, BindingMode.TwoWay, null);

        public static readonly BindableProperty TitleTextProperty =
            BindableProperty.Create(nameof(TitleText), typeof(string), typeof(TMInput), null, BindingMode.TwoWay, null, propertyChanged: OnTitleTextChanged);

        public static readonly BindableProperty HelperTextProperty =
            BindableProperty.Create(nameof(HelperText), typeof(string), typeof(TMInput), null, propertyChanged: OnHelperTextChanged);

        public static readonly BindableProperty LeftIconSourceProperty =
            BindableProperty.Create("LeftIconSource", typeof(ImageSource), typeof(TMInput), null, propertyChanged: OnLeftIconSourceChanged);

        public static readonly BindableProperty RightIconSourceProperty =
            BindableProperty.Create("RightIconSource", typeof(ImageSource), typeof(TMInput), null, propertyChanged: OnRightIconSourceChanged);

        public static readonly BindableProperty LeftIconCommandProperty =
            BindableProperty.Create("LeftIconCommand", typeof(ICommand), typeof(TMInput), null, BindingMode.OneWay, null);

        public static readonly BindableProperty LeftIconCommandParameterProperty =
            BindableProperty.Create("LeftIconCommandParameter", typeof(object), typeof(TMInput), null, BindingMode.OneWay, null);

        public static readonly BindableProperty RightIconCommandProperty =
            BindableProperty.Create("RightIconCommand", typeof(ICommand), typeof(TMInput), null, BindingMode.OneWay, null);

        public static readonly BindableProperty RightIconCommandParameterProperty =
            BindableProperty.Create("RightIconCommandParameter", typeof(object), typeof(TMInput), null, BindingMode.OneWay, null);

        public string HelperText
        {
            get { return (string)GetValue(HelperTextProperty); }
            set { SetValue(HelperTextProperty, value); }
        }
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); OnPropertyChanged(nameof(Text)); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); OnPropertyChanged(nameof(TextColor)); }
        }

        public new bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public int CursorPosition
        {
            get { return (int)GetValue(CursorPositionProperty); }
            set { SetValue(CursorPositionProperty, value); }
        }

        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public ICommand ReturnCommand
        {
            get => (ICommand)GetValue(ReturnCommandProperty);
            set => SetValue(ReturnCommandProperty, value);
        }

        public object ReturnCommandParameter
        {
            get => GetValue(ReturnCommandParameterProperty);
            set => SetValue(ReturnCommandParameterProperty, value);
        }

        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public ReturnType ReturnType
        {
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }

        public ImageSource LeftIconSource
        {
            get { return (ImageSource)GetValue(LeftIconSourceProperty); }
            set { SetValue(LeftIconSourceProperty, value); }
        }

        public ImageSource RightIconSource
        {
            get { return (ImageSource)GetValue(RightIconSourceProperty); }
            set { SetValue(RightIconSourceProperty, value); }
        }

        public ICommand LeftIconCommand
        {
            get => (ICommand)GetValue(LeftIconCommandProperty);
            set => SetValue(LeftIconCommandProperty, value);
        }

        public object LeftIconCommandParameter
        {
            get => GetValue(LeftIconCommandParameterProperty);
            set => SetValue(LeftIconCommandParameterProperty, value);
        }

        public ICommand RightIconCommand
        {
            get => (ICommand)GetValue(RightIconCommandProperty);
            set => SetValue(RightIconCommandProperty, value);
        }

        public object RightIconCommandParameter
        {
            get => GetValue(LeftIconCommandParameterProperty);
            set => SetValue(LeftIconCommandParameterProperty, value);
        }

        private static void OnInputTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tMinput = (TMInput)bindable;
            if(tMinput.InputValidation != null)
            {
                if (!string.IsNullOrEmpty((string)newValue))
                {
                    var result = tMinput.InputValidation?.Invoke(tMinput);

                    tMinput._validationLabel.Text = result.Item2;

                    tMinput.SetValidationTextStyle(result.Item1);
                    tMinput.OnHelperTextChanged(string.Empty);

                    tMinput._validationContainer.IsVisible = true;
                    tMinput._validationContainer.HeightRequest = 25;
                }
                else
                {
                    tMinput.OnHelperTextChanged(tMinput.HelperText);

                    tMinput._validationContainer.IsVisible = false;
                    tMinput._validationContainer.HeightRequest = 0;
                }
            }
            
            tMinput.TextChanged?.Invoke(tMinput, new TextChangedEventArgs((string)oldValue, (string)newValue));
        }

        private void SetValidationTextStyle(bool success)
        {
            _validationLabel.TextColor = success? Colors.Green : Colors.Red;
            _validationIcon.Source = success ? 
                                     ImageSource.FromResource("Trimble.Modus.Components.Images.input_valid_icon.png") :
                                     ImageSource.FromResource("Trimble.Modus.Components.Images.input_error_icon.png");
        }

        private static void OnEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TMInput).OnEnabledPropertyChanged((bool)newValue);
        }

        private static void OnReadOnlyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TMInput).OnReadOnlyPropertyChanged((bool)newValue);
        }

        private static void OnCursorPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TMInput).OnCursorPositionPropertyChanged((int)newValue);
        }

        private static void OnHelperTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TMInput).OnHelperTextChanged((string)newValue);
        }

        private static void OnLeftIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tMinput = (TMInput)bindable;
            double dimensionValue = newValue!= null? 25 : 0;
            if(tMinput._leftIcon != null)
            {
                tMinput._leftIcon.Source = (ImageSource)newValue;
                tMinput._leftIcon.HeightRequest = dimensionValue;
                tMinput._leftIcon.WidthRequest = dimensionValue;
            }
        }

        private static void OnRightIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tMinput = (TMInput)bindable;
            double dimensionValue = newValue != null ? 25 : 0;
            if (tMinput._rightIcon != null)
            {
                tMinput._rightIcon.Source = (ImageSource)newValue;
                tMinput._rightIcon.HeightRequest = dimensionValue;
                tMinput._rightIcon.WidthRequest = dimensionValue;
            }
        }

        private static void OnTitleTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tMinput = (TMInput)bindable;
            if (string.IsNullOrEmpty(tMinput.TitleText))
            {
                tMinput._titleLabel.IsVisible = false;
                tMinput._titleLabel.HeightRequest = 0;
            }
            else
            {
                tMinput._titleLabel.IsVisible = true;
                tMinput._titleLabel.HeightRequest = tMinput._originalTitleHeight;
            }
        }

        public TMInput()
        {
            _titleLabel = new Label { Padding = new Thickness(0, 0, 0, 4) };
            _originalTitleHeight = _titleLabel.HeightRequest;
            _helperIcon = new Image { Source = ImageSource.FromResource("Trimble.Modus.Components.Images.helper_icon.png")  , VerticalOptions = LayoutOptions.Center };
            _validationIcon = new Image { VerticalOptions = LayoutOptions.Center };
            _helperLabel = new Label() { Margin = new Thickness(5, 0, 0, 0) , FontSize = (double)Enums.FontSize.Small, VerticalOptions=LayoutOptions.Center};
            _validationLabel = new Label() { Margin = new Thickness(5, 0, 0, 0) , FontSize = (double)Enums.FontSize.Small, VerticalOptions=LayoutOptions.Center};
            _helperText = new StackLayout
            {   
                Orientation = StackOrientation.Horizontal,
                IsVisible = false,
                Children =
                {
                    _helperIcon,
                    _helperLabel,
                }
            };
            _validationContainer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                IsVisible = false,
                Children =
                {   _validationIcon,
                    _validationLabel
                }
            };
            _leftIcon = new ImageButton { HorizontalOptions = LayoutOptions.Start, HeightRequest=0, WidthRequest=0 };
            _rightIcon = new ImageButton { HorizontalOptions = LayoutOptions.End, HeightRequest = 0, WidthRequest = 0 };
            _borderlessEntry = new BorderlessEntry { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center};

            _labelContainer = new StackLayout { Orientation = StackOrientation.Vertical, Padding = 0 };
            _labelContainer.Children.Add(_titleLabel);

            _entryGridContainer = new Grid();
            _entryGridContainer.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto}
            };
            _entryGridContainer.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto },
            };

            _entryGridContainer.Children.Add(_leftIcon);
            _entryGridContainer.SetColumn(_leftIcon, 0);
            _entryGridContainer.Children.Add(_borderlessEntry);
            _entryGridContainer.SetColumn(_borderlessEntry, 1);
            _entryGridContainer.Children.Add(_rightIcon);
            _entryGridContainer.SetColumn(_rightIcon, 2);

            _border = new Border
            {
                Padding = new Thickness(16, 0),
                Content = _entryGridContainer,
                HorizontalOptions = LayoutOptions.Fill,
                StrokeShape = new Rectangle
                {
                    RadiusX = 4,
                    RadiusY = 4
                },
                Stroke = (Color)BaseComponent.colorsDictionary()["Black"],
            };
            SetDefault();

            _borderlessEntry.Focused += OnEntryFocusChanged;
            _borderlessEntry.Unfocused += OnEntryFocusChanged;
            _labelContainer.Children.Add(_border);
            _labelContainer.Children.Add(_helperText);
            _labelContainer.Children.Add(_validationContainer);

            SetBinding();
            Content = _labelContainer;
        }

        private void SetDefault()
        {
            _titleLabel.TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
        }

        private void SetBinding()
        {
            _borderlessEntry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.IsPasswordProperty, new Binding(nameof(IsPassword), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.KeyboardProperty, new Binding(nameof(Keyboard), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.ReturnCommandProperty, new Binding(nameof(ReturnCommand), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.ReturnCommandParameterProperty, new Binding(nameof(ReturnCommandParameter), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.FontAttributesProperty, new Binding(nameof(FontAttributes), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.FontFamilyProperty, new Binding(nameof(FontFamily), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.FontSizeProperty, new Binding(nameof(FontSize), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.ReturnTypeProperty, new Binding(nameof(ReturnType), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.PlaceholderColorProperty, new Binding(nameof(PlaceholderColor), BindingMode.TwoWay, source: this));
            _borderlessEntry.SetBinding(Entry.MaxLengthProperty, new Binding(nameof(MaxLength), BindingMode.TwoWay, source: this));

            _titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(TitleText), BindingMode.TwoWay, source: this));

            _leftIcon.SetBinding(ImageButton.CommandProperty, new Binding(nameof(LeftIconCommand), BindingMode.TwoWay, source: this));
            _leftIcon.SetBinding(ImageButton.CommandParameterProperty, new Binding(nameof(LeftIconCommandParameter), BindingMode.TwoWay, source: this));
            _rightIcon.SetBinding(ImageButton.CommandProperty, new Binding(nameof(RightIconCommand), BindingMode.TwoWay, source: this));
            _rightIcon.SetBinding(ImageButton.CommandParameterProperty, new Binding(nameof(RightIconCommandParameter), BindingMode.TwoWay, source: this));

            _helperLabel.SetBinding(Label.TextProperty, new Binding(nameof(HelperText), source: this));
        }

        private void OnEntryFocusChanged(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry && entry.IsFocused)
            {
                _border.Stroke = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
                _border.StrokeThickness = 2;
                Focused?.Invoke(this, e);
            }
            else
            {
                _border.Stroke = (Color)BaseComponent.colorsDictionary()["Black"];
                _border.StrokeThickness = 1;
                Unfocused?.Invoke(this, e);
            }
        }

        private void OnEnabledPropertyChanged(bool isEnabled)
        {
            base.IsEnabled = isEnabled;
            _borderlessEntry.IsEnabled = isEnabled;
            _leftIcon.IsEnabled = isEnabled;
            _rightIcon.IsEnabled = isEnabled;
            this.UpdateColor();
        }

        private void OnReadOnlyPropertyChanged(bool isReadOnly)
        {
            IsReadOnly = isReadOnly;
            base.IsEnabled = !isReadOnly;
            _borderlessEntry.IsReadOnly = isReadOnly;
            _leftIcon.IsEnabled = !isReadOnly;
            _rightIcon.IsEnabled = !isReadOnly;
            if (isReadOnly)
            {
                _border.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleReadOnlyGray"];
                _border.StrokeThickness = 0;
            }
            else
            {
                _border.BackgroundColor = Colors.White;
                _border.StrokeThickness = 1;
            }
        }

        private void OnCursorPositionPropertyChanged(int cursorPosition)
        {
            if (_borderlessEntry.Text.Length > cursorPosition)
            {
                _borderlessEntry.CursorPosition = cursorPosition;
            }
            else
            {
                _borderlessEntry.CursorPosition = _borderlessEntry.Text.Length - 1;
            }
        }

        private void OnHelperTextChanged(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                _helperText.IsVisible = true;
                _helperText.HeightRequest = 25;
            }
            else
            {
                _helperText.IsVisible = false;
                _helperText.HeightRequest = 0;
            }
        }

        private void UpdateColor()
        {
            if (this.IsEnabled)
            {
                _border.Opacity = 1;
            }
            else
            {
                _border.Opacity = 0.4;
            }
        }

    }
}