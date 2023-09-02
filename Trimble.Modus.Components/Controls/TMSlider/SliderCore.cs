using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Controls.Slider;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components.Controls
{
    public abstract class SliderCore : Grid
    {
        #region Private fields
        const double _enabledOpacity = 1;
        const double _disabledOpacity = .5;
        const int _iconHorizontalPadding = 4;
        protected const int _thumbZindex = 3;
        protected const int _stepLabelSpacing = 20;
        protected const int _iconSize = 20;
        Microsoft.Maui.Graphics.Size _allocatedSize;
        protected readonly Dictionary<View, double> _thumbPositionMap = new Dictionary<View, double>();
        protected double _labelMaxHeight;
        protected int _dragCount;
        protected double _thumbSize;
        #endregion

        #region Bindable Property
        public static BindableProperty MinimumValueProperty = BindableProperty.Create(nameof(MinimumValue), typeof(double), typeof(SliderCore), .0,  propertyChanged: OnMinimumMaximumValuePropertyChanged);
        public static BindableProperty MaximumValueProperty = BindableProperty.Create(nameof(MaximumValue), typeof(double), typeof(SliderCore), 1.0, propertyChanged: OnMinimumMaximumValuePropertyChanged);
        public static BindableProperty StepValueProperty = BindableProperty.Create(nameof(StepValue), typeof(double), typeof(SliderCore), (double)1, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnMinimumMaximumValuePropertyChanged);
        public static BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(SliderSize), typeof(SliderCore), SliderSize.Medium, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnLayoutPropertyChanged);
        public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(SliderCore), null, propertyChanged: OnTitleTextPropertyChanged);
        public static BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(SliderCore), null);
        public static BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(SliderCore), null);
        public static BindableProperty LeftIconProperty = BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(SliderCore), null, propertyChanged: OnLeftIconSourceChanged);
        public static BindableProperty RightIconProperty = BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(SliderCore), null, propertyChanged: OnRightIconSourceChanged);
        public static BindableProperty ShowStepsProperty = BindableProperty.Create(nameof(ShowSteps), typeof(Boolean), typeof(SliderCore), false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnShowStepsPropertyChanged);
        public static BindableProperty ShowToolTipProperty= BindableProperty.Create(nameof(ShowToolTip), typeof(Boolean), typeof(SliderCore), false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnShowToolTipPropertyChanged);
        #endregion

        #region Property change methods
        static void OnTitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnTitleTextPropertyChanged((string) newValue);
        static void OnShowStepsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnShowStepsPropertyChanged();
        static void OnShowToolTipPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnShowToolTipPropertyChanged();
        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnLayoutPropertyChanged();
        static void OnMinimumMaximumValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnMinimumMaximumValuePropertyChanged();
        static void OnLeftIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnLeftIconSourceChanged();
        static void OnRightIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SliderCore)bindable).OnRightIconSourceChanged();
        #endregion

        #region Public Property
        const int _outerElementTopPadding = 30;

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
        #endregion

        #region UI Elements
        internal Border Track { get; } = SliderHelper.CreateBorderElement<Border>();
        internal Border TrackHighlight { get; } = SliderHelper.CreateBorderElement<Border>();
        internal StackLayout StepContainer { get; } = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Horizontal, Spacing = 0 };
        internal StackLayout LastStepContainer { get; } = SliderHelper.CreateStepLabelContainer();
        internal BoxView LastStepLine { get; } = SliderHelper.CreateStepLine();
        internal Label LastLabel { get; } = SliderHelper.CreateStepLabel();
        internal Label SliderTitle = new Label { FontSize = 12, TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray8) };
        internal Label LeftLabel= new Label { FontSize = 12, Margin = new Thickness(0, (DeviceInfo.Idiom == DeviceIdiom.Desktop )?_outerElementTopPadding : 0, 0, 0), TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray), HorizontalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.Center};
        internal Image LeftIcon = new Image { HeightRequest = 20, Margin = new Thickness(_iconHorizontalPadding, (DeviceInfo.Idiom == DeviceIdiom.Desktop) ? _outerElementTopPadding : 0, _iconHorizontalPadding, 0),};
        internal Image RightIcon = new Image { HeightRequest = 20, Margin = new Thickness(_iconHorizontalPadding, (DeviceInfo.Idiom == DeviceIdiom.Desktop) ? _outerElementTopPadding : 0, _iconHorizontalPadding, 0), };
        internal Label RightLabel= new Label { FontSize = 12, Margin = new Thickness(0, (DeviceInfo.Idiom == DeviceIdiom.Desktop) ? _outerElementTopPadding : 0, 0, 0), TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray), HorizontalOptions = LayoutOptions.End, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center};
        internal AbsoluteLayout SliderContainer = new AbsoluteLayout();
        internal StackLayout SliderHolderLayout = new StackLayout();
        #endregion

        #region Constructor
        internal SliderCore()
        {
            RowDefinitions = new RowDefinitionCollection(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }, new RowDefinition());
            Children.Add(SliderHolderLayout);
            Grid.SetRow(SliderHolderLayout, 1);
        }
        #endregion

        #region Protected methods
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width == _allocatedSize.Width && height == _allocatedSize.Height)
                return;

            _allocatedSize = new Microsoft.Maui.Graphics.Size(width, height);
            OnLayoutPropertyChanged();
        }
        protected void OnIsEnabledChanged()
        {
            foreach (View child in Children)
            {
                child.Opacity = IsEnabled
                                ? _enabledOpacity
                                : _disabledOpacity;
            }
            foreach (View child in SliderContainer)
            {
                if (child.ZIndex != _thumbZindex)
                {
                    child.Opacity = IsEnabled
                                    ? _enabledOpacity
                                    : _disabledOpacity;
                }
                else if (child is Border)
                {
                    (child as Border).Stroke = IsEnabled ? ResourcesDictionary.ColorsDictionary(ColorsConstants.BlueLightColor) : ResourcesDictionary.ColorsDictionary(ColorsConstants.SliderThumbBorderDisabledColor);
                }
            }
        }
        internal void SetThumbStyle(Border border, double thumbStrokeThickness, double thumbSize)
        {
            border.StrokeThickness = thumbStrokeThickness;
            border.Stroke = IsEnabled ? ResourcesDictionary.ColorsDictionary(ColorsConstants.BlueLightColor) : ResourcesDictionary.ColorsDictionary(ColorsConstants.SliderThumbBorderDisabledColor);
            border.Margin = new Thickness(0);
            border.BackgroundColor = Colors.White;
            border.StrokeShape = new Ellipse() { WidthRequest = thumbSize, HeightRequest = thumbSize };
            border.ZIndex = _thumbZindex;
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
        protected void BuildStepper()
        {
            StepContainer.Children.Clear();

            StepContainer.WidthRequest = SliderContainer.Width - _thumbSize;
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
        protected double GetPanShiftValue(View view)
            => Device.RuntimePlatform == Device.Android
                ? view.TranslationX
                : _thumbPositionMap[view];
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
    }
}
