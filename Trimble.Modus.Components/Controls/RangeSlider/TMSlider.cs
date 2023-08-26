using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using Trimble.Modus.Components.Controls;
using Trimble.Modus.Components.Controls.Slider;
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
                = BindableProperty.Create(nameof(Value), typeof(double), typeof(TMSlider), .0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);

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
                    SliderHelper.RaiseEvent(this, ValueChanged);
                    break;
            }
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        Label ValueLabel { get; } = SliderHelper.CreateLabelElement();
        Border ThumbIcon = SliderHelper.CreateBorderElement<Border>();
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
            ValueLabel.SizeChanged += OnViewSizeChanged;

            OnIsEnabledChanged();
            OnLayoutPropertyChanged();
        }
        static object CoerceValue(BindableObject bindable, object value)
        {
            var slider = (bindable as TMSlider);
            return SliderHelper.CoerceValue((double)value, slider.StepValue, slider.MinimumValue, slider.MaximumValue);
        }
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
            this.SetLayoutBounds(TrackHighlight, new Rect(0, bounds.Y, lowerTranslation+32, bounds.Height));
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
            StepContainer.BatchBegin();
            LastStepContainer.BatchBegin();
            LastLabel.Text = MaximumValue.ToString();

            Track.BackgroundColor = Color.FromArgb("#FFA3A6B1");
            Track.StrokeThickness = 0;
            TrackHighlight.BackgroundColor = Color.FromArgb("#FF0063A3");
            TrackHighlight.StrokeThickness = 0;

            var trackSize = 8;
            thumbSize = 24;
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

            if (ShowSteps)
            {
                SetLayoutBounds((IView)StepContainer, new Rect(0, trackVerticalPosition + 20, -1, -1));
                SetLayoutBounds((IView)LastStepContainer, new Rect(TrackWidth, trackVerticalPosition + 20, -1, -1));
            }

            SetValueLabelBinding(ValueLabel, ValueProperty);
            ValueLabel.Style = ValueLabelStyle ?? ValueLabelStyle;
            OnLowerUpperValuePropertyChanged();

            Track.BatchCommit();
            TrackHighlight.BatchCommit();
            ThumbIcon.BatchCommit();
            ValueLabel.BatchCommit();
            StepContainer.BatchCommit();
            LastStepContainer.BatchCommit();
            BuildStepper();

            BatchCommit();
        }

        protected override void OnMinimumMaximumValuePropertyChanged()
        {
            Value = SliderHelper.CoerceValue(Value, StepValue, MinimumValue, MaximumValue);
            OnLowerUpperValuePropertyChanged();
        }

        protected override void OnPanStarted(View view)
        {
            thumbPositionMap[view] = view.TranslationX;
            SliderHelper.RaiseEvent(this, DragStarted);
        }

        protected override void OnPanRunning(View view, double value)
            => UpdateValue(view, value + GetPanShiftValue(view));

        protected override void OnPanCompleted(View view)
        {
            thumbPositionMap[view] = view.TranslationX;
            SliderHelper.RaiseEvent(this, DragCompleted);
        }

        protected override void OnViewSizeChanged(object sender, EventArgs e)
        {
            var maxHeight = ValueLabel.Height;
            if ((sender == ValueLabel) && labelMaxHeight == maxHeight)
            {
                Device.BeginInvokeOnMainThread(OnValueLabelTranslationChanged);
                return;
            }

            labelMaxHeight = maxHeight;
            OnLayoutPropertyChanged();
        }
        protected override void OnShowStepsPropertyChanged()
        {
            if (ShowSteps)
            {
                Children.Add(StepContainer);
                LastStepContainer.Children.Add(LastStepLine);
                LastStepContainer.Children.Add(LastLabel);
                Children.Add(LastStepContainer);
                OnLayoutPropertyChanged();
            }
            else
            {
                Children.Remove(StepContainer);
                Children.Remove(LastStepContainer);
            }
        }
        void UpdateValue(View view, double value)
        {
            var rangeValue = MaximumValue - MinimumValue;
            Value = Min(Max(MinimumValue, (value / TrackWidth * rangeValue) + MinimumValue), MaximumValue);
        }
    }
}
