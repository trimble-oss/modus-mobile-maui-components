
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components
{

    public class TMInput : ContentView, IDisposable
    {
        #region Fields
        /// <summary>
        /// Store Focused State
        /// </summary>
        private bool _focused;

        /// <summary>
        /// Label for the title 
        /// </summary>
        private readonly Label _titleLabel;

        /// <summary>
        /// Icon for the helper text
        /// </summary>
        private readonly Image _helperIcon;

        /// <summary>
        /// Displays helper text
        /// </summary>
        private readonly Label _helperLabel;

        /// <summary>
        /// Displays success icon based on validation
        /// </summary>
        private readonly Image _successIcon;

        /// <summary>
        /// Displays error icon based on validation
        /// </summary>
        private readonly Image _errorIcon;

        /// <summary>
        /// Display the validation text
        /// </summary>
        private readonly Label _validationLabel;

        /// <summary>
        /// Wraps the grid of entry, title label and helper text labels
        /// </summary>
        private readonly StackLayout _labelContainer;

        /// <summary>
        /// Displays entry without default border
        /// </summary>
        private readonly BorderlessEntry _borderlessEntry;

        /// <summary>
        /// Displays border over the entry control
        /// </summary>
        private readonly Border _border;

        /// <summary>
        /// Contain the helper icon and helper text
        /// </summary>
        private readonly StackLayout _helperText;

        /// <summary>
        /// Contains the validation icon and validation text
        /// </summary>
        private readonly StackLayout _validationContainer;

        /// <summary>
        /// Displays the left icon in entry
        /// </summary>
        private readonly ImageButton _leftIcon;

        /// <summary>
        /// Displays the right icon in entry
        /// </summary>
        private readonly ImageButton _rightIcon;

        /// <summary>
        /// Contains the borderless entry, left and right icon
        /// </summary>
        private readonly Grid _entryGridContainer;

        /// <summary>
        /// Private field to hold the default height of the title label
        /// </summary>
        private double _originalTitleHeight;

        /// <summary>
        /// Size of the leading and trailing icon.
        /// </summary>
        internal const int ICONSIZE = 25;
        
        private const double disabledOpacity = 0.4;

        private bool _isTextValidated = false;

        /// <summary>
        /// Tracks whether Dispose has been called
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the TextProperty bindable property. This property is used to display the property text
        /// </summary>
        public static readonly BindableProperty TextProperty =
          BindableProperty.Create(
             propertyName: "Text",
              returnType: typeof(string),
              declaringType: typeof(TMInput),
              defaultBindingMode: BindingMode.TwoWay,
              propertyChanged: OnInputTextChanged);

        /// <summary>
        /// Identifies the TextColor bindable property. This property is used to define the color of the entry text
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create("TextColor", typeof(Color), typeof(TMInput), (Color)BaseComponent.colorsDictionary()["Black"], BindingMode.Default, null);

        /// <summary>
        /// Gets or sets value that indicates whether the input control is enabled or not.
        /// </summary>
        public static new readonly BindableProperty IsEnabledProperty =
            BindableProperty.Create("IsEnabled", typeof(bool), typeof(TMInput), true, BindingMode.Default, null, OnEnabledPropertyChanged);

        /// <summary>
        /// Gets or sets value that indicates whether the input control is readonly or not.
        /// </summary>
        public static readonly BindableProperty IsReadOnlyProperty =
            BindableProperty.Create("IsReadOnly", typeof(bool), typeof(TMInput), false, BindingMode.Default, null, OnReadOnlyPropertyChanged);

        /// <summary>
        /// Identifies the cursor position in the text entry
        /// </summary>
        public static readonly BindableProperty CursorPositionProperty =
            BindableProperty.Create("CursorPosition", typeof(int), typeof(TMInput), 0, BindingMode.Default, null, OnCursorPositionPropertyChanged);

        /// <summary>
        /// Identifies the IsPassword property, this property is used to hide the text entered into dots.
        /// </summary>
        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create("IsPassword", typeof(bool), typeof(TMInput), false, BindingMode.TwoWay, null);

        /// <summary>
        /// Gets or sets the type of keyboard is used when the entry is focused.
        /// </summary>
        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create("Keyboard", typeof(Keyboard), typeof(TMInput), Keyboard.Default, BindingMode.Default, null);

        /// <summary>
        /// Gets or sets the placeholder text of the text input
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create("Placeholder", typeof(string), typeof(TMInput), string.Empty, BindingMode.Default, null);

        /// <summary>
        /// Used to set the return command of the entry
        /// </summary>
        public static readonly BindableProperty ReturnCommandProperty =
            BindableProperty.Create("ReturnCommand", typeof(ICommand), typeof(TMInput), null, BindingMode.OneWay, null);

        /// <summary>
        /// Used to set the properties for the return command
        /// </summary>
        public static readonly BindableProperty ReturnCommandParameterProperty =
            BindableProperty.Create("ReturnCommandParameter", typeof(object), typeof(TMInput), null, BindingMode.OneWay, null);

        /// <summary>
        /// Gets or sets the FontAttribute property of the entry
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(TMInput), FontAttributes.None, BindingMode.TwoWay, null);

        /// <summary>
        /// Gets or sets the font family property of the entry
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create("FontFamily", typeof(string), typeof(TMInput), null, BindingMode.TwoWay, null);

        /// <summary>
        /// Gets or sets the font size property of the entry
        /// </summary>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create("FontSize", typeof(double), typeof(TMInput), (double)Enums.FontSize.Small, BindingMode.TwoWay, null);

        /// <summary>
        /// Gets or sets the type of the return button in the keyboard
        /// </summary>
        public static readonly BindableProperty ReturnTypeProperty =
            BindableProperty.Create("ReturnType", typeof(ReturnType), typeof(TMInput), ReturnType.Default, BindingMode.TwoWay, null);

        /// <summary>
        /// Gets or sets the color of the placeholder text
        /// </summary>
        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create("PlaceholderColor", typeof(Color), typeof(TMInput), Colors.DarkGray, BindingMode.TwoWay, null);

        /// <summary>
        /// Gets or sets the maximum allowed length of the text entry
        /// </summary>
        public static readonly BindableProperty MaxLengthProperty =
            BindableProperty.Create("MaxLength", typeof(int), typeof(TMInput), int.MaxValue, BindingMode.TwoWay, null);

        /// <summary>
        /// Gets or sets the text for the title label in the control
        /// </summary>
        public static readonly BindableProperty TitleTextProperty =
            BindableProperty.Create(nameof(TitleText), typeof(string), typeof(TMInput), null, BindingMode.TwoWay, null, propertyChanged: OnTitleTextChanged);

        /// <summary>
        /// Gets or sets the text for helper text label in the control
        /// </summary>
        public static readonly BindableProperty HelperTextProperty =
            BindableProperty.Create(nameof(HelperText), typeof(string), typeof(TMInput), null, propertyChanged: OnHelperTextChanged);

        /// <summary>
        /// Gets or sets the image source for the left icon in the entry
        /// </summary>
        public static readonly BindableProperty LeftIconSourceProperty =
            BindableProperty.Create("LeftIconSource", typeof(ImageSource), typeof(TMInput), null, propertyChanged: OnLeftIconSourceChanged);

        /// <summary>
        /// Gets or sets the image source for the right icon in the entry
        /// </summary>
        public static readonly BindableProperty RightIconSourceProperty =
            BindableProperty.Create("RightIconSource", typeof(ImageSource), typeof(TMInput), null, propertyChanged: OnRightIconSourceChanged);

        /// <summary>
        /// Gets or sets the command for left icon
        /// </summary>
        public static readonly BindableProperty LeftIconCommandProperty =
            BindableProperty.Create("LeftIconCommand", typeof(ICommand), typeof(TMInput), null, BindingMode.OneWay, null);

        /// <summary>
        /// Gets or sets the command property for left icon
        /// </summary>
        public static readonly BindableProperty LeftIconCommandParameterProperty =
            BindableProperty.Create("LeftIconCommandParameter", typeof(object), typeof(TMInput), null, BindingMode.OneWay, null);

        /// <summary>
        /// Gets or sets the command for right icon
        /// </summary>
        public static readonly BindableProperty RightIconCommandProperty =
            BindableProperty.Create("RightIconCommand", typeof(ICommand), typeof(TMInput), null, BindingMode.OneWay, null);

        /// <summary>
        /// Gets or sets the command property for right icon
        /// </summary>
        public static readonly BindableProperty RightIconCommandParameterProperty =
            BindableProperty.Create("RightIconCommandParameter", typeof(object), typeof(TMInput), null, BindingMode.OneWay, null);

        #endregion

        #region Public properties
        /// <summary>
        /// Delegate for the input validation function
        /// </summary>
        /// <param name="sender">Reference to the sender input control</param>
        /// <returns>Tuple of bool and string, bool represent the result of the text validation and string represent the text to be displayed</returns>
        public delegate Tuple<bool, string> InputValidationHandler(object sender);

        /// <summary>
        /// Public event handler to be invoked when the entry is focused
        /// </summary>
        public new event EventHandler Focused;

        /// <summary>
        /// Public event handler to be invoked when the entry goes away from focus
        /// </summary>
        public new event EventHandler Unfocused;

        /// <summary>
        /// Public event handler to be invoked when text is changed
        /// </summary>
        public event EventHandler<TextChangedEventArgs> TextChanged;

        /// <summary>
        /// Public event handler to hold the input validation function
        /// </summary>
        public event InputValidationHandler InputValidation;

        /// <summary>
        /// Gets or sets the helper text
        /// </summary>
        public string HelperText
        {
            get { return (string)GetValue(HelperTextProperty); }
            set { SetValue(HelperTextProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the entry text
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); OnPropertyChanged(nameof(Text)); }
        }

        /// <summary>
        /// Gets or sets the text color
        /// </summary>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); OnPropertyChanged(nameof(TextColor)); }
        }

        /// <summary>
        /// Gets or sets the text color
        /// </summary>
        public new bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the readonly property of the control
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the cursor position
        /// </summary>
        public int CursorPosition
        {
            get { return (int)GetValue(CursorPositionProperty); }
            set { SetValue(CursorPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the IsPassword property of the text input
        /// </summary>
        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        /// <summary>
        /// Gets or sets the keyboard type property for text input
        /// </summary>
        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Placeholder value 
        /// </summary>
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Return command
        /// </summary>
        public ICommand ReturnCommand
        {
            get => (ICommand)GetValue(ReturnCommandProperty);
            set => SetValue(ReturnCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the return command parameter
        /// </summary>
        public object ReturnCommandParameter
        {
            get => GetValue(ReturnCommandParameterProperty);
            set => SetValue(ReturnCommandParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets the Font attributes
        /// </summary>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Font family
        /// </summary>
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the return type in the keyboard
        /// </summary>
        public ReturnType ReturnType
        {
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the max length of the entry field
        /// </summary>
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Placeholder color
        /// </summary>
        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title text
        /// </summary>
        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the left icon in entry's source
        /// </summary>
        public ImageSource LeftIconSource
        {
            get { return (ImageSource)GetValue(LeftIconSourceProperty); }
            set { SetValue(LeftIconSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the right icon in entry's source
        /// </summary>
        public ImageSource RightIconSource
        {
            get { return (ImageSource)GetValue(RightIconSourceProperty); }
            set { SetValue(RightIconSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command for left icon
        /// </summary>
        public ICommand LeftIconCommand
        {
            get => (ICommand)GetValue(LeftIconCommandProperty);
            set => SetValue(LeftIconCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command parameter for left icon
        /// </summary>
        public object LeftIconCommandParameter
        {
            get => GetValue(LeftIconCommandParameterProperty);
            set => SetValue(LeftIconCommandParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets the right icon command
        /// </summary>
        public ICommand RightIconCommand
        {
            get => (ICommand)GetValue(RightIconCommandProperty);
            set => SetValue(RightIconCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the right icon command parameter
        /// </summary>
        public object RightIconCommandParameter
        {
            get => GetValue(LeftIconCommandParameterProperty);
            set => SetValue(LeftIconCommandParameterProperty, value);
        }
        #endregion

        #region Property changes

        /// <summary>
        /// This method is triggered when the text in the input field is changed, 
        /// this performs text validation to update the validation text field
        /// </summary>
        /// <param name="bindable">Object</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        private static void OnInputTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tmInput = (TMInput)bindable;
            Tuple<bool, string> result = null;

            if (!string.IsNullOrEmpty((string)newValue) && 
                (result = tmInput.InputValidation?.Invoke(tmInput))!= null && 
                !string.IsNullOrEmpty(result.Item2) )
            {
                tmInput._validationLabel.Text = result.Item2;
                tmInput._isTextValidated = true;
                tmInput.SetValidationTextStyle(result.Item1);
                if (!tmInput._validationContainer.IsVisible)
                {
                    tmInput._validationLabel.Text = result.Item2;
                    tmInput._validationContainer.Padding = new Thickness(0, 5, 0, 0);
                    tmInput.SetValidationTextStyle(result.Item1);
                    tmInput.OnHelperTextChanged(string.Empty);

                    tmInput._validationContainer.IsVisible = true;
                    tmInput._validationContainer.HeightRequest = 25;
                }
            }
            else
            {
                tmInput.HideValidationText();
                tmInput._isTextValidated = false;
                tmInput.ResetBorder();
            }
            
            tmInput.TextChanged?.Invoke(tmInput, new TextChangedEventArgs((string)oldValue, (string)newValue));
        }

        private void ResetBorder()
        {
            if (!_focused) { 

                _border.Stroke = (Color)BaseComponent.colorsDictionary()["Black"];
               }
            else {
                _border.Stroke = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
            }
        }

        /// <summary>
        /// Triggered when the IsEnabled property is changed
        /// </summary>
        /// <param name="bindable">Object</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        private static void OnEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TMInput).OnEnabledPropertyChanged((bool)newValue);
        }

        /// <summary>
        /// Triggered when the IsReadOnly property is changed
        /// </summary>
        /// <param name="bindable">Object</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        private static void OnReadOnlyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TMInput).OnReadOnlyPropertyChanged((bool)newValue);
            if (bindable is TMInput tmInput)
            {
                if ((bool)newValue)
                {
                    tmInput._helperText.Opacity = disabledOpacity;
                }
            }
        }

        /// <summary>
        /// Triggered when the cursor position is changed,
        /// This will set the cursor position to the last index
        /// when the cursor position is set to a greated index
        /// </summary>
        /// <param name="bindable">Object</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        private static void OnCursorPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TMInput).OnCursorPositionPropertyChanged((int)newValue);
        }

        /// <summary>
        /// Triggered when the helper text property is changed
        /// </summary>
        /// <param name="bindable">Object</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        private static void OnHelperTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TMInput).OnHelperTextChanged((string)newValue);
        }

        /// <summary>
        /// Trigged when the left icon source is changed to update the height and width
        /// </summary>
        /// <param name="bindable">Object</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New Value</param>
        private static void OnLeftIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tMinput = (TMInput)bindable;
            double dimensionValue = newValue!= null? ICONSIZE : 0;
            if(tMinput._leftIcon != null)
            {
                tMinput._leftIcon.Source = (ImageSource)newValue;
                tMinput._leftIcon.HeightRequest = dimensionValue;
                tMinput._leftIcon.WidthRequest = dimensionValue;
            }
        }

        /// <summary>
        /// Trigged when the right icon source is changed to update the height and width
        /// </summary>
        /// <param name="bindable">Object</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        private static void OnRightIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tMinput = (TMInput)bindable;
            double dimensionValue = newValue != null ? ICONSIZE : 0;
            if (tMinput._rightIcon != null)
            {
                tMinput._rightIcon.Source = (ImageSource)newValue;
                tMinput._rightIcon.HeightRequest = dimensionValue;
                tMinput._rightIcon.WidthRequest = dimensionValue;
            }
        }

        /// <summary>
        /// Trigged when the title text is changed to update the height
        /// </summary>
        /// <param name="bindable">Object</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
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

        #endregion
        public TMInput()
        {
            _titleLabel = new Label { Padding = new Thickness(0, 0, 0, 4) };
            _originalTitleHeight = _titleLabel.HeightRequest;
            _helperIcon = new Image { Source = ImageSource.FromResource("Trimble.Modus.Components.Images.helper_icon.png")  , VerticalOptions = LayoutOptions.Center };
            _successIcon = new Image { Source = ImageSource.FromResource("Trimble.Modus.Components.Images.input_valid_icon.png"), VerticalOptions = LayoutOptions.Center, WidthRequest = 0, HeightRequest = 20 };
            _errorIcon = new Image { Source = ImageSource.FromResource("Trimble.Modus.Components.Images.input_error_icon.png") ,VerticalOptions = LayoutOptions.Center, WidthRequest = 0, HeightRequest = 20 };
            _helperLabel = new Label() { Margin = new Thickness(5, 0, 0, 0) , FontSize = (double)Enums.FontSize.Small, VerticalOptions=LayoutOptions.Center};
            _validationLabel = new Label() { Margin = new Thickness(5, 0, 0, 0) , FontSize = (double)Enums.FontSize.Small, VerticalOptions=LayoutOptions.Center};
            _helperText = new StackLayout
            {   
                Orientation = StackOrientation.Horizontal,
                IsVisible = false,
                Padding = new Thickness(0,5,0,0),
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
                {   _successIcon,
                    _errorIcon,
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

        /// <summary>
        /// Set the validation text style
        /// </summary>
        /// <param name="success"></param>
        private void SetValidationTextStyle(bool success)
        {
            if (success )
            {
                _validationLabel.TextColor = Colors.Green;
                _successIcon.WidthRequest = 24;
                _errorIcon.WidthRequest = 0;
                _border.Stroke = Colors.Green;
            }
            else if (!success) 
            {
                _validationLabel.TextColor = Colors.Red;
                _successIcon.WidthRequest = 0;
                _errorIcon.WidthRequest = 24;
                _border.Stroke = Colors.Red;
            }
            
        }

        /// <summary>
        /// Sets the binding for different properties
        /// </summary>
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

        /// <summary>
        /// Listens to entry focus change and updates the border style
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e"></param>
        private void OnEntryFocusChanged(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry && entry.IsFocused)
            {
                _focused = true;
                _border.Stroke = _isTextValidated? _validationLabel.TextColor : (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
                _border.StrokeThickness = 2;
                Focused?.Invoke(this, e);
            }
            else
            {
                _focused = false;
                _border.StrokeThickness = 1;
                Unfocused?.Invoke(this, e);
                HideValidationText();
                _border.Stroke = (Color)BaseComponent.colorsDictionary()["Black"];
            }
        }

        /// <summary>
        /// Update the IsEnabled property of entry and icons based on IsEnabled property
        /// </summary>
        /// <param name="isEnabled">IsEnabled value</param>
        private void OnEnabledPropertyChanged(bool isEnabled)
        {
            base.IsEnabled = isEnabled;
            _borderlessEntry.IsEnabled = isEnabled;
            _leftIcon.IsEnabled = isEnabled;
            _rightIcon.IsEnabled = isEnabled;
            this.UpdateColor();
        }

        /// <summary>
        /// Update the IsEnabled and IsReadOnly property of entry and icons based on IsReadOnly property
        /// </summary>
        /// <param name="isReadOnly">IsReadOnly value</param>
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

        /// <summary>
        /// Checks if the cursor position is valid
        /// </summary>
        /// <param name="cursorPosition"></param>
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

        /// <summary>
        /// Updates HelperText container size based on the value
        /// </summary>
        /// <param name="newValue"></param>
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
                _validationContainer.IsVisible = false;
                _validationContainer.HeightRequest = 0;
            }
        }

        /// <summary>
        /// Update border opacity
        /// </summary>
        private void UpdateColor()
        {
            if (this.IsEnabled)
            {
                _border.Opacity = 1;
            }
            else
            {
                _border.Opacity = disabledOpacity;
            }
        }

        /// <summary>
        /// Show the validation text and hide helper text
        /// </summary>
        private void ShowValidationText()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                OnHelperTextChanged(HelperText);

                _validationContainer.IsVisible = true;
                _validationContainer.HeightRequest = 25;
            }
          
        }

        /// <summary>
        /// Hides the validation text and displays helper text if its available
        /// </summary>
        private void HideValidationText()
        {
            OnHelperTextChanged(HelperText);

            _validationContainer.IsVisible = false;
            _validationContainer.HeightRequest = 0;
        }

        /// <summary>
        /// Called by consumer object to clear the control
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Unsubscribe from entryfocus event when object is disposed
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                _borderlessEntry.Focused -= OnEntryFocusChanged;
                _borderlessEntry.Unfocused -= OnEntryFocusChanged;

                disposed = true;
            }
        }

        ~TMInput()
        {
            Dispose(disposing: false);
        }

    }
}