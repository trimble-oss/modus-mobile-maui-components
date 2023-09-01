using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using Trimble.Modus.Components.Controls;
using Trimble.Modus.Components.Controls.RangeSlider;
using Trimble.Modus.Components.Controls.Slider;
using Trimble.Modus.Components.Enums;
using static System.Math;

namespace Trimble.Modus.Components
{
	public class RangeSlider : SliderCore
    {
        #region Event Handlers
        public event EventHandler? ValueChanged;

		
		public event EventHandler? LowerValueChanged;

		
		public event EventHandler? UpperValueChanged;

		
		public event EventHandler? DragStarted;

		
		public event EventHandler? LowerDragStarted;

		
		public event EventHandler? UpperDragStarted;

		
		public event EventHandler? DragCompleted;

		
		public event EventHandler? LowerDragCompleted;

		
		public event EventHandler? UpperDragCompleted;
        #endregion

        #region Bindable Property
        public static BindableProperty LowerValueProperty
			= BindableProperty.Create(nameof(LowerValue), typeof(double), typeof(RangeSlider), .0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);

		public static BindableProperty UpperValueProperty
			= BindableProperty.Create(nameof(UpperValue), typeof(double), typeof(RangeSlider), 1.0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);
        #endregion

        #region Private fields
        readonly PanGestureRecognizer lowerThumbGestureRecognizer = new PanGestureRecognizer();

		readonly PanGestureRecognizer upperThumbGestureRecognizer = new PanGestureRecognizer();

		double _lowerTranslation;

		double _upperTranslation;
        double _trackWidth => AbsoluteLayout.Width - thumbSize * 2;
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
        #endregion

