using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Controls.Slider;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components.Controls
{
    public abstract class SliderCore : Grid
    {
        #region Private fields
        const double _enabledOpacity = 1;
        const double _disabledOpacity = .5;
        const int _iconHorizontalPadding = 4;
        protected const int _thumbZindex = 3;
        protected const int _stepLabelSpacing = 20;
        protected const int _iconSize = 20;
        Microsoft.Maui.Graphics.Size _allocatedSize;
        protected readonly Dictionary<View, double> _thumbPositionMap = new Dictionary<View, double>();
        protected double _labelMaxHeight;
        protected int _dragCount;
        protected double _thumbSize;
        #endregion

        #region Bindable Property
        public static BindableProperty MinimumValueProperty = BindableProperty.Create(nameof(MinimumValue), typeof(double), typeof(SliderCore), .0, propertyChanged: OnMinimumMaximumValuePropertyChanged);
        public static BindableProperty MaximumValueProperty = BindableProperty.Create(nameof(MaximumValue), typeof(double), typeof(SliderCore), 1.0, propertyChanged: OnMinimumMaximumValuePropertyChanged);
        public static BindableProperty StepValueProperty = BindableProperty.Create(nameof(StepValue), typeof(double), typeof(SliderCore), 0.01, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnMinimumMaximumValuePropertyChanged);
        public static BindableProperty TickValueProperty = BindableProperty.Create(nameof(TickValue), typeof(double), typeof(SliderCore), 10.0, defaultBindingMode: BindingMode.TwoWay, coerceValue: TickCoerceValue);
        public static BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(SliderSize), typeof(SliderCore), SliderSize.Medium, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnLayoutPropertyChanged);
        public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(SliderCore), null, propertyChanged: OnTitleTextPropertyChanged);
        public static BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(SliderCore), null);
        public static BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(SliderCore), null);
        public static BindableProperty LeftIconProperty = BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(SliderCore), null, propertyChanged: OnLeftIconSourceChanged);
        public static BindableProperty RightIconProperty = BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(SliderCore), null, propertyChanged: OnRightIconSourceChanged);
        public static BindableProperty ShowStepsProperty = BindableProperty.Create(nameof(ShowSteps), typeof(Boolean), typeof(SliderCore), false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnShowStepsPropertyChanged);
        public static BindableProperty ShowToolTipProperty = BindableProperty.Create(nameof(ShowToolTip), typeof(Boolean), typeof(SliderCore), false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnShowToolTipPropertyChanged);
        #endregion

        #region Property change methods
        static void OnTitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnTitleTextPropertyChanged((string)newValue);
        static void OnShowStepsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnShowStepsPropertyChanged();
        static void OnShowToolTipPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnShowToolTipPropertyChanged();
        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnLayoutPropertyChanged();
        static void OnMinimumMaximumValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnMinimumMaximumValuePropertyChanged();
        static void OnLeftIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnLeftIconSourceChanged();
        static void OnRightIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnRightIconSourceChanged();
        #endregion

        #region Public Property
        const int _outerElementTopPadding = 30;

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public string LeftText
        {
            get => (string)GetValue(LeftTextProperty);
            set => SetValue(LeftTextProperty, value);
        }
        public string RightText
        {
            get => (string)GetValue(RightTextProperty);
            set => SetValue(RightTextProperty, value);
        }
        public ImageSource LeftIconSource
        {
            get => (ImageSource)GetValue(LeftIconProperty);
            set => SetValue(LeftIconProperty, value);
        }
        public ImageSource RightIconSource
        {
            get => (ImageSource)GetValue(RightIconProperty);
            set => SetValue(RightIconProperty, value);
        }
        public Boolean ShowSteps
        {
            get => (Boolean)GetValue(ShowStepsProperty);
            set => SetValue(ShowStepsProperty, value);
        }
        public Boolean ShowToolTip
        {
            get => (Boolean)GetValue(ShowToolTipProperty);
            set => SetValue(ShowToolTipProperty, value);
        }
        public SliderSize Size
        {
            get => (SliderSize)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        public double MinimumValue
        {
            get => (double)GetValue(MinimumValueProperty);
            set => SetValue(MinimumValueProperty, value);
        }
        public double MaximumValue
        {
            get => (double)GetValue(MaximumValueProperty);
            set => SetValue(MaximumValueProperty, value);
        }
        public double StepValue
        {
            get => (double)GetValue(StepValueProperty);
            set => SetValue(StepValueProperty, value);
        }
        public double TickValue
        {
            get => (double)GetValue(TickValueProperty);
            set => SetValue(TickValueProperty, value);
        }
        #endregion

        #region UI Elements
        internal Border Track { get; } = SliderHelper.CreateBorderElement<Border>();
        internal Border TrackHighlight { get; } = SliderHelper.CreateBorderElement<Border>();
        internal StackLayout StepContainer { get; } =
            new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Spacing = 0
            };
        internal StackLayout LastStepContainer { get; } = SliderHelper.CreateStepLabelContainer();
        internal BoxView LastStepLine { get; } = SliderHelper.CreateStepLine();
        internal Label LastLabel { get; } = SliderHelper.CreateStepLabel();
        internal Label SliderTitle = new Label
        {
            FontSize = 12,
            TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray8)
        };
        internal Label LeftLabel = new Label
        {
            FontSize = 12,
            Margin = new Thickness(
                0,
                (DeviceInfo.Idiom == DeviceIdiom.Desktop) ? _outerElementTopPadding : 0,
                0,
                0
            ),
            TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray),
            HorizontalOptions = LayoutOptions.Start,
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalOptions = LayoutOptions.Center
        };
        internal Image LeftIcon = new Image
        {
            HeightRequest = 20,
            Margin = new Thickness(
                _iconHorizontalPadding,
                (DeviceInfo.Idiom == DeviceIdiom.Desktop) ? _outerElementTopPadding : 0,
                _iconHorizontalPadding,
                0
            ),
        };
        internal Image RightIcon = new Image
        {
            HeightRequest = 20,
            Margin = new Thickness(
                _iconHorizontalPadding,
                (DeviceInfo.Idiom == DeviceIdiom.Desktop) ? _outerElementTopPadding : 0,
                _iconHorizontalPadding,
                0
            ),
        };
        internal Label RightLabel = new Label
        {
            FontSize = 12,
            Margin = new Thickness(
                0,
                (DeviceInfo.Idiom == DeviceIdiom.Desktop) ? _outerElementTopPadding : 0,
                0,
                0
            ),
            TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray),
            HorizontalOptions = LayoutOptions.End,
            HorizontalTextAlignment = TextAlignment.End,
            VerticalTextAlignment = TextAlignment.Center
        };
        internal AbsoluteLayout SliderContainer = new AbsoluteLayout();
        internal StackLayout SliderHolderLayout = new StackLayout();
        private bool alternateValue = true;
        #endregion

        #region Constructor
        internal SliderCore()
        {
            RowDefinitions = new RowDefinitionCollection(
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition()
            );
            Children.Add(SliderHolderLayout);
            Grid.SetRow(SliderHolderLayout, 1);
        }
        #endregion

        #region Protected methods
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width == _allocatedSize.Width && height == _allocatedSize.Height)
                return;

            _allocatedSize = new Microsoft.Maui.Graphics.Size(width, height);
            OnLayoutPropertyChanged();
        }

        /// <summary>
        /// Change the opacity of the elements in the control based on IsEnabled status
        /// </summary>
        protected void OnIsEnabledChanged()
        {
            foreach (View child in Children)
            {
                child.Opacity = IsEnabled ? _enabledOpacity : _disabledOpacity;
            }
            foreach (View child in SliderContainer)
            {
                if (child.ZIndex != _thumbZindex)
                {
                    child.Opacity = IsEnabled ? _enabledOpacity : _disabledOpacity;
                }
                else if (child is Border)
                {
                    (child as Border).Stroke = IsEnabled
                        ? ResourcesDictionary.ColorsDictionary(ColorsConstants.BlueLightColor)
                        : ResourcesDictionary.ColorsDictionary(
                            ColorsConstants.SliderThumbBorderDisabledColor
                        );
                }
            }
        }

        /// <summary>
        /// Set the thumb style 
        /// </summary>
        /// <param name="border"></param>
        /// <param name="thumbStrokeThickness"></param>
        /// <param name="thumbSize"></param>
        internal void SetThumbStyle(Border border, double thumbStrokeThickness, double thumbSize)
        {
            border.StrokeThickness = thumbStrokeThickness;
            border.Stroke = IsEnabled
                ? ResourcesDictionary.ColorsDictionary(ColorsConstants.BlueLightColor)
                : ResourcesDictionary.ColorsDictionary(
                    ColorsConstants.SliderThumbBorderDisabledColor
                );
            border.Margin = new Thickness(0);
            border.BackgroundColor = Colors.White;
            border.StrokeShape = new Ellipse()
            {
                WidthRequest = thumbSize,
                HeightRequest = thumbSize
            };
            border.ZIndex = _thumbZindex;
        }

        /// <summary>
        /// Triggered when a thumb is moved
        /// </summary>
        protected void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            var view = (View)(
                sender ?? throw new NullReferenceException($"{nameof(sender)} cannot be null")
            );
            if (!IsEnabled)
            {
                return;
            }
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    OnPanStarted(view);
                    break;
                case GestureStatus.Running:
                    OnPanRunning(view, e.TotalX);
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    OnPanCompleted(view);
                    break;
            }
        }

        /// <summary>
        /// Build the step label container based on the parameters of the slider
        /// </summary>
        protected void BuildStepper()
        {
            StepContainer.Children.Clear();

            StepContainer.WidthRequest = SliderContainer.Width - _thumbSize;
            for (var i = MinimumValue; TickValue != 0 && i < MaximumValue; i += (MaximumValue - MinimumValue) / TickValue)
            {
                var stack = SliderHelper.CreateStepLabelContainer();
                var box = SliderHelper.CreateStepLine(Size);
                box.VerticalOptions = LayoutOptions.Start;
                stack.Children.Add(box);

                if (!alternateValue)
                {
                    var label = SliderHelper.CreateStepLabel(Size);
                    label.Text = Math.Round(i, 2).ToString();
                    stack.Children.Add(label);
                }
                alternateValue = !alternateValue;
                StepContainer.Children.Add(stack);
            }
        }

        /// <summary>
        /// Get the Pan shift value for thumb 
        /// </summary>
        protected double GetPanShiftValue(View view) =>
            Device.RuntimePlatform == Device.Android ? view.TranslationX : _thumbPositionMap[view];

        /// <summary>
        /// Add gesture recognizer for thumb
        /// </summary>
        protected void AddGestureRecognizer(View view, PanGestureRecognizer gestureRecognizer)
        {
            gestureRecognizer.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(gestureRecognizer);
        }

        /// <summary>
        /// Set label binding
        /// </summary>
        /// <param name="label"></param>
        /// <param name="bindableProperty"></param>
        protected void SetValueLabelBinding(Label label, BindableProperty bindableProperty) =>
            label.SetBinding(
                Label.TextProperty,
                new Binding { Source = this, Path = bindableProperty.PropertyName, }
            );

        #endregion

        #region Abstract Methods
        /// <summary>
        /// Set the layout of the absolute layout
        /// </summary>
        protected abstract void OnLayoutPropertyChanged();
        /// <summary>
        /// Update the slider when minimum or maximum value is changed
        /// </summary>
        protected abstract void OnMinimumMaximumValuePropertyChanged();
        /// <summary>
        /// On view size changed
        /// </summary>
        protected abstract void OnViewSizeChanged(object? sender, System.EventArgs e);
        /// <summary>
        /// Triggered when Panning started
        /// </summary>
        protected abstract void OnPanStarted(View view);
        /// <summary>
        /// Triggered when panning
        /// </summary>
        protected abstract void OnPanRunning(View view, double value);
        /// <summary>
        /// Triggered when panning is completed
        /// </summary>
        protected abstract void OnPanCompleted(View view);
        /// <summary>
        /// Update translation of value label border and tooltip when translate of value label is changed
        /// </summary>
        protected abstract void OnValueLabelTranslationChanged();
        /// <summary>
        /// Build Step labels and add or remove it from the container based on property
        /// </summary>
        protected abstract void OnShowStepsPropertyChanged();
        /// <summary>
        /// Show or hide tooltip for value 
        /// </summary>
        protected abstract void OnShowToolTipPropertyChanged();
        /// <summary>
        /// Show or hide left icon
        /// </summary>
        protected abstract void OnLeftIconSourceChanged();
        /// <summary>
        /// Show or hide right icon
        /// </summary>
        protected abstract void OnRightIconSourceChanged();
        /// <summary>
        /// Add title text
        /// </summary>
        /// <param name="newValue"></param>
        protected abstract void OnTitleTextPropertyChanged(string newValue);
        #endregion
        #region Private Methods
        private static object TickCoerceValue(BindableObject bindable, object value)
        {
            return CoerceValue((double)value);

        }
        private static object CoerceValue(double value)
        {
            if (value < 0)
            {
                return 0;
            }
            if (value < 1 && value > 0)
            {
                return 1;
            }
            if (value > 50)
            {
                return 50;
            }
            else
            {
                return value;
            }
        }
        #endregion
    }
}
