using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using Trimble.Modus.Components.Controls;
using Trimble.Modus.Components.Controls.Slider;
using Trimble.Modus.Components.Enums;
using static System.Math;

namespace Trimble.Modus.Components
{
	public class RangeSlider : SliderControl
    {
		public event EventHandler? ValueChanged;

		
		public event EventHandler? LowerValueChanged;

		
		public event EventHandler? UpperValueChanged;

		
		public event EventHandler? DragStarted;

		
		public event EventHandler? LowerDragStarted;

		
		public event EventHandler? UpperDragStarted;

		
		public event EventHandler? DragCompleted;

		
		public event EventHandler? LowerDragCompleted;

		
		public event EventHandler? UpperDragCompleted;

		public static BindableProperty LowerValueProperty
			= BindableProperty.Create(nameof(LowerValue), typeof(double), typeof(RangeSlider), .0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);

		public static BindableProperty UpperValueProperty
			= BindableProperty.Create(nameof(UpperValue), typeof(double), typeof(RangeSlider), 1.0, BindingMode.TwoWay, propertyChanged: OnLowerUpperValuePropertyChanged, coerceValue: CoerceValue);

		public static BindableProperty LowerValueLabelStyleProperty
			= BindableProperty.Create(nameof(LowerValueLabelStyle), typeof(Style), typeof(RangeSlider), propertyChanged: OnLayoutPropertyChanged);

		public static BindableProperty UpperValueLabelStyleProperty
			= BindableProperty.Create(nameof(UpperValueLabelStyle), typeof(Style), typeof(RangeSlider), propertyChanged: OnLayoutPropertyChanged);

		readonly PanGestureRecognizer lowerThumbGestureRecognizer = new PanGestureRecognizer();

		readonly PanGestureRecognizer upperThumbGestureRecognizer = new PanGestureRecognizer();

		double lowerTranslation;

		double upperTranslation;

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

		public Style LowerValueLabelStyle
		{
			get => (Style)GetValue(LowerValueLabelStyleProperty);
			set => SetValue(LowerValueLabelStyleProperty, value);
		}

		public Style UpperValueLabelStyle
		{
			get => (Style)GetValue(UpperValueLabelStyleProperty);
			set => SetValue(UpperValueLabelStyleProperty, value);
		}

		Label LowerValueLabel { get; } = SliderHelper.CreateLabelElement();

		Label UpperValueLabel { get; } = SliderHelper.CreateLabelElement();

        Border RightThumbIcon = SliderHelper.CreateBorderElement<Border>();
        Border LeftThumbIcon = SliderHelper.CreateBorderElement<Border>();

		double TrackWidth => Width - RightThumbIcon.Width - LeftThumbIcon.Width;

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
        public RangeSlider()
        {
            Children.Add(Track);
            Children.Add(TrackHighlight);
            Children.Add(LeftThumbIcon);
            Children.Add(RightThumbIcon);
            Children.Add(LowerValueLabel);
            Children.Add(UpperValueLabel);
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
            //BuildStepper();
            OnLayoutPropertyChanged();
        }

		static void OnLowerUpperValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((RangeSlider)bindable).OnLowerUpperValuePropertyChanged();

        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((RangeSlider)bindable).OnLayoutPropertyChanged();

        static object CoerceValue(BindableObject bindable, object value)
        {
            var slider = (RangeSlider)bindable;
            return SliderHelper.CoerceValue((double)value, slider.StepValue, slider.MinimumValue, slider.MaximumValue);
        }

        protected override void OnMinimumMaximumValuePropertyChanged()
		{
			LowerValue = SliderHelper.CoerceValue(LowerValue, StepValue, MinimumValue, MaximumValue);
			UpperValue = SliderHelper.CoerceValue(UpperValue, StepValue, MinimumValue, MaximumValue);
            OnLowerUpperValuePropertyChanged();
        }

		void OnLowerUpperValuePropertyChanged()
		{
			var rangeValue = MaximumValue - MinimumValue;
			var trackWidth = TrackWidth;

			lowerTranslation = (LowerValue - MinimumValue) / rangeValue * trackWidth;
			upperTranslation = ((UpperValue - MinimumValue) / rangeValue * trackWidth) + LeftThumbIcon.Width;

            LeftThumbIcon.TranslationX = lowerTranslation;
			RightThumbIcon.TranslationX = upperTranslation;
			OnValueLabelTranslationChanged();

			var bounds = GetLayoutBounds((IView)TrackHighlight);
			//this.SetLayoutBounds(TrackHighlight, new Rect(lowerTranslation, bounds.Y, upperTranslation - lowerTranslation + RightThumbIcon.Width, bounds.Height));
			this.SetLayoutBounds(TrackHighlight, new Rect(lowerTranslation, bounds.Y, upperTranslation - lowerTranslation + thumbSize, bounds.Height));
		}

		protected override void OnValueLabelTranslationChanged()
		{
			var labelSpacing = 5;
			var lowerLabelTranslation = lowerTranslation + ((LeftThumbIcon.Width - LowerValueLabel.Width) / 2);
			var upperLabelTranslation = upperTranslation + ((RightThumbIcon.Width - UpperValueLabel.Width) / 2);
			LowerValueLabel.TranslationX = Min(Max(lowerLabelTranslation, 0), Width - LowerValueLabel.Width - UpperValueLabel.Width - labelSpacing);
			UpperValueLabel.TranslationX = Min(Max(upperLabelTranslation, LowerValueLabel.TranslationX + LowerValueLabel.Width + labelSpacing), Width - UpperValueLabel.Width);
		}

		protected override void OnLayoutPropertyChanged()
		{
			BatchBegin();
			Track.BatchBegin();
			TrackHighlight.BatchBegin();
			LeftThumbIcon.BatchBegin();
			RightThumbIcon.BatchBegin();
			LowerValueLabel.BatchBegin();
			UpperValueLabel.BatchBegin();
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
			if (labelWithSpacingHeight > 0)
				labelWithSpacingHeight += ValueLabelSpacing;

			var trackThumbHeight = Max(Max(thumbSize, thumbSize), trackSize);
			var trackVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - trackSize) / 2);
			var lowerThumbVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - thumbSize) / 2);
			var upperThumbVerticalPosition = labelWithSpacingHeight + ((trackThumbHeight - thumbSize) / 2);

			this.HeightRequest = labelWithSpacingHeight + trackThumbHeight;

			var trackHighlightBounds = GetLayoutBounds((IView)TrackHighlight);
			SetLayoutBounds((IView)TrackHighlight, new Rect(trackHighlightBounds.X, trackVerticalPosition, trackHighlightBounds.Width, trackSize));
			SetLayoutBounds((IView)Track, new Rect(0, trackVerticalPosition, Width, trackSize));
			SetLayoutBounds((IView)LeftThumbIcon, new Rect(0, lowerThumbVerticalPosition, thumbSize, thumbSize));
			SetLayoutBounds((IView)RightThumbIcon, new Rect(0, upperThumbVerticalPosition, thumbSize, thumbSize));
			SetLayoutBounds((IView)LowerValueLabel, new Rect(0, 0, -1, -1));
			SetLayoutBounds((IView)UpperValueLabel, new Rect(0, 0, -1, -1));
            if (ShowSteps)
            {
                SetLayoutBounds((IView)StepContainer, new Rect(0, trackVerticalPosition + 20, -1, -1));
                SetLayoutBounds((IView)LastStepContainer, new Rect(TrackWidth, trackVerticalPosition + 20, -1, -1));
            }
            SetValueLabelBinding(LowerValueLabel, LowerValueProperty);
			SetValueLabelBinding(UpperValueLabel, UpperValueProperty);
			LowerValueLabel.Style = LowerValueLabelStyle ?? ValueLabelStyle;
			UpperValueLabel.Style = UpperValueLabelStyle ?? ValueLabelStyle;
			OnLowerUpperValuePropertyChanged();

			Track.BatchCommit();
			TrackHighlight.BatchCommit();
            LeftThumbIcon.BatchCommit();
            RightThumbIcon.BatchCommit();
			LowerValueLabel.BatchCommit();
			UpperValueLabel.BatchCommit();
            BuildStepper();
            StepContainer.BatchCommit();
            LastStepContainer.BatchCommit();
            BatchCommit();
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

		void UpdateValue(View view, double value)
		{
			var rangeValue = MaximumValue - MinimumValue;
			if (view == LeftThumbIcon)
			{
				LowerValue = Min(Max(MinimumValue, (value / TrackWidth * rangeValue) + MinimumValue), UpperValue);
				return;
			}
			UpperValue = Min(Max(LowerValue, ((value - LeftThumbIcon.Width) / TrackWidth * rangeValue) + MinimumValue), MaximumValue);
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
    }
}