        #region UI Elements
        StackLayout UpperValueHolder = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 0, Padding = 0 };
        View UpperValueToolTipShape = new ToolTipAnchor() { };
        Border UpperValueBorder = new Border { StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(4) }, StrokeThickness = 0};
        StackLayout LowerValueHolder = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 0, Padding = 0 };
        Border LowerValueBorder = new Border { StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(4) }, StrokeThickness = 0 };
        View LowerValueToolTipShape = new ToolTipAnchor() { };
        Label LowerValueLabel { get; } = SliderHelper.CreateLabelElement();
		Label UpperValueLabel { get; } = SliderHelper.CreateLabelElement();
        Border RightThumbIcon = SliderHelper.CreateBorderElement<Border>();
        Border LeftThumbIcon = SliderHelper.CreateBorderElement<Border>();
        #endregion

        #region Constructors
        public RangeSlider()
        {
            sliderHolderLayout.Orientation = StackOrientation.Horizontal;
            AbsoluteLayout.Children.Add(Track);
            AbsoluteLayout.Children.Add(TrackHighlight);
            AbsoluteLayout.Children.Add(LeftThumbIcon);
            AbsoluteLayout.Children.Add(RightThumbIcon);

            UpperValueToolTipShape.VerticalOptions = LayoutOptions.Start;
            UpperValueToolTipShape.TranslationY = 0;
            UpperValueToolTipShape.RotateTo(180);
            UpperValueLabel.BackgroundColor = Color.FromArgb("#585C65");
            UpperValueBorder.Content = UpperValueLabel;
            UpperValueHolder.Children.Add(UpperValueBorder);
            UpperValueHolder.Children.Add(UpperValueToolTipShape);

            LowerValueToolTipShape.VerticalOptions = LayoutOptions.Start;
            LowerValueToolTipShape.TranslationY = 0;
            LowerValueToolTipShape.RotateTo(180);
            LowerValueLabel.BackgroundColor = Color.FromArgb("#585C65");
            LowerValueBorder.Content = LowerValueLabel;
            LowerValueHolder.Children.Add(LowerValueBorder);
            LowerValueHolder.Children.Add(LowerValueToolTipShape);
            AbsoluteLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            LeftIcon.VerticalOptions = LayoutOptions.End;
            RightIcon.VerticalOptions = LayoutOptions.End;
            sliderHolderLayout.Children.Add(LeftLabel);
            sliderHolderLayout.Children.Add(LeftIcon);
            sliderHolderLayout.Children.Add(AbsoluteLayout);
            sliderHolderLayout.Children.Add(RightIcon);
            sliderHolderLayout.Children.Add(RightLabel);

            RightThumbIcon.ZIndex = 3;
            LeftThumbIcon.ZIndex = 3;

            AddGestureRecognizer(LeftThumbIcon, lowerThumbGestureRecognizer);
            AddGestureRecognizer(RightThumbIcon, upperThumbGestureRecognizer);

            Track.SizeChanged += OnViewSizeChanged;
            LeftThumbIcon.SizeChanged += OnViewSizeChanged;
            RightThumbIcon.SizeChanged += OnViewSizeChanged;
            LowerValueLabel.SizeChanged += OnViewSizeChanged;
            UpperValueLabel.SizeChanged += OnViewSizeChanged;
            OnIsEnabledChanged();
            OnLayoutPropertyChanged();
        }
        #endregion

        #region Private methods
        static void OnLowerUpperValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((RangeSlider)bindable).OnLowerUpperValuePropertyChanged();

        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((RangeSlider)bindable).OnLayoutPropertyChanged();

        static object CoerceValue(BindableObject bindable, object value)
        {
            var slider = (RangeSlider)bindable;
            return SliderHelper.CoerceValue((double)value, slider.StepValue, slider.MinimumValue, slider.MaximumValue);
        }
        void OnLowerUpperValuePropertyChanged()
		{
			var rangeValue = MaximumValue - MinimumValue;
			var trackWidth = _trackWidth+thumbSize;

			_lowerTranslation = (LowerValue - MinimumValue) / rangeValue * trackWidth;
            _upperTranslation = ((UpperValue - MinimumValue) / rangeValue * trackWidth);
            if(UpperValue == LowerValue)
            {
                if(UpperValue == MaximumValue)
                {
                    _lowerTranslation -= thumbSize;
                }
                else
                {
                    _upperTranslation += thumbSize;
                }
            }

            LeftThumbIcon.TranslationX = _lowerTranslation;
			RightThumbIcon.TranslationX = _upperTranslation;
			OnValueLabelTranslationChanged();

            var bounds = AbsoluteLayout.GetLayoutBounds((IView)TrackHighlight);
            AbsoluteLayout.SetLayoutBounds((IView)TrackHighlight, new Rect(_lowerTranslation+thumbSize/2, bounds.Y, _upperTranslation - _lowerTranslation, bounds.Height));
        }
        void UpdateValue(View view, double value)
        {
            var rangeValue = MaximumValue - MinimumValue;
            if (view == LeftThumbIcon)
            {
                LowerValue = Min(Max(MinimumValue, ((value - thumbSize / 2.5) / _trackWidth * rangeValue) + MinimumValue), UpperValue);
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
                    SliderHelper.RaiseEvent(this, LowerValueChanged);
                    SliderHelper.RaiseEvent(this, ValueChanged);
                    break;
                case nameof(UpperValue):
                    SliderHelper.RaiseEvent(this, UpperValueChanged);
                    SliderHelper.RaiseEvent(this, ValueChanged);
                    break;
            }
        }
        protected override void OnValueLabelTranslationChanged()
		{
			var labelSpacing = 5;
			var lowerLabelTranslation = _lowerTranslation + ((LeftThumbIcon.Width - LowerValueLabel.Width) / 2);
			var upperLabelTranslation = _upperTranslation + ((RightThumbIcon.Width - UpperValueLabel.Width) / 2);
			LowerValueBorder.TranslationX = Min(Max(lowerLabelTranslation, 0), AbsoluteLayout.Width - LowerValueLabel.Width - UpperValueLabel.Width - labelSpacing);
            LowerValueToolTipShape.TranslationX = Min(Max(lowerLabelTranslation, 0), AbsoluteLayout.Width - LowerValueLabel.Width - UpperValueLabel.Width - labelSpacing);
			UpperValueBorder.TranslationX = Min(Max(upperLabelTranslation, LowerValueLabel.TranslationX + LowerValueLabel.Width + labelSpacing), AbsoluteLayout.Width - UpperValueLabel.Width);
            UpperValueToolTipShape.TranslationX = Min(Max(upperLabelTranslation, LowerValueLabel.TranslationX + LowerValueLabel.Width + labelSpacing), AbsoluteLayout.Width - UpperValueLabel.Width);
		}

		protected override void OnLayoutPropertyChanged()
		{
            AbsoluteLayout.BatchBegin();
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

            Track.BackgroundColor = Color.FromArgb("#FFA3A6B1");
            Track.StrokeThickness = 0;
            TrackHighlight.BackgroundColor = Color.FromArgb("#FF0063A3");
            TrackHighlight.StrokeThickness = 0;
            UpperValueLabel.TextColor = Colors.White;
            LowerValueLabel.TextColor = Colors.White;

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
            else if ( Size == SliderSize.Large)
            {
                trackSize = 12;
                thumbSize = 32;
                thumbStrokeThickness = 4;
                thumbRadius = 16;
            }
            SetThumbStyle( LeftThumbIcon, thumbStrokeThickness, thumbSize, thumbRadius);
            SetThumbStyle( RightThumbIcon , thumbStrokeThickness, thumbSize, thumbRadius);

            Track.StrokeShape = new Rectangle() { RadiusX = 100, RadiusY = 100};
			TrackHighlight.StrokeShape = new Rectangle() { RadiusX = 100, RadiusY = 100 };

			var labelWithSpacingHeight = Max(Max(LowerValueLabel.Height, UpperValueLabel.Height), 0);

			var trackThumbHeight = Max(Max(thumbSize, thumbSize), trackSize);
			var trackVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - trackSize) / 2);
			var lowerThumbVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - thumbSize) / 2);
			var upperThumbVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - thumbSize) / 2);

            AbsoluteLayout.HeightRequest = labelWithSpacingHeight + trackThumbHeight;

            var trackHighlightBounds = AbsoluteLayout.GetLayoutBounds((IView)TrackHighlight);
            AbsoluteLayout.SetLayoutBounds((IView)TrackHighlight, new Rect(trackHighlightBounds.X, trackVerticalPosition, trackHighlightBounds.Width+thumbSize, trackSize));
            AbsoluteLayout.SetLayoutBounds((IView)Track, new Rect(thumbSize / 4, trackVerticalPosition, _trackWidth + 1.25*thumbSize , trackSize));
            AbsoluteLayout.SetLayoutBounds((IView)LeftThumbIcon, new Rect(0, lowerThumbVerticalPosition, thumbSize, thumbSize));
            AbsoluteLayout.SetLayoutBounds((IView)RightThumbIcon, new Rect(0, upperThumbVerticalPosition, thumbSize, thumbSize));


            if (ShowSteps)
            {
                AbsoluteLayout.SetLayoutBounds((IView)StepContainer, new Rect(0, trackVerticalPosition + 20, -1, -1));
                AbsoluteLayout.SetLayoutBounds((IView)LastStepContainer, new Rect(_trackWidth+thumbSize, trackVerticalPosition + 20, -1, -1));
            }
            if (ShowToolTip)
            {
                AbsoluteLayout.SetLayoutBounds((IView)UpperValueHolder, new Rect(0, -4, -1, -1));
                AbsoluteLayout.SetLayoutBounds((IView)LowerValueHolder, new Rect(0, -4, -1, -1));
            }
            SetValueLabelBinding(LowerValueLabel, LowerValueProperty);
			SetValueLabelBinding(UpperValueLabel, UpperValueProperty);
            SetTitleLabelBinding(LeftLabel, LeftTextProperty);
            SetTitleLabelBinding(RightLabel, RightTextProperty);

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
            AbsoluteLayout.BatchCommit();
		}

		protected override void OnViewSizeChanged(object? sender, System.EventArgs e)
		{
			var maxHeight = Max(LowerValueLabel.Height, UpperValueLabel.Height);
			if ((sender == LowerValueLabel || sender == UpperValueLabel) && labelMaxHeight == maxHeight)
			{
				Device.BeginInvokeOnMainThread(OnValueLabelTranslationChanged);
				return;
			}

			labelMaxHeight = maxHeight;
			OnLayoutPropertyChanged();
		}

		protected override void OnPanStarted(View view)
		{
			thumbPositionMap[view] = view.TranslationX;
			SliderHelper.RaiseEvent(this, view == LeftThumbIcon
				? LowerDragStarted
				: UpperDragStarted);

			if (Interlocked.Increment(ref dragCount) == 1)
				SliderHelper.RaiseEvent(this, DragStarted);
		}

		protected override void OnPanRunning(View view, double value)
			=> UpdateValue(view, value + GetPanShiftValue(view));

		protected override void OnPanCompleted(View view)
		{
			thumbPositionMap[view] = view.TranslationX;
			SliderHelper.RaiseEvent(this, view == LeftThumbIcon
				? LowerDragCompleted
				: UpperDragCompleted);

			if (Interlocked.Decrement(ref dragCount) == 0)
				SliderHelper.RaiseEvent(this, DragCompleted);
		}

        protected override void OnShowStepsPropertyChanged()
        {
            if (ShowSteps)
            {
                AbsoluteLayout.Children.Add(StepContainer);
                LastStepContainer.Children.Add(LastStepLine);
                LastStepContainer.Children.Add(LastLabel);
                AbsoluteLayout.Children.Add(LastStepContainer);
            }
            else
            {
                AbsoluteLayout.Children.Remove(StepContainer);
                AbsoluteLayout.Children.Remove(LastStepContainer);
                LastStepContainer.Children.Remove(LastStepLine);
                LastStepContainer.Children.Remove(LastLabel);
            }
            OnLayoutPropertyChanged();
        }
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

        protected override void OnShowToolTipPropertyChanged()
        {
            if (ShowToolTip)
            {
                AbsoluteLayout.Children.Add(LowerValueHolder);
                AbsoluteLayout.Children.Add(UpperValueHolder);
            }
            else
            {
                AbsoluteLayout.Children.Remove(LowerValueHolder);
                AbsoluteLayout.Children.Remove(UpperValueHolder);
            }
            OnTitleTextPropertyChanged(SliderTitle.Text);
            OnLayoutPropertyChanged();
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
        #endregion
    }
}
