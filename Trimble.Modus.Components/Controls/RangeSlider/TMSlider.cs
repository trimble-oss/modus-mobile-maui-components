using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using Trimble.Modus.Components.Controls;
using Trimble.Modus.Components.Enums;
using static System.Math;

namespace Trimble.Modus.Components
{
    public class TMSlider : SliderControl
    {
        public event EventHandler? ValueChanged;

        public event EventHandler? DragStarted;

        public event EventHandler? DragCompleted;

        public static BindableProperty ValueProperty
                = BindableProperty.Create(nameof(Value), typeof(double), typeof(RangeSlider), .0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);

        readonly PanGestureRecognizer thumbGestureRecognizer = new PanGestureRecognizer();
        double thumbTranslation;

        double lowerTranslation;
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(IsEnabled):
                    OnIsEnabledChanged();
                    break;
                case nameof(Value):
                    RaiseEvent(ValueChanged);
                    break;
            }
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        Label ValueLabel { get; } = CreateLabelElement();
        Border ThumbIcon = CreateBorderElement<Border>();
        double TrackWidth => Width - ThumbIcon.Width;

        public TMSlider()
        {
            Children.Add(Track);
            Children.Add(TrackHighlight);
            Children.Add(ThumbIcon);
            Children.Add(ValueLabel);
            ThumbIcon.ZIndex = 3;

            AddGestureRecognizer(ThumbIcon, thumbGestureRecognizer);

            Track.SizeChanged += OnViewSizeChanged;
            ThumbIcon.SizeChanged += OnViewSizeChanged;
            OnIsEnabledChanged();
            OnLayoutPropertyChanged();
        }
        static object CoerceValue(BindableObject bindable, object value)
            => ((TMSlider)bindable).CoerceValue((double)value);
        static void OnLowerUpperValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((TMSlider)bindable).OnLowerUpperValuePropertyChanged();

        void OnLowerUpperValuePropertyChanged()
        {
            var rangeValue = MaximumValue - MinimumValue;
            var trackWidth = TrackWidth;

            lowerTranslation = (Value - MinimumValue) / rangeValue * trackWidth;

            ThumbIcon.TranslationX = lowerTranslation;
            OnValueLabelTranslationChanged();

            var bounds = GetLayoutBounds((IView)TrackHighlight);
            this.SetLayoutBounds(TrackHighlight, new Rect(lowerTranslation, bounds.Y, lowerTranslation, bounds.Height));
        }
        protected override void OnValueLabelTranslationChanged()
        {
            var labelSpacing = 5;
            var lowerLabelTranslation = lowerTranslation + ((ThumbIcon.Width - ValueLabel.Width) / 2);
            ValueLabel.TranslationX = Min(Max(lowerLabelTranslation, 0), Width - ValueLabel.Width - labelSpacing);
        }
        protected override void OnLayoutPropertyChanged()
        {
            BatchBegin();
            Track.BatchBegin();
            TrackHighlight.BatchBegin();
            ThumbIcon.BatchBegin();
            ValueLabel.BatchBegin();

            Track.BackgroundColor = Color.FromArgb("#FFA3A6B1");
            Track.StrokeThickness = 0;
            TrackHighlight.BackgroundColor = Color.FromArgb("#FF0063A3");
            TrackHighlight.StrokeThickness = 0;

            var trackSize = 8;
            var thumbSize = 24;
            var thumbStrokeThickness = 3;
            var thumbRadius = 13;
            if (Size == SliderSize.Small)
            {
                trackSize = 4;
                thumbSize = 20;
                thumbStrokeThickness = 2;
                thumbRadius = 10;
            }
            else if (Size == SliderSize.Large)
            {
                trackSize = 12;
                thumbSize = 32;
                thumbStrokeThickness = 4;
                thumbRadius = 16;
            }
            SetThumbStyle(ThumbIcon, thumbStrokeThickness, thumbSize, thumbRadius);

            Track.StrokeShape = new Rectangle() { RadiusX = 100, RadiusY = 100 };
            TrackHighlight.StrokeShape = new Rectangle() { RadiusX = 100, RadiusY = 100 };
            var labelWithSpacingHeight = Max(ValueLabel.Height, 0);
            if (labelWithSpacingHeight > 0)
                labelWithSpacingHeight += ValueLabelSpacing;

            var trackThumbHeight = Max(Max(thumbSize, thumbSize), trackSize);
            var trackVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - trackSize) / 2);
            var thumbVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - thumbSize) / 2);

            this.HeightRequest = labelWithSpacingHeight + trackThumbHeight;

            var trackHighlightBounds = GetLayoutBounds((IView)TrackHighlight);
            SetLayoutBounds((IView)TrackHighlight, new Rect(trackHighlightBounds.X, trackVerticalPosition, trackHighlightBounds.Width, trackSize));
            SetLayoutBounds((IView)Track, new Rect(0, trackVerticalPosition, Width, trackSize));
            SetLayoutBounds((IView)ThumbIcon, new Rect(0, thumbVerticalPosition, thumbSize, thumbSize));
            SetLayoutBounds((IView)ValueLabel, new Rect(0, 0, -1, -1));
            SetValueLabelBinding(ValueLabel, ValueProperty);
            ValueLabel.Style = ValueLabelStyle ?? ValueLabelStyle;
            OnLowerUpperValuePropertyChanged();

            Track.BatchCommit();
            TrackHighlight.BatchCommit();
            ThumbIcon.BatchCommit();
            ValueLabel.BatchCommit();
            BatchCommit();
        }

        protected override void OnMinimumMaximumValuePropertyChanged()
        {
            Value = CoerceValue(Value);
            OnLowerUpperValuePropertyChanged();
        }

        protected override void OnPanStarted(View view)
        {
            thumbPositionMap[view] = view.TranslationX;
            RaiseEvent(DragStarted);

            if (Interlocked.Increment(ref dragCount) == 1)
                RaiseEvent(DragStarted);
        }

        protected override void OnPanRunning(View view, double value)
            => UpdateValue(view, value + GetPanShiftValue(view));

        protected override void OnPanCompleted(View view)
        {
            thumbPositionMap[view] = view.TranslationX;
            RaiseEvent(DragCompleted);

            if (Interlocked.Decrement(ref dragCount) == 0)
                RaiseEvent(DragCompleted);
        }

        protected override void OnViewSizeChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        void UpdateValue(View view, double value)
        {
            var rangeValue = MaximumValue - MinimumValue;
            Value = Min(Max(Value, ((value - ThumbIcon.Width) / TrackWidth * rangeValue) + MinimumValue), MaximumValue);
        }
    }
}
