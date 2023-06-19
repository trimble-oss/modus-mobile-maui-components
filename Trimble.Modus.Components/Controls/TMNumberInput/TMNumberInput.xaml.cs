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
    public static new readonly BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMNumberInput), true);
    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(TMNumberInput), false);

    #endregion

    #region Public properties
    public event EventHandler<ValueChangedEventArgs> ValueChanged;

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
        get { return string.IsNullOrEmpty(TMInputControl.Text) ? double.NaN : double.Parse(TMInputControl.Text); }
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

    #region Private methods
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
        numberInput.TMInputControl.ToggleRightIconState(numberInput.Value > min);
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
        numberInput.TMInputControl.ToggleRightIconState(numberInput.Value < max);
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
        TMInput tMInput = sender as TMInput;
        tMInput.TextChanged -= OnTextChanged;
        if (string.IsNullOrEmpty(newText) || newText == "-" || newText == "." || newText == "-." || newText == "," || newText == "-,")
        {
            tMInput.TextChanged += OnTextChanged;
            return;
        }

        // This is a hack since the TextChanged event is fired again when we set the text
        await Task.Delay(10);
        if (double.TryParse(newText, out double number))
        {
            if (number > MaxValue)
            {
                tMInput.Text = MaxValue.ToString();
                ValueChanged?.Invoke(this, new ValueChangedEventArgs(number, MaxValue));
            }
            else if (number < MinValue)
            {
                tMInput.Text = MinValue.ToString();
                ValueChanged?.Invoke(this, new ValueChangedEventArgs(number, MinValue));
            }
        }
        else
        {
            tMInput.Text = oldText;
        }
        tMInput.ToggleRightIconState(IsEnabled && double.Parse(tMInput.Text) < MaxValue);
        tMInput.ToggleLeftIconState(IsEnabled && double.Parse(tMInput.Text) > MinValue);
        tMInput.TextChanged += OnTextChanged;
    }

    /// <summary>
    /// Command for left minus icon
    /// </summary>
    private ICommand MinusCommand
    {
        get
        {
            return _minusCommand ?? (_minusCommand = new Command(() =>
            {
                if (double.TryParse(TMInputControl.Text, out double number))
                {
                    if (number > MinValue)
                    {
                        number -= Step;
                        ValueChanged?.Invoke(this, new ValueChangedEventArgs(number + Step, number));
                        TMInputControl.Text = number.ToString();
                    }
                }
            }));
        }
    }

    /// <summary>
    /// Command for right plus icon
    /// </summary>
    private ICommand PlusCommand
    {
        get
        {
            return _plusCommand ?? (_plusCommand = new Command(() =>
            {
                if (double.TryParse(TMInputControl.Text, out double number))
                {
                    if (number < MaxValue)
                    {
                        number += Step;
                        ValueChanged?.Invoke(this, new ValueChangedEventArgs(number - Step, number));
                        TMInputControl.Text = number.ToString();
                    }
                }
            }));
        }
    }


    #endregion

}
