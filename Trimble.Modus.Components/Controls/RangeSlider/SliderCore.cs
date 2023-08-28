using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Controls.Slider;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components.Controls
{
    public abstract class SliderControl : AbsoluteLayout
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
        public static BindableProperty ValueLabelSpacingProperty = BindableProperty.Create(nameof(ValueLabelSpacing), typeof(double), typeof(SliderControl), 5.0, propertyChanged: OnLayoutPropertyChanged);
        public static BindableProperty ShowStepsProperty = BindableProperty.Create(nameof(ShowSteps), typeof(Boolean), typeof(SliderControl), false, propertyChanged: OnShowStepsPropertyChanged);
        #endregion

        #region Property change methods
        static void OnShowStepsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnShowStepsPropertyChanged();
        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnLayoutPropertyChanged();
        static void OnMinimumMaximumValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnMinimumMaximumValuePropertyChanged();
        #endregion

        #region Public Property
        public Boolean ShowSteps
        {
            get => (Boolean)GetValue(ShowStepsProperty);
            set => SetValue(ShowStepsProperty, value);
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
        #endregion

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
        #endregion

        protected void BuildStepper(bool isRangeSlider = false)
        {
            StepContainer.Children.Clear();

            StepContainer.WidthRequest = Width-(isRangeSlider? 2*thumbSize : thumbSize);
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
