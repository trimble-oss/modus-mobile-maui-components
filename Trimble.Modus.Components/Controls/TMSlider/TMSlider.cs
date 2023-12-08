using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Controls;
using Trimble.Modus.Components.Controls.Slider;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;
using static System.Math;

namespace Trimble.Modus.Components
{
    public class TMSlider : SliderCore
    {
        #region Private fields
        readonly PanGestureRecognizer thumbGestureRecognizer = new PanGestureRecognizer();
        double _trackWidth => SliderContainer.Width - ThumbIcon.Width;
        double _lowerTranslation;
        #endregion

        #region EventHandler
        public event EventHandler? ValueChanged;
        #endregion

        #region BindableProperty
        public static BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(TMSlider), .0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);
        public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(nameof(ValueChangedCommand), typeof(ICommand), typeof(TMSlider), null);
        public static readonly BindableProperty ValueChangedCommandParameterProperty = BindableProperty.Create(nameof(ValueChangedCommandParameter), typeof(object), typeof(TMSlider), null, BindingMode.TwoWay);
        #endregion

        #region Public Property
        public ICommand ValueChangedCommand
        {
            get => (ICommand)GetValue(ValueChangedCommandProperty);
            set => SetValue(ValueChangedCommandProperty, value);
        }

        public object ValueChangedCommandParameter
        {
            get => GetValue(ValueChangedCommandParameterProperty);
            set => SetValue(ValueChangedCommandParameterProperty, value);
        }
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        #endregion

