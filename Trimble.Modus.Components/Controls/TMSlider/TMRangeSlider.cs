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
    public class TMRangeSlider : SliderCore
    {
        #region Event Handlers
        public event EventHandler? LowerValueChanged;

        public event EventHandler? UpperValueChanged;
        #endregion

        #region Bindable Property
        public static BindableProperty LowerValueProperty = BindableProperty.Create(nameof(LowerValue), typeof(double), typeof(TMRangeSlider), .0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);
		public static BindableProperty UpperValueProperty = BindableProperty.Create(nameof(UpperValue), typeof(double), typeof(TMRangeSlider), 1.0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);
        public static readonly BindableProperty LowerValueChangedCommandProperty = BindableProperty.Create(nameof(LowerValueChangedCommand), typeof(ICommand), typeof(TMRangeSlider), null);
        public static readonly BindableProperty LowerValueChangedCommandParameterProperty = BindableProperty.Create(nameof(LowerValueChangedCommandParameter), typeof(object), typeof(TMRangeSlider), null, BindingMode.TwoWay);
        public static readonly BindableProperty UpperValueChangedCommandProperty = BindableProperty.Create(nameof(UpperValueChangedCommand), typeof(ICommand), typeof(TMRangeSlider), null);
        public static readonly BindableProperty UpperValueChangedCommandParameterProperty = BindableProperty.Create(nameof(UpperValueChangedCommandParameter), typeof(object), typeof(TMRangeSlider), null, BindingMode.TwoWay);
        #endregion

        #region Private fields
        readonly PanGestureRecognizer lowerThumbGestureRecognizer = new PanGestureRecognizer();

        readonly PanGestureRecognizer upperThumbGestureRecognizer = new PanGestureRecognizer();

        double _lowerTranslation;

        double _upperTranslation;
        double _trackWidth => SliderContainer.Width - _thumbSize * 2;
        #endregion

        #region Public Property
        public double LowerValue
        {
            get => (double)GetValue(LowerValueProperty);
            set => SetValue(LowerValueProperty, value);
        }

        public double UpperValue
        {
            get => (double)GetValue(UpperValueProperty);
            set => SetValue(UpperValueProperty, value);
        }
        public ICommand LowerValueChangedCommand
        {
            get => (ICommand)GetValue(LowerValueChangedCommandProperty);
            set => SetValue(LowerValueChangedCommandProperty, value);
        }

        public object LowerValueChangedCommandParameter
        {
            get => GetValue(LowerValueChangedCommandParameterProperty);
            set => SetValue(LowerValueChangedCommandParameterProperty, value);
        }
        public ICommand UpperValueChangedCommand
        {
            get => (ICommand)GetValue(UpperValueChangedCommandProperty);
            set => SetValue(UpperValueChangedCommandProperty, value);
        }

        public object UpperValueChangedCommandParameter
        {
            get => GetValue(UpperValueChangedCommandParameterProperty);
            set => SetValue(UpperValueChangedCommandParameterProperty, value);
        }
        #endregion

        #region UI Elements
        StackLayout UpperValueHolder = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Spacing = 0,
            Padding = 0
        };
        View UpperValueToolTipShape = new ToolTipAnchor() { };
        Border UpperValueBorder = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(4) },
            StrokeThickness = 0
        };
        StackLayout LowerValueHolder = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Spacing = 0,
            Padding = 0
        };
        Border LowerValueBorder = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(4) },
            StrokeThickness = 0
        };
        View LowerValueToolTipShape = new ToolTipAnchor() { };
        Label LowerValueLabel { get; } = SliderHelper.CreateLabelElement();
        Label UpperValueLabel { get; } = SliderHelper.CreateLabelElement();
        Border RightThumbIcon = SliderHelper.CreateBorderElement<Border>();
        Border LeftThumbIcon = SliderHelper.CreateBorderElement<Border>();
        #endregion

        #region Constructors
        public TMRangeSlider()
        {
            SliderHolderLayout.Orientation = StackOrientation.Horizontal;
            SliderContainer.Children.Add(Track);
            SliderContainer.Children.Add(TrackHighlight);
            SliderContainer.Children.Add(LeftThumbIcon);
            SliderContainer.Children.Add(RightThumbIcon);

            UpperValueToolTipShape.VerticalOptions = LayoutOptions.Start;
            UpperValueToolTipShape.TranslationY = 0;
            UpperValueToolTipShape.RotateTo(180);

            UpperValueBorder.Content = UpperValueLabel;
            UpperValueHolder.Children.Add(UpperValueBorder);
            UpperValueHolder.Children.Add(UpperValueToolTipShape);

            LowerValueToolTipShape.VerticalOptions = LayoutOptions.Start;
            LowerValueToolTipShape.TranslationY = 0;
            LowerValueToolTipShape.RotateTo(180);

            LowerValueBorder.Content = LowerValueLabel;
            LowerValueHolder.Children.Add(LowerValueBorder);
            LowerValueHolder.Children.Add(LowerValueToolTipShape);
            SliderContainer.HorizontalOptions = LayoutOptions.FillAndExpand;
            LeftIcon.VerticalOptions = LayoutOptions.End;
            RightIcon.VerticalOptions = LayoutOptions.End;
            SliderHolderLayout.Children.Add(LeftLabel);
            SliderHolderLayout.Children.Add(LeftIcon);
            SliderHolderLayout.Children.Add(SliderContainer);
            SliderHolderLayout.Children.Add(RightIcon);
            SliderHolderLayout.Children.Add(RightLabel);

            RightThumbIcon.ZIndex = _thumbZindex;
            LeftThumbIcon.ZIndex = _thumbZindex;

            AddGestureRecognizer(LeftThumbIcon, lowerThumbGestureRecognizer);
            AddGestureRecognizer(RightThumbIcon, upperThumbGestureRecognizer);

            Track.SizeChanged += OnViewSizeChanged;
            LeftThumbIcon.SizeChanged += OnViewSizeChanged;
            RightThumbIcon.SizeChanged += OnViewSizeChanged;
            LowerValueLabel.SizeChanged += OnViewSizeChanged;
            UpperValueLabel.SizeChanged += OnViewSizeChanged;
            SliderContainer.BatchCommitted += OnSliderContainerCommitted;
            OnIsEnabledChanged();
            OnLayoutPropertyChanged();
        }
        #endregion

        #region Private methods
        static void OnLowerUpperValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((TMRangeSlider)bindable).OnLowerUpperValuePropertyChanged();

        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((TMRangeSlider)bindable).OnLayoutPropertyChanged();

        static object CoerceValue(BindableObject bindable, object value)
        {
            var slider = (TMRangeSlider)bindable;
            return SliderHelper.CoerceValue((double)value, slider.StepValue, slider.MinimumValue, slider.MaximumValue);
        }
        void OnSliderContainerCommitted(object sender, Microsoft.Maui.Controls.Internals.EventArg<VisualElement> e)
        {
            if (!ShowSteps || !ShowToolTip)
            {
                Track.WidthRequest = _trackWidth + 1.25 * _thumbSize;
            }
            else
            {
                Track.WidthRequest = -1;
            }
        }
        /// <summary>
        /// Update slider when lower or upper value is changed
        /// </summary>
        void OnLowerUpperValuePropertyChanged()
        {
            var rangeValue = MaximumValue - MinimumValue;
            var trackWidth = _trackWidth + _thumbSize;

            _lowerTranslation = (LowerValue - MinimumValue) / rangeValue * trackWidth;
            _upperTranslation = ((UpperValue - MinimumValue) / rangeValue * trackWidth);
            if (UpperValue == LowerValue)
            {
                if (UpperValue == MaximumValue)
                {
                    _lowerTranslation -= _thumbSize;
                }
                else
                {
                    _upperTranslation += _thumbSize;
                }
            }

            LeftThumbIcon.TranslationX = _lowerTranslation;
            RightThumbIcon.TranslationX = _upperTranslation;
            OnValueLabelTranslationChanged();

            var bounds = SliderContainer.GetLayoutBounds((IView)TrackHighlight);
            SliderContainer.SetLayoutBounds(
                (IView)TrackHighlight,
                new Rect(
                    _lowerTranslation + _thumbSize / 2,
                    bounds.Y,
                    _upperTranslation - _lowerTranslation,
                    bounds.Height
                )
            );
        }

        /// <summary>
        /// Update Lower / Upper value
        /// </summary>
        void UpdateValue(View view, double value)
        {
            var rangeValue = MaximumValue - MinimumValue;
            if (view == LeftThumbIcon)
            {
                LowerValue = Min(Max(MinimumValue, ((value - _thumbSize / 2.5) / _trackWidth * rangeValue) + MinimumValue), UpperValue);
                return;
            }
            UpperValue = Min(Max(LowerValue, ((value - LeftThumbIcon.Width) / _trackWidth * rangeValue) + MinimumValue), MaximumValue);
        }
        #endregion

        #region Protected Methods
        protected override void OnMinimumMaximumValuePropertyChanged()
        {
            LowerValue = SliderHelper.CoerceValue(LowerValue, StepValue, MinimumValue, MaximumValue);
            UpperValue = SliderHelper.CoerceValue(UpperValue, StepValue, MinimumValue, MaximumValue);
            BuildStepper();
            LastLabel.Text = MaximumValue.ToString();
            OnLowerUpperValuePropertyChanged();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(IsEnabled):
                    OnIsEnabledChanged();
                    break;
                case nameof(LowerValue):
                    LowerValueChangedCommand?.Execute(LowerValueChangedCommandParameter);
                    SliderHelper.RaiseEvent(this, LowerValueChanged);
                    break;
                case nameof(UpperValue):
                    UpperValueChangedCommand?.Execute(UpperValueChangedCommandParameter);
                    SliderHelper.RaiseEvent(this, UpperValueChanged);
                    break;
            }
        }

        protected override void OnValueLabelTranslationChanged()
		{
			var labelSpacing = 5;
			var lowerLabelTranslation = _lowerTranslation + ((LeftThumbIcon.Width - LowerValueLabel.Width) / 2);
			var upperLabelTranslation = _upperTranslation + ((RightThumbIcon.Width - UpperValueLabel.Width) / 2);
			LowerValueBorder.TranslationX = Min(Max(lowerLabelTranslation, 0), SliderContainer.Width - LowerValueLabel.Width - UpperValueLabel.Width - labelSpacing);
            LowerValueToolTipShape.TranslationX = Min(Max(lowerLabelTranslation, 0), SliderContainer.Width - LowerValueLabel.Width - UpperValueLabel.Width - labelSpacing);
			UpperValueBorder.TranslationX = Min(Max(upperLabelTranslation, LowerValueLabel.TranslationX + LowerValueLabel.Width + labelSpacing), SliderContainer.Width - UpperValueLabel.Width);
            UpperValueToolTipShape.TranslationX = Min(Max(upperLabelTranslation, LowerValueLabel.TranslationX + LowerValueLabel.Width + labelSpacing), SliderContainer.Width - UpperValueLabel.Width);
		}

		protected override void OnLayoutPropertyChanged()
		{
            SliderContainer.BatchBegin();
            Track.BatchBegin();
            TrackHighlight.BatchBegin();
            LeftThumbIcon.BatchBegin();
            RightThumbIcon.BatchBegin();
            LowerValueLabel.BatchBegin();
            UpperValueLabel.BatchBegin();
            LowerValueBorder.BatchBegin();
            UpperValueBorder.BatchBegin();
            UpperValueHolder.BatchBegin();
            LowerValueHolder.BatchBegin();

            StepContainer.BatchBegin();
            LastStepContainer.BatchBegin();
            LastLabel.Text = MaximumValue.ToString();

            Track.StrokeThickness = 0;
            TrackHighlight.StrokeThickness = 0;
            UpperValueLabel.TextColor = ResourcesDictionary.GetColor(ColorsConstants.DefaultTextColor);
            LowerValueLabel.TextColor = ResourcesDictionary.GetColor(ColorsConstants.DefaultTextColor);

            var trackSize = 8;
            _thumbSize = 24;
            var thumbStrokeThickness = 3;
            if (Size == SliderSize.Small)
            {
                trackSize = 4;
                _thumbSize = 20;
                thumbStrokeThickness = 2;
                LeftIcon.Margin = new Thickness(0, 20, 0, 0);
                LeftLabel.Margin = new Thickness(0, 20, 0, 0);
                RightIcon.Margin = new Thickness(0, 20, 0, 0);
                RightLabel.Margin = new Thickness(0, 20, 0, 0);
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
            SetThumbStyle(LeftThumbIcon, thumbStrokeThickness, _thumbSize);
            SetThumbStyle(RightThumbIcon, thumbStrokeThickness, _thumbSize);

            Track.StrokeShape = new Rectangle() { RadiusX = 100, RadiusY = 100 };
            TrackHighlight.StrokeShape = new Rectangle() { RadiusX = 100, RadiusY = 100 };

			var labelWithSpacingHeight = Max(Max(LowerValueLabel.Height, UpperValueLabel.Height), 0);

			var trackThumbHeight = Max(Max(_thumbSize, _thumbSize), trackSize);
			var trackVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - trackSize) / 2);
			var lowerThumbVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - _thumbSize) / 2);
			var upperThumbVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - _thumbSize) / 2);

            SliderContainer.HeightRequest = labelWithSpacingHeight + trackThumbHeight;

            var trackHighlightBounds = SliderContainer.GetLayoutBounds((IView)TrackHighlight);
            SliderContainer.SetLayoutBounds((IView)TrackHighlight, new Rect(trackHighlightBounds.X, trackVerticalPosition, trackHighlightBounds.Width+_thumbSize, trackSize));
            SliderContainer.SetLayoutBounds((IView)Track, new Rect(_thumbSize / 4, trackVerticalPosition, _trackWidth + 1.25*_thumbSize , trackSize));
            SliderContainer.SetLayoutBounds((IView)LeftThumbIcon, new Rect(0, lowerThumbVerticalPosition, _thumbSize, _thumbSize));
            SliderContainer.SetLayoutBounds((IView)RightThumbIcon, new Rect(0, upperThumbVerticalPosition, _thumbSize, _thumbSize));


            if (ShowSteps)
            {
                SliderContainer.SetLayoutBounds((IView)StepContainer, new Rect(0, trackVerticalPosition + _stepLabelSpacing, -1, -1));
                SliderContainer.SetLayoutBounds((IView)LastStepContainer, new Rect(_trackWidth+_thumbSize, trackVerticalPosition + _stepLabelSpacing, -1, -1));
            }
            if (ShowToolTip)
            {
                SliderContainer.SetLayoutBounds((IView)UpperValueHolder, new Rect(0, -4, -1, -1));
                SliderContainer.SetLayoutBounds((IView)LowerValueHolder, new Rect(0, -4, -1, -1));
            }
            SetValueLabelBinding(LowerValueLabel, LowerValueProperty);
            SetValueLabelBinding(UpperValueLabel, UpperValueProperty);
            SetValueLabelBinding(LeftLabel, LeftTextProperty);
            SetValueLabelBinding(RightLabel, RightTextProperty);

            OnLowerUpperValuePropertyChanged();

            Track.BatchCommit();
            TrackHighlight.BatchCommit();
            LeftThumbIcon.BatchCommit();
            RightThumbIcon.BatchCommit();
            LowerValueLabel.BatchCommit();
            UpperValueLabel.BatchCommit();
            UpperValueHolder.BatchCommit();
            LowerValueBorder.BatchCommit();
            UpperValueBorder.BatchCommit();
            LowerValueHolder.BatchCommit();
            BuildStepper();
            StepContainer.BatchCommit();
            LastStepContainer.BatchCommit();
            SliderContainer.BatchCommit();
		}

		protected override void OnViewSizeChanged(object? sender, System.EventArgs e)
		{
			var maxHeight = Max(LowerValueLabel.Height, UpperValueLabel.Height);
			if ((sender == LowerValueLabel || sender == UpperValueLabel) && _labelMaxHeight == maxHeight)
			{
				MainThread.BeginInvokeOnMainThread(OnValueLabelTranslationChanged);
				return;
			}

			_labelMaxHeight = maxHeight;
			OnLayoutPropertyChanged();
		}

		protected override void OnPanStarted(View view)
		{
			_thumbPositionMap[view] = view.TranslationX;
		}

		protected override void OnPanRunning(View view, double value)
			=> UpdateValue(view, value + GetPanShiftValue(view));

		protected override void OnPanCompleted(View view)
		{
			_thumbPositionMap[view] = view.TranslationX;
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

        protected override void OnLeftIconSourceChanged()
        {
            LeftIcon.Source = LeftIconSource;
            if (LeftIconSource != null)
            {
                LeftIcon.WidthRequest = _iconSize;
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
                RightIcon.WidthRequest = _iconSize;
                RightIcon.IsVisible = true;
            }
            else
            {
                RightIcon.WidthRequest = 0;
                RightIcon.IsVisible = false;
            }
        }

        protected override void OnShowToolTipPropertyChanged()
        {
            SliderContainer.Children.Remove(LowerValueHolder);
            SliderContainer.Children.Remove(UpperValueHolder);
            if (ShowToolTip)
            {
                SliderContainer.Children.Add(LowerValueHolder);
                SliderContainer.Children.Add(UpperValueHolder);
            }
            OnTitleTextPropertyChanged(SliderTitle.Text);
        }

        protected override void OnTitleTextPropertyChanged(string newValue)
        {
            SliderTitle.Text = newValue;
            SliderTitle.Margin = (ShowToolTip) ? new Thickness(0, 0, 0, 10) : new Thickness(0, 10, 0, 0);
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
            RefreshThumbColor(LeftThumbIcon);
            RefreshThumbColor(RightThumbIcon);
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
            if (LowerValueLabel != null) LowerValueLabel.BackgroundColor = newValue;
            if (UpperValueLabel != null) UpperValueLabel.BackgroundColor = newValue;
        }
        #endregion
    }
}
