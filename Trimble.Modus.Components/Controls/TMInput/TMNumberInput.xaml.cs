using System.Windows.Input;

namespace Trimble.Modus.Components;

public partial class TMNumberInput : ContentView
{
    #region Private Fields
    private ICommand _minusCommand;
    private ICommand _plusCommand;
    #endregion

    #region Bindable properties
    public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(double), typeof(TMNumberInput), double.MaxValue, propertyChanged: OnMaxValuePropertyChanged);
    public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(double), typeof(TMNumberInput), double.MinValue, propertyChanged: OnMinValuePropertyChanged);
    public static readonly BindableProperty StepProperty = BindableProperty.Create(nameof(Step), typeof(double), typeof(TMNumberInput), 1d);
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(TMNumberInput), double.NaN, propertyChanged: OnValueChanged);
    public static new readonly BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMNumberInput), true, propertyChanged: OnEnabledOrReadOnlyPropertyChanged);
    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(TMNumberInput), false, propertyChanged: OnEnabledOrReadOnlyPropertyChanged);
    public static readonly BindableProperty ValueChangeCommandProperty = BindableProperty.Create(nameof(ValueChangeCommand), typeof(ICommand), typeof(TMNumberInput), null);
    public static readonly BindableProperty ValueChangeCommandParameterProperty = BindableProperty.Create(nameof(ValueChangeCommandParameter), typeof(object), typeof(TMNumberInput), null, BindingMode.OneWay, null);
    #endregion

    #region Public properties
    /// <summary>
    /// Event raised when the value is changed
    /// </summary>
    public event EventHandler<ValueChangedEventArgs> ValueChanged;
    /// <summary>
    /// Gets or sets the value change command
    /// </summary>
    public ICommand ValueChangeCommand
    {
        get => (ICommand)GetValue(ValueChangeCommandProperty);
        set => SetValue(ValueChangeCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the value change command parameter
    /// </summary>
    public object ValueChangeCommandParameter
    {
        get => GetValue(ValueChangeCommandParameterProperty);
        set => SetValue(ValueChangeCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or Sets Max value for the input
    /// </summary>
    public double MaxValue
    {
        get { return (double)GetValue(MaxValueProperty); }
        set { SetValue(MaxValueProperty, value); }
    }
    /// <summary>
    /// Gets or Sets Min value for the input
    /// </summary>
    public double MinValue
    {
        get { return (double)GetValue(MinValueProperty); }
        set { SetValue(MinValueProperty, value); }
    }
    /// <summary>
    /// Gets or Sets value for the input
    /// </summary>
    public double Value
    {
        get { return (double)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }
    /// <summary>
    /// Gets or Sets Step value for the input
    /// </summary>
    public double Step
    {
        get { return (double)GetValue(StepProperty); }
        set { SetValue(StepProperty, value); }
    }
    /// <summary>
    /// Gets or Sets IsEnabled value for the input
    /// </summary>
    public new bool IsEnabled
    {
        get { return (bool)GetValue(IsEnabledProperty); }
        set { SetValue(IsEnabledProperty, value); }
    }

    /// <summary>
    /// Gets or Sets IsReadOnly value for the input
    /// </summary>
    public bool IsReadOnly
    {
        get { return (bool)GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="TMNumberInput"/> class.
    /// </summary>
    public TMNumberInput()
    {
        InitializeComponent();
        BindingContext = this;
        TMInputControl.SetCenterTextAlignment();
        TMInputControl.RightIconCommand = new Command(PlusCommand.Execute);
        TMInputControl.LeftIconCommand = new Command(MinusCommand.Execute);
    }

    #endregion
    #region Private methods
    /// <summary>
    /// Triggered when IsEnabled is changed
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <exception cref="NotImplementedException"></exception>
    private static void OnEnabledOrReadOnlyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMNumberInput numberInput)
        {
            numberInput.ToggleLeftAndRightIcons(numberInput.TMInputControl, numberInput.IsEnabled && !numberInput.IsReadOnly);
        }
    }
    /// <summary>
    /// Called when the <see cref="MinValue"/> property is changed.
    /// </summary>
    /// <param name="bindable">The object that the property was changed on.</param>
    /// <param name="oldValue">The old value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    /// <exception cref="ArgumentException">Thrown when the new minimum value is greater than the maximum value.</exception>
    private static void OnMinValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TMNumberInput numberInput = bindable as TMNumberInput;
        double min = (double)newValue;
        double max = numberInput.MaxValue;
        if (min > max)
        {
            throw new ArgumentException("Min value cannot be greater than max value");
        }
        else if (numberInput.Value < min)
        {
            numberInput.Value = min;
        }
        numberInput.ToggleLeftAndRightIcons(numberInput.TMInputControl, numberInput.IsEnabled && !numberInput.IsReadOnly);
    }
    /// <summary>
    /// Called when the <see cref="MaxValue"/> property is changed.
    /// </summary>
    /// <param name="bindable">The object that the property was changed on.</param>
    /// <param name="oldValue">The old value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    /// <exception cref="ArgumentException">Thrown when the new minimum value is greater than the maximum value.</exception>
    private static void OnMaxValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TMNumberInput numberInput = bindable as TMNumberInput;
        double max = (double)newValue;
        double min = numberInput.MinValue;
        if (min > max)
        {
            throw new ArgumentException("Min value cannot be greater than max value");
        }
        else if (numberInput.Value > max)
        {
            numberInput.Value = max;
        }
        numberInput.ToggleLeftAndRightIcons(numberInput.TMInputControl, numberInput.IsEnabled && !numberInput.IsReadOnly);
    }

    /// <summary>
    /// Called when the <see cref="Value"/> property is changed.
    /// </summary>
    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMNumberInput numberInput)
        {
            numberInput.TMInputControl.Text = newValue.ToString();
        }
    }

    /// <summary>
    /// Method to validate that the input is a number and that it is within the min and max values
    /// </summary>
    private async void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string newText = e.NewTextValue;
        string oldText = e.OldTextValue;
        bool validOldValue = double.TryParse(oldText, out double oldNumber);
        TMInput tMInput = sender as TMInput;

        if (string.IsNullOrEmpty(newText) || !double.TryParse(newText, out double number))
        {
            if (validOldValue)
            {
                ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldNumber, double.NaN));
                ValueChangeCommand?.Execute(ValueChangeCommandParameter);
            }
            return;
        }

        tMInput.TextChanged -= OnTextChanged;

        // FIXME: This is a hack to delay execution to avoid re-triggering the TextChanged event
        await Task.Delay(10);

        if (!validOldValue)
        {
            oldNumber = double.NaN;
        }

        double clampedValue = Math.Clamp(number, MinValue, MaxValue);

        if (clampedValue != number)
        {
            UpdateValue(number, clampedValue);
        }

        if (oldNumber != clampedValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldNumber, clampedValue));
            ValueChangeCommand?.Execute(ValueChangeCommandParameter);
            Value = clampedValue;
        }

        ToggleLeftAndRightIcons(tMInput, IsEnabled && !IsReadOnly);
        tMInput.TextChanged += OnTextChanged;
    }

    /// <summary>
    /// Command for left minus icon
    /// </summary>
    private ICommand MinusCommand
    {
        get
        {
            return _minusCommand ??= new Command(OnMinusCommandClicked);
        }
    }

    /// <summary>
    /// Command for right plus icon
    /// </summary>
    private ICommand PlusCommand
    {
        get
        {
            return _plusCommand ??= new Command(OnPlusCommandClicked);
        }
    }

    private void OnMinusCommandClicked()
    {

        if (double.TryParse(TMInputControl.Text, out double number))
        {
            if (number <= MinValue)
            {
                return;
            }
            double nearestValue;
            double numberRemainingValue = number % Step;
            if (numberRemainingValue == 0)
            {
                nearestValue = number - Step;
            }
            else
            {
                if (number < 0)
                {
                    double nearestStepNumber = number - numberRemainingValue;
                    nearestValue = nearestStepNumber - Step;
                }
                else
                {
                    nearestValue = number - numberRemainingValue;
                }
            }

            UpdateValue(number, nearestValue);
        }
    }
    private void OnPlusCommandClicked()
    {
        if (double.TryParse(TMInputControl.Text, out double number))
        {
            if (number >= MaxValue)
            {
                return;
            }
            double nearestValue;
           
            if (number != 0)
            {
                double numberRemainingValue = number % Step;
                double previousStepNumber = number - numberRemainingValue;

                if (number < 0 && numberRemainingValue != 0)
                {
                    nearestValue = previousStepNumber;
                }
                else
                {
                    nearestValue = previousStepNumber + Step;
                }
            }
            else
            {
                nearestValue = Step;
            }
            UpdateValue(number, nearestValue);
        }

    }

    /// <summary>
    /// Invoke ValueChanged call and update text
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private void UpdateValue(double oldValue, double newValue)
    {
        ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
        ValueChangeCommand?.Execute(ValueChangeCommandParameter);
        TMInputControl.Text = newValue.ToString();
        Value = newValue;
    }

    /// <summary>
    /// Toggle left and right icons states
    /// </summary>
    /// <param name="tMInput"></param>
    /// <param name="shouldEnable"></param>
    private void ToggleLeftAndRightIcons(TMInput tMInput, bool shouldEnable)
    {
        tMInput.ToggleRightIconState(shouldEnable && Value < MaxValue);
        tMInput.ToggleLeftIconState(shouldEnable && Value > MinValue);
    }
    #endregion

}
