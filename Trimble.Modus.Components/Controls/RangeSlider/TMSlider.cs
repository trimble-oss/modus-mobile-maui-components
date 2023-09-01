using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using Trimble.Modus.Components.Controls;
using Trimble.Modus.Components.Controls.RangeSlider;
using Trimble.Modus.Components.Controls.Slider;
using Trimble.Modus.Components.Enums;
using static System.Math;

namespace Trimble.Modus.Components
{
    public class TMSlider : SliderCore
    {
        #region Private fields
        readonly PanGestureRecognizer thumbGestureRecognizer = new PanGestureRecognizer();
        double _trackWidth => AbsoluteLayout.Width - ThumbIcon.Width;
        double _lowerTranslation;
        #endregion

        #region EventHandler
        public event EventHandler? ValueChanged;

        public event EventHandler? DragStarted;

        public event EventHandler? DragCompleted;
        #endregion

        #region BindableProperty
        public static BindableProperty ValueProperty
                = BindableProperty.Create(nameof(Value), typeof(double), typeof(TMSlider), .0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);
        #endregion

        #region Public Property

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        #endregion

        #region UI Elements
        StackLayout ValueHolder = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 0, Padding = 0 };
        View ValueToolTipShape = new ToolTipAnchor() { };
        Border ValueBorder = new Border { StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(4) }, StrokeThickness = 0 };
        Label ValueLabel { get; } = SliderHelper.CreateLabelElement();
        Border ThumbIcon = SliderHelper.CreateBorderElement<Border>();
        #endregion

        #region Constructors
        public TMSlider()
        {
            sliderHolderLayout.Orientation = StackOrientation.Horizontal;
            AbsoluteLayout.Children.Add(Track);
            AbsoluteLayout.Children.Add(TrackHighlight);
            AbsoluteLayout.Children.Add(ThumbIcon);
            ValueToolTipShape.VerticalOptions = LayoutOptions.Start;
            ValueToolTipShape.TranslationY = 0;
            ValueToolTipShape.RotateTo(180);
            ValueLabel.BackgroundColor = Color.FromArgb("#585C65");
            ValueBorder.Content = ValueLabel;
            ValueHolder.Children.Add(ValueBorder);
            ValueHolder.Children.Add(ValueToolTipShape);


            AbsoluteLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            LeftIcon.VerticalOptions = LayoutOptions.End;
            RightIcon.VerticalOptions = LayoutOptions.End;
            sliderHolderLayout.Children.Add(LeftLabel);
            sliderHolderLayout.Children.Add(LeftIcon);
            sliderHolderLayout.Children.Add(AbsoluteLayout);
            sliderHolderLayout.Children.Add(RightIcon);
            sliderHolderLayout.Children.Add(RightLabel);

            ThumbIcon.ZIndex = 3;

            AddGestureRecognizer(ThumbIcon, thumbGestureRecognizer);

            Track.SizeChanged += OnViewSizeChanged;
            ThumbIcon.SizeChanged += OnViewSizeChanged;
            ValueLabel.SizeChanged += OnViewSizeChanged;

            OnIsEnabledChanged();
            OnLayoutPropertyChanged();
        }
        #endregion

        #region Private methods
        void UpdateValue(View view, double value)
        {
            var rangeValue = MaximumValue - MinimumValue;
            Value = Min(Max(MinimumValue, (value / _trackWidth * rangeValue) + MinimumValue), MaximumValue);
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
            var trackWidth = _trackWidth;

            _lowerTranslation = (Value - MinimumValue) / rangeValue * trackWidth;

            ThumbIcon.TranslationX = _lowerTranslation;
            OnValueLabelTranslationChanged();

            var bounds = AbsoluteLayout.GetLayoutBounds((IView)TrackHighlight);
            AbsoluteLayout.SetLayoutBounds((IView)TrackHighlight, new Rect(thumbSize/4, bounds.Y, _lowerTranslation, bounds.Height));
        }
        #endregion

        #region Protected Methods
        protected override void OnLeftIconSourceChanged()
        {
            LeftIcon.Source = LeftIconSource;
            if (LeftIconSource != null)
            {
                LeftIcon.WidthRequest = 20;
                LeftIcon.IsVisible = true;
            }
            else
            {
                LeftIcon.WidthRequest = 0;
                LeftIcon.IsVisible = false;
            }
        }
        protected override void OnRightIconSourceChanged()
        {
            RightIcon.Source = RightIconSource;
            if (RightIconSource != null)
            {
                RightIcon.WidthRequest = 20;
                RightIcon.IsVisible = true;
            }
            else
            {
                RightIcon.WidthRequest = 0;
                RightIcon.IsVisible = false;
            }
        }
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
        protected override void OnValueLabelTranslationChanged()
        {
            var labelSpacing = 5;
            var lowerLabelTranslation = _lowerTranslation + ((ThumbIcon.Width - ValueLabel.Width) / 2);
            ValueHolder.TranslationX = Min(Max(lowerLabelTranslation, 0), AbsoluteLayout.Width - ValueLabel.Width - labelSpacing);
            if (Value == MaximumValue)
            {
                ValueHolder.TranslationX += 3;
            }
        }
        protected override void OnLayoutPropertyChanged()
        {
            AbsoluteLayout.BatchBegin();
            Track.BatchBegin();
            TrackHighlight.BatchBegin();
            ThumbIcon.BatchBegin();
            ValueLabel.BatchBegin();
            ValueHolder.BatchBegin();
            StepContainer.BatchBegin();
            LastStepContainer.BatchBegin();
            LastLabel.Text = MaximumValue.ToString();

            Track.BackgroundColor = Color.FromArgb("#FFA3A6B1");
            Track.StrokeThickness = 0;
            TrackHighlight.BackgroundColor = Color.FromArgb("#FF0063A3");
            TrackHighlight.StrokeThickness = 0;
            ValueLabel.TextColor = Colors.White;
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

            var trackThumbHeight = Max(thumbSize, trackSize);
            var trackVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - trackSize) / 2);
            var thumbVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - thumbSize) / 2);

            AbsoluteLayout.HeightRequest = labelWithSpacingHeight + trackThumbHeight;

            var trackHighlightBounds = AbsoluteLayout.GetLayoutBounds((IView)TrackHighlight);
            AbsoluteLayout.SetLayoutBounds((IView)TrackHighlight, new Rect(trackHighlightBounds.X, trackVerticalPosition, trackHighlightBounds.Width, trackSize));
            AbsoluteLayout.SetLayoutBounds((IView)Track, new Rect(thumbSize/4, trackVerticalPosition, _trackWidth+thumbSize / 4, trackSize));
            AbsoluteLayout.SetLayoutBounds((IView)ThumbIcon, new Rect(0, thumbVerticalPosition, thumbSize, thumbSize));

            if (ShowSteps)
            {
                AbsoluteLayout.SetLayoutBounds((IView)StepContainer, new Rect(0, trackVerticalPosition + 20, -1, -1));
                AbsoluteLayout.SetLayoutBounds((IView)LastStepContainer, new Rect(_trackWidth, trackVerticalPosition + 20, -1, -1));
            }
            if (ShowToolTip)
            {
                AbsoluteLayout.SetLayoutBounds((IView)ValueHolder, new Rect(0, -4, -1, -1));
            }

            SetValueLabelBinding(ValueLabel, ValueProperty);
            SetTitleLabelBinding(LeftLabel, LeftTextProperty);
            SetTitleLabelBinding(RightLabel, RightTextProperty);
            OnLowerUpperValuePropertyChanged();

            Track.BatchCommit();
            TrackHighlight.BatchCommit();
            ThumbIcon.BatchCommit();
            ValueLabel.BatchCommit();
            ValueHolder.BatchCommit();
            BuildStepper();
            StepContainer.BatchCommit();
            LastStepContainer.BatchCommit();

            AbsoluteLayout.BatchCommit();
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
                AbsoluteLayout.Children.Add(StepContainer);
                LastStepContainer.Children.Add(LastStepLine);
                LastStepContainer.Children.Add(LastLabel);
                AbsoluteLayout.Children.Add(LastStepContainer);
                OnLayoutPropertyChanged();
            }
            else
            {
                AbsoluteLayout.Children.Remove(StepContainer);
                AbsoluteLayout.Children.Remove(LastStepContainer);
                LastStepContainer.Children.Remove(LastStepLine);
                LastStepContainer.Children.Remove(LastLabel);
            }
        }
        protected override void OnShowToolTipPropertyChanged()
        {
            if (ShowToolTip)
            {
                AbsoluteLayout.Children.Add(ValueHolder);
            }
            else
            {
                AbsoluteLayout.Children.Remove(ValueHolder);
            }
            OnTitleTextPropertyChanged(SliderTitle.Text);
            OnLayoutPropertyChanged();
        }
        protected override void OnTitleTextPropertyChanged(string newValue)
        {
            SliderTitle.Text = newValue;
            SliderTitle.Margin = (ShowToolTip) ? new Thickness(0,0,0,10) : new Thickness(0, 10, 0, 0);

            if (!string.IsNullOrEmpty(newValue))
            {
                Children.Remove(SliderTitle);
                Children.Add(SliderTitle);
            }
            else
            {
                Children.Remove(SliderTitle);
            }
            OnLayoutPropertyChanged();
        }
        #endregion
    }
}