        #region UI Elements
        StackLayout ValueHolder = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Spacing = 0,
            Padding = 0
        };
        View ValueToolTipShape = new ToolTipAnchor() { };
        Border ValueBorder = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(4) },
            StrokeThickness = 0
        };
        Label ValueLabel { get; } = SliderHelper.CreateLabelElement();
        Border ThumbIcon = SliderHelper.CreateBorderElement<Border>();
        #endregion

        #region Constructors
        public TMSlider()
        {
            // Initialize the layout and container
            SliderHolderLayout.Orientation = StackOrientation.Horizontal;
            SliderContainer.Children.Add(Track);
            SliderContainer.Children.Add(TrackHighlight);
            SliderContainer.Children.Add(ThumbIcon);

            // Configure the ValueToolTipShape
            ValueToolTipShape.VerticalOptions = LayoutOptions.Start;
            ValueToolTipShape.TranslationY = 0;
            ValueToolTipShape.RotateTo(180);

            // Configure the ValueLabel
            ValueBorder.Content = ValueLabel;
            ValueHolder.Children.Add(ValueBorder);
            ValueHolder.Children.Add(ValueToolTipShape);

            SliderContainer.HorizontalOptions = LayoutOptions.FillAndExpand;

            // Configure Left and Right icons and labels
            LeftIcon.VerticalOptions = LayoutOptions.End;
            RightIcon.VerticalOptions = LayoutOptions.End;
            SliderHolderLayout.Children.Add(LeftLabel);
            SliderHolderLayout.Children.Add(LeftIcon);
            SliderHolderLayout.Children.Add(SliderContainer);
            SliderHolderLayout.Children.Add(RightIcon);
            SliderHolderLayout.Children.Add(RightLabel);

            // Configure ThumbIcon
            ThumbIcon.ZIndex = _thumbZindex;
            AddGestureRecognizer(ThumbIcon, thumbGestureRecognizer);

            // Subscribe to SizeChanged events
            Track.SizeChanged += OnViewSizeChanged;
            ThumbIcon.SizeChanged += OnViewSizeChanged;
            ValueLabel.SizeChanged += OnViewSizeChanged;

            // Perform initial setup based on other properties
            OnIsEnabledChanged();
            SliderContainer.BatchCommitted += OnSliderContainerCommitted;
            OnLayoutPropertyChanged();
        }

        #endregion

        #region Private methods
        private void OnSliderContainerCommitted(object sender, Microsoft.Maui.Controls.Internals.EventArg<VisualElement> e)
        {
            if (!ShowSteps || !ShowToolTip)
            {
                Track.WidthRequest = _trackWidth + _thumbSize / 4;
            }
            else
            {
                Track.WidthRequest = -1;
            }
        }
        /// <summary>
        /// Update Lower / Upper value
        /// </summary>
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

        /// <summary>
        /// Update slider when lower or upper value is changed
        /// </summary>
        void OnLowerUpperValuePropertyChanged()
        {
            var rangeValue = MaximumValue - MinimumValue;
            var trackWidth = _trackWidth;

            _lowerTranslation = (Value - MinimumValue) / rangeValue * trackWidth;

            ThumbIcon.TranslationX = _lowerTranslation;
            OnValueLabelTranslationChanged();

            var bounds = SliderContainer.GetLayoutBounds((IView)TrackHighlight);
            SliderContainer.SetLayoutBounds((IView)TrackHighlight, new Rect(_thumbSize/4, bounds.Y, _lowerTranslation, bounds.Height));
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
                    ValueChangedCommand?.Execute(ValueChangedCommandParameter);
                    SliderHelper.RaiseEvent(this, ValueChanged);
                    break;
            }
        }

        protected override void OnValueLabelTranslationChanged()
        {
            var labelSpacing = 5;
            var lowerLabelTranslation = _lowerTranslation + ((ThumbIcon.Width - ValueLabel.Width) / 2);
            ValueHolder.TranslationX = Min(Max(lowerLabelTranslation, 0), SliderContainer.Width - ValueLabel.Width - labelSpacing);
            if (Value == MaximumValue)
            {
                // The ToolTip is slightly misaligned when it is at the maximum value, this should negate it.
                ValueHolder.TranslationX += 3;
            }
        }

        protected override void OnLayoutPropertyChanged()
        {
            SliderContainer.BatchBegin();
            Track.BatchBegin();
            TrackHighlight.BatchBegin();
            ThumbIcon.BatchBegin();
            ValueLabel.BatchBegin();
            ValueHolder.BatchBegin();
            StepContainer.BatchBegin();
            LastStepContainer.BatchBegin();
            LastLabel.Text = MaximumValue.ToString();

            Track.StrokeThickness = 0;
            TrackHighlight.StrokeThickness = 0;
            ValueLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
            var trackSize = 8;
            _thumbSize = 24;
            var thumbStrokeThickness = 3;
            if (Size == SliderSize.Small)
            {
                trackSize = 4;
                _thumbSize = 20;
                thumbStrokeThickness = 2;
                LeftIcon.Margin = new Thickness(0, 15, 0, 0);
                LeftLabel.Margin = new Thickness(0, 15, 0, 0);
                RightIcon.Margin = new Thickness(0, 15, 0, 0);
                RightLabel.Margin = new Thickness(0, 15, 0, 0);
            }
            else if (Size == SliderSize.Large)
            {
                trackSize = 12;
                _thumbSize = 32;
                thumbStrokeThickness = 4;
                LeftIcon.Margin = new Thickness(0, 0, 0, 5);
                LeftLabel.Margin = new Thickness(0, 0, 0, 5);
                RightIcon.Margin = new Thickness(0, 0, 0, 5);
                RightLabel.Margin = new Thickness(0, 0, 0, 5);
            }
            SetThumbStyle(ThumbIcon, thumbStrokeThickness, _thumbSize);

            Track.StrokeShape = new Rectangle() { RadiusX = 100, RadiusY = 100 };
            TrackHighlight.StrokeShape = new Rectangle() { RadiusX = 100, RadiusY = 100 };
            var labelWithSpacingHeight = Max(ValueLabel.Height, 0);

            var trackThumbHeight = Max(_thumbSize, trackSize);
            var trackVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - trackSize) / 2);
            var thumbVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - _thumbSize) / 2);

            SliderContainer.HeightRequest = labelWithSpacingHeight + trackThumbHeight;

            var trackHighlightBounds = SliderContainer.GetLayoutBounds((IView)TrackHighlight);
            SliderContainer.SetLayoutBounds((IView)TrackHighlight, new Rect(trackHighlightBounds.X, trackVerticalPosition, trackHighlightBounds.Width, trackSize));
            SliderContainer.SetLayoutBounds((IView)Track, new Rect(_thumbSize/4, trackVerticalPosition, _trackWidth+_thumbSize / 4, trackSize));
            SliderContainer.SetLayoutBounds((IView)ThumbIcon, new Rect(0, thumbVerticalPosition, _thumbSize, _thumbSize));

            if (ShowSteps)
            {
                SliderContainer.SetLayoutBounds((IView)StepContainer, new Rect(0, trackVerticalPosition + _stepLabelSpacing, -1, -1));
                SliderContainer.SetLayoutBounds((IView)LastStepContainer, new Rect(_trackWidth, trackVerticalPosition + _stepLabelSpacing, -1, -1));
            }
            if (ShowToolTip)
            {
                SliderContainer.SetLayoutBounds((IView)ValueHolder, new Rect(0, -4, -1, -1));
            }

            SetValueLabelBinding(ValueLabel, ValueProperty);
            SetValueLabelBinding(LeftLabel, LeftTextProperty);
            SetValueLabelBinding(RightLabel, RightTextProperty);
            OnLowerUpperValuePropertyChanged();

            Track.BatchCommit();
            TrackHighlight.BatchCommit();
            ThumbIcon.BatchCommit();
            ValueLabel.BatchCommit();
            ValueHolder.BatchCommit();
            BuildStepper();
            StepContainer.BatchCommit();
            LastStepContainer.BatchCommit();

            SliderContainer.BatchCommit();
        }

        protected override void OnMinimumMaximumValuePropertyChanged()
        {
            Value = SliderHelper.CoerceValue(Value, StepValue, MinimumValue, MaximumValue);
            BuildStepper();
            LastLabel.Text = MaximumValue.ToString();
            OnLowerUpperValuePropertyChanged();
        }

        protected override void OnPanStarted(View view)
        {
            _thumbPositionMap[view] = view.TranslationX;
        }

        protected override void OnPanRunning(View view, double value) =>
            UpdateValue(view, value + GetPanShiftValue(view));

        protected override void OnPanCompleted(View view)
        {
            _thumbPositionMap[view] = view.TranslationX;
        }

        protected override void OnViewSizeChanged(object sender, EventArgs e)
        {
            var maxHeight = ValueLabel.Height;
            if ((sender == ValueLabel) && _labelMaxHeight == maxHeight)
            {
                Device.BeginInvokeOnMainThread(OnValueLabelTranslationChanged);
                return;
            }

            _labelMaxHeight = maxHeight;
            OnLayoutPropertyChanged();
        }
        protected override void OnShowStepsPropertyChanged()
        {
            SliderContainer.Children.Remove(StepContainer);
            SliderContainer.Children.Remove(LastStepContainer);
            LastStepContainer.Children.Remove(LastStepLine);
            LastStepContainer.Children.Remove(LastLabel);
            if (ShowSteps)
            {
                SliderContainer.Children.Add(StepContainer);
                LastStepContainer.Children.Add(LastStepLine);
                LastStepContainer.Children.Add(LastLabel);
                SliderContainer.Children.Add(LastStepContainer);
            }
        }

        protected override void OnShowToolTipPropertyChanged()
        {
            SliderContainer.Children.Remove(ValueHolder);
            if (ShowToolTip)
            {
                SliderContainer.Children.Add(ValueHolder);
            }
            OnTitleTextPropertyChanged(SliderTitle.Text);
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

        protected override void OnThumbColorPropertyChanged(Color newValue)
        {
            RefreshThumbColor(ThumbIcon);
        }
        protected override void OnTrackBackgroundColorChanged(Color newValue)
        {
            Track.BackgroundColor = newValue;
        }
        protected override void OnTrackHighlightColorChanged(Color newValue)
        {
            TrackHighlight.BackgroundColor = newValue;
        }
        protected override void OnToolTipBackgroundColorChanged(Color newValue)
        {
            if (ValueLabel != null) ValueLabel.BackgroundColor = newValue;
        }
        #endregion
    }
}
