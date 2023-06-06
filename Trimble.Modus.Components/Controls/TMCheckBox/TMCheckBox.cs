using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components
{
    public class TMCheckBox : ContentView
    {
        #region Fields
        private Label _label;
        private Image _checkbox;
        private int _defaultWidth = 24, _defaultHeight = 24, _largeWidth = 32, _large_height = 32;
        private int _defaultFontSize = 14, _largeFontSize = 16;
        private EventHandler _checked;
        #endregion

        #region Public Properties

        public event EventHandler IsCheckedChanged
        {
            add { _checked += value; }
            remove { _checked -= value; }
        }

        public bool IsDisabled
        {
            get => (bool)GetValue(IsDisabledProperty);
            set => SetValue(IsDisabledProperty, value);
        }
        public CheckboxSize Size
        {
            get => (CheckboxSize)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }
        #endregion

        #region Bindable Properties

        public static readonly BindableProperty IsDisabledProperty =
         BindableProperty.Create(
             nameof(IsDisabled),
             typeof(bool),
             typeof(TMCheckBox),
             false,
             propertyChanged: OnIsDisabledChanged);

        public static readonly BindableProperty IsIndeterminateProperty =
          BindableProperty.Create(
              nameof(IsIndeterminate),
              typeof(bool),
              typeof(TMCheckBox),
              false,
              propertyChanged: OnIsIndeterminateChanged);

        public static readonly BindableProperty TextProperty =
           BindableProperty.Create(
               nameof(Text),
               typeof(string),
               typeof(TMCheckBox),
               string.Empty,
               propertyChanged: OnTextChanged);


        public static readonly BindableProperty IsCheckedProperty =
            BindableProperty.Create(
                nameof(IsChecked),
                typeof(bool),
                typeof(TMCheckBox),
                false,
                propertyChanged: OnIsCheckedChanged);
        public static readonly BindableProperty SizeProperty =
                   BindableProperty.Create(
                       nameof(Size),
                       typeof(CheckboxSize),
                       typeof(TMCheckBox),
                       CheckboxSize.Default,
                       propertyChanged: OnSizeChanged);
        #endregion

        #region Property changes

        private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customCheckboxView = (TMCheckBox)bindable;

            if (customCheckboxView.Size == CheckboxSize.Large)
            {
                customCheckboxView._label.FontSize = customCheckboxView._largeFontSize;
                customCheckboxView._checkbox.WidthRequest = customCheckboxView._largeWidth;
                customCheckboxView._checkbox.HeightRequest = customCheckboxView._large_height;
            }
            else
            {
                customCheckboxView._label.FontSize = customCheckboxView._defaultFontSize;
                customCheckboxView._checkbox.WidthRequest = customCheckboxView._defaultWidth;
                customCheckboxView._checkbox.HeightRequest = customCheckboxView._defaultHeight;
            }
        }


        private static void OnIsIndeterminateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customCheckboxView = (TMCheckBox)bindable;

            if (customCheckboxView.IsIndeterminate)
            {
                customCheckboxView.IsChecked = false;
                customCheckboxView._checkbox.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.indeterminate_checkbox.png");

            }
            else
            {
                customCheckboxView._checkbox.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.default_checkbox.png");
            }

        }
        private static void OnIsDisabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customCheckboxView = (TMCheckBox)bindable;
            customCheckboxView.UpdateDisabledState();
        }


        private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customCheckboxView = (TMCheckBox)bindable;
            if (customCheckboxView.IsChecked)
            {
                customCheckboxView._checkbox.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.checked_checkbox.png");

            }
            else
            {
                customCheckboxView._checkbox.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.default_checkbox.png");
            }

        }

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customCheckboxView = (TMCheckBox)bindable;
            customCheckboxView._label.Text = (string)newValue;
        }
        #endregion

        public TMCheckBox()
        {
            _label = new Label() { FontSize = _defaultFontSize, VerticalOptions = LayoutOptions.Center };
            _checkbox = new Image { Source = ImageSource.FromResource("Trimble.Modus.Components.Images.default_checkbox.png"), VerticalOptions = LayoutOptions.Center, HeightRequest = _defaultHeight, WidthRequest = _defaultWidth, Margin = new Thickness(0, 0, 4, 0) };
            Content = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    _checkbox,
                    _label,
                }
            };
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnTapGestureTapped;
            GestureRecognizers.Add(tapGesture);
        }
        /// <summary>
        /// For checked state while tapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTapGestureTapped(object sender, TappedEventArgs e)
        {
            if (!IsDisabled)
            {
                IsChecked = !IsChecked;
            }
            _checked?.Invoke(this, e);

        }
        /// <summary>
        /// Set The Disabled State 
        /// </summary>
        private void UpdateDisabledState()
        {
            _checkbox.IsEnabled = !IsDisabled;
            _label.IsEnabled = !IsDisabled;
            Opacity = IsDisabled ? 0.5 : 1;

        }
    }
}
