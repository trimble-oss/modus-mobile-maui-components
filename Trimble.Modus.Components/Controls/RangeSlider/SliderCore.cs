using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Controls.Slider;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components.Controls
{
    public abstract class SliderControl : Grid
    {
        #region Private fields
        const double enabledOpacity = 1;
        const double disabledOpacity = .5;
        Microsoft.Maui.Graphics.Size allocatedSize;
        protected readonly Dictionary<View, double> thumbPositionMap = new Dictionary<View, double>();
        protected double labelMaxHeight;
        protected int dragCount;
        protected double thumbSize;
        #endregion

        #region Bindable Property
        public static BindableProperty MinimumValueProperty = BindableProperty.Create(nameof(MinimumValue), typeof(double), typeof(SliderControl), .0, propertyChanged: OnMinimumMaximumValuePropertyChanged);
        public static BindableProperty MaximumValueProperty = BindableProperty.Create(nameof(MaximumValue), typeof(double), typeof(SliderControl), 1.0, propertyChanged: OnMinimumMaximumValuePropertyChanged);
        public static BindableProperty StepValueProperty = BindableProperty.Create(nameof(StepValue), typeof(double), typeof(SliderControl), 0.0, propertyChanged: OnMinimumMaximumValuePropertyChanged);
        public static BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(SliderSize), typeof(SliderControl), SliderSize.Medium, propertyChanged: OnLayoutPropertyChanged);
        public static BindableProperty ValueLabelStyleProperty = BindableProperty.Create(nameof(ValueLabelStyle), typeof(Style), typeof(SliderControl), propertyChanged: OnLayoutPropertyChanged);
        public static BindableProperty ValueLabelStringFormatProperty = BindableProperty.Create(nameof(ValueLabelStringFormat), typeof(string), typeof(SliderControl), "{0:0.##}", propertyChanged: OnLayoutPropertyChanged);
        public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(SliderControl), null, propertyChanged: OnTitleTextPropertyChanged);
        public static BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(SliderControl), null);
        public static BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(SliderControl), null);
        public static BindableProperty LeftIconProperty = BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(SliderControl), null, propertyChanged: OnLeftIconSourceChanged);
        public static BindableProperty RightIconProperty = BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(SliderControl), null, propertyChanged: OnRightIconSourceChanged);
        public static BindableProperty ValueLabelSpacingProperty = BindableProperty.Create(nameof(ValueLabelSpacing), typeof(double), typeof(SliderControl), 5.0, propertyChanged: OnLayoutPropertyChanged);
        public static BindableProperty ShowStepsProperty = BindableProperty.Create(nameof(ShowSteps), typeof(Boolean), typeof(SliderControl), false, propertyChanged: OnShowStepsPropertyChanged);
        public static BindableProperty ShowToolTipProperty= BindableProperty.Create(nameof(ShowToolTip), typeof(Boolean), typeof(SliderControl), false, propertyChanged: OnShowToolTipPropertyChanged);
        #endregion

        #region Property change methods
        static void OnTitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnTitleTextPropertyChanged((string) newValue);
        static void OnShowStepsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnShowStepsPropertyChanged();
        static void OnShowToolTipPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnShowToolTipPropertyChanged();
        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnLayoutPropertyChanged();
        static void OnMinimumMaximumValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnMinimumMaximumValuePropertyChanged();
        static void OnLeftIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnLeftIconSourceChanged();
        static void OnRightIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnRightIconSourceChanged();
        #endregion

        #region Public Property
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
        public Style ValueLabelStyle
        {
            get => (Style)GetValue(ValueLabelStyleProperty);
            set => SetValue(ValueLabelStyleProperty, value);
        }
        public string ValueLabelStringFormat
        {
            get => (string)GetValue(ValueLabelStringFormatProperty);
            set => SetValue(ValueLabelStringFormatProperty, value);
        }
        public double ValueLabelSpacing
        {
            get => (double)GetValue(ValueLabelSpacingProperty);
            set => SetValue(ValueLabelSpacingProperty, value);
        }
        #endregion

        #region UI Elements
        internal Border Track { get; } = SliderHelper.CreateBorderElement<Border>();
        internal Border TrackHighlight { get; } = SliderHelper.CreateBorderElement<Border>();
        internal StackLayout StepContainer { get; } = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Horizontal, Spacing = 0 };
        internal StackLayout LastStepContainer { get; } = SliderHelper.CreateStepLabelContainer();
        internal BoxView LastStepLine { get; } = SliderHelper.CreateStepLine();
        internal Label LastLabel { get; } = SliderHelper.CreateStepLabel();
        internal Label SliderTitle = new Label { FontSize = 12, TextColor = Color.FromArgb("#464B52")};
        internal Label LeftLabel= new Label { FontSize = 12, Margin = new Thickness(0, 25, 0, 0), TextColor = Color.FromArgb("#252A2E"), HorizontalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.Center};
        internal Image LeftIcon = new Image { HeightRequest = 20, Margin = new Thickness(5,25,5,0),};
        internal Image RightIcon = new Image { HeightRequest = 20, Margin = new Thickness(5, 25, 5, 0), };
        internal Label RightLabel= new Label { FontSize = 12, Margin = new Thickness(0, 25, 0, 0), TextColor = Color.FromArgb("#252A2E"), HorizontalOptions = LayoutOptions.End, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center};
        internal AbsoluteLayout AbsoluteLayout = new AbsoluteLayout();
        internal StackLayout sliderHolderLayout = new StackLayout();

        #endregion

        public SliderControl()
        {
            RowDefinitions = new RowDefinitionCollection(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }, new RowDefinition());
            Children.Add(sliderHolderLayout);
            Grid.SetRow(sliderHolderLayout, 1);
        }

        #region Protected methods
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width == allocatedSize.Width && height == allocatedSize.Height)
                return;

            allocatedSize = new Microsoft.Maui.Graphics.Size(width, height);
            OnLayoutPropertyChanged();
        }
        protected void OnIsEnabledChanged()
        {
            foreach (View child in Children)
            {
                if (child.ZIndex != 3)
                {
                    child.Opacity = IsEnabled
                                    ? enabledOpacity
                                    : disabledOpacity;
                }
                else if (child is Border)
                {
                    (child as Border).Stroke = IsEnabled ? Color.FromArgb("#217CBB") : Color.FromArgb("#C3C4C9");
                }
            }
        }
        internal void SetThumbStyle(Border border, double thumbStrokeThickness, double thumbSize, double thumbRadius)
        {
            border.StrokeThickness = thumbStrokeThickness;
            border.Stroke = IsEnabled ? Color.FromArgb("#217CBB") : Color.FromArgb("#C3C4C9");
            border.Margin = new Thickness(0);
            border.BackgroundColor = Colors.White;
            border.StrokeShape = new Ellipse() { WidthRequest = thumbSize, HeightRequest = thumbSize };
            border.ZIndex = 3;
        }
        protected void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            var view = (View)(sender ?? throw new NullReferenceException($"{nameof(sender)} cannot be null"));
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
        protected double GetPanShiftValue(View view)
            => Device.RuntimePlatform == Device.Android
                ? view.TranslationX
                : thumbPositionMap[view];
        protected void AddGestureRecognizer(View view, PanGestureRecognizer gestureRecognizer)
        {
            gestureRecognizer.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(gestureRecognizer);
        }
        protected void SetValueLabelBinding(Label label, BindableProperty bindableProperty)
            => label.SetBinding(Label.TextProperty, new Binding
            {
                Source = this,
                Path = bindableProperty.PropertyName,
                StringFormat = ValueLabelStringFormat
            });
        protected void SetTitleLabelBinding(Label label, BindableProperty bindableProperty)
            => label.SetBinding(Label.TextProperty, new Binding
            {
                Source = this,
                Path = bindableProperty.PropertyName,
            });
        #endregion

        #region Abstract Methods
        protected abstract void OnLayoutPropertyChanged();
        protected abstract void OnMinimumMaximumValuePropertyChanged();
        protected abstract void OnViewSizeChanged(object? sender, System.EventArgs e);
        protected abstract void OnPanStarted(View view);
        protected abstract void OnPanRunning(View view, double value);
        protected abstract void OnPanCompleted(View view);
        protected abstract void OnValueLabelTranslationChanged();
        protected abstract void OnShowStepsPropertyChanged();
        protected abstract void OnShowToolTipPropertyChanged();
        protected abstract void OnLeftIconSourceChanged();
        protected abstract void OnRightIconSourceChanged();
        protected abstract void OnTitleTextPropertyChanged(string newValue);
        #endregion

        protected void BuildStepper(bool isRangeSlider = false)
        {
            StepContainer.Children.Clear();

            StepContainer.WidthRequest = AbsoluteLayout.Width - (isRangeSlider? thumbSize : thumbSize);
            //StepContainer.WidthRequest = Width-2*thumbSize; For RangeSlider
            for (var i = MinimumValue; StepValue != 0 && i < MaximumValue; i += StepValue)
            {
                var stack = SliderHelper.CreateStepLabelContainer();
                var box = SliderHelper.CreateStepLine(Size);
                stack.Children.Add(box);

                var label = SliderHelper.CreateStepLabel(Size);
                label.Text = i.ToString();
                stack.Children.Add(label);
                StepContainer.Children.Add(stack);
            }
        }
    }
}
