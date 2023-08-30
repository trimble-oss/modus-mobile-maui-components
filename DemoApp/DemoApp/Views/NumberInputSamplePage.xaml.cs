namespace DemoApp.Views;

public partial class NumberInputSamplePage : ContentPage
{
    public NumberInputSamplePage()
    {
        InitializeComponent();
        StepValueInput.TextChanged += OnStepValueChanged;
        NumberInputSample.ValueChanged += OnValueChanged;
    }

    private void OnStepValueChanged(object sender, TextChangedEventArgs e)
    {
        if (double.TryParse(e.NewTextValue, out double value))
        {
            NumberInputSample.Step = value;
        }
    }

    private void OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        ValueChange.Text = $"Value Changed: Old Value: {e.OldValue} New Value: {e.NewValue}";
    }

    private void ShowValueClicked(object sender, EventArgs e)
    {
        DisplayAlert("Value", $"Value: {NumberInputSample.Value}", "OK");
    }

    private void SetMinValueClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(MinValueInput.Text))
            {
                DisplayAlert("Error", "Value cannot be empty", "OK");
            }
            else
            {
                NumberInputSample.MinValue = double.Parse(MinValueInput.Text);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void SetMaxValueClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(MaxValueInput.Text))
            {
                DisplayAlert("Error", "Value cannot be empty", "OK");
            }
            else
            {
                NumberInputSample.MaxValue = double.Parse(MaxValueInput.Text);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void SetValueClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(ValueInput.Text))
            {
                DisplayAlert("Error", "Value cannot be empty", "OK");
            }
            else
            {
                NumberInputSample.Value = double.Parse(ValueInput.Text);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }
    private void SetStepValueClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(StepValueInput.Text))
            {
                DisplayAlert("Error", "Value cannot be empty", "OK");
            }
            else
            {
                NumberInputSample.Step = double.Parse(StepValueInput.Text);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

}
