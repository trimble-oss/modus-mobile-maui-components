using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Enums;
using static System.Math;

namespace Trimble.Modus.Components.Controls
{
    public abstract class SliderControl : AbsoluteLayout
    {
        const double enabledOpacity = 1;

        const double disabledOpacity = .5;
        public static BindableProperty MinimumValueProperty
                    = BindableProperty.Create(nameof(MinimumValue), typeof(double), typeof(SliderControl), .0, propertyChanged: OnMinimumMaximumValuePropertyChanged);

        public static BindableProperty MaximumValueProperty
            = BindableProperty.Create(nameof(MaximumValue), typeof(double), typeof(SliderControl), 1.0, propertyChanged: OnMinimumMaximumValuePropertyChanged);

        public static BindableProperty StepValueProperty
            = BindableProperty.Create(nameof(StepValue), typeof(double), typeof(SliderControl), 0.0, propertyChanged: OnMinimumMaximumValuePropertyChanged);

        public static BindableProperty SizeProperty
            = BindableProperty.Create(nameof(Size), typeof(SliderSize), typeof(SliderControl), SliderSize.Medium, propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty ValueLabelStyleProperty
            = BindableProperty.Create(nameof(ValueLabelStyle), typeof(Style), typeof(SliderControl), propertyChanged: OnLayoutPropertyChanged);


        public static BindableProperty ValueLabelStringFormatProperty
            = BindableProperty.Create(nameof(ValueLabelStringFormat), typeof(string), typeof(SliderControl), "{0:0.##}", propertyChanged: OnLayoutPropertyChanged);

        public static BindableProperty ValueLabelSpacingProperty
            = BindableProperty.Create(nameof(ValueLabelSpacing), typeof(double), typeof(SliderControl), 5.0, propertyChanged: OnLayoutPropertyChanged);
        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderControl)bindable).OnLayoutPropertyChanged();

        protected readonly Dictionary<View, double> thumbPositionMap = new Dictionary<View, double>();

        Microsoft.Maui.Graphics.Size allocatedSize;

        protected double labelMaxHeight;

        protected int dragCount;

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
        internal Border Track { get; } = CreateBorderElement<Border>();

        internal Border TrackHighlight { get; } = CreateBorderElement<Border>();


        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width == allocatedSize.Width && height == allocatedSize.Height)
                return;

            allocatedSize = new Microsoft.Maui.Graphics.Size(width, height);
            OnLayoutPropertyChanged();
        }
        protected abstract void OnLayoutPropertyChanged();
        protected static Border CreateBorderElement<TBorder>() where TBorder : Border, new()
        {
            var border = new Border
            {
                Padding = 0
            };

            return border;
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

        protected static Label CreateLabelElement()
            => new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            };

        static void OnMinimumMaximumValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((SliderControl)bindable).OnMinimumMaximumValuePropertyChanged();
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

        protected double CoerceValue(double value)
        {
            if (StepValue > 0 && value < MaximumValue)
            {
                var stepIndex = (int)((value - MinimumValue) / StepValue);
                value = MinimumValue + (stepIndex * StepValue);
            }
            return Clamp(value, MinimumValue, MaximumValue);
        }

        protected abstract void OnMinimumMaximumValuePropertyChanged();
        protected abstract void OnViewSizeChanged(object? sender, System.EventArgs e);

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
        protected abstract void OnPanStarted(View view);
        protected abstract void OnPanRunning(View view, double value);
        protected abstract void OnPanCompleted(View view);
        protected abstract void OnValueLabelTranslationChanged();
        protected double GetPanShiftValue(View view)
            => Device.RuntimePlatform == Device.Android
                ? view.TranslationX
                : thumbPositionMap[view];
        protected void AddGestureRecognizer(View view, PanGestureRecognizer gestureRecognizer)
        {
            gestureRecognizer.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(gestureRecognizer);
        }

        protected void RaiseEvent(EventHandler? eventHandler)
            => eventHandler?.Invoke(this, EventArgs.Empty);
        protected void SetValueLabelBinding(Label label, BindableProperty bindableProperty)
            => label.SetBinding(Label.TextProperty, new Binding
            {
                Source = this,
                Path = bindableProperty.PropertyName,
                StringFormat = ValueLabelStringFormat
            });
    }
}
