using DemoApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Collection;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Views;

public partial class TMRadioButtonPage : ContentPage
{
    private RadioButtonCollection<string> _radioButtons;

    public RadioButtonCollection<string> RadioButtons
    {
        get
        {
            return _radioButtons;
        }
        set
        {
            _radioButtons = value;
            OnPropertyChanged(nameof(RadioButtons));
        }
    }
	public TMRadioButtonPage()
	{
		InitializeComponent();
        BindingContext = this;
        RadioButtons = new RadioButtonCollection<string>() { "Bird" };
	}

	private void OnDisableToggled(object sender, ToggledEventArgs e)
	{
        RadioGroup.IsEnabled = !e.Value;
    }

	/// <summary>
	/// This method is called when the user taps on the radio button group for size selection.
	/// </summary>
	private void OnSelectedRadioButtonChanged(object sender, TMRadioButtonEventArgs e)
	{
        if (e.RadioButtonIndex == 0)
        {
            RadioGroup.Size = CheckboxSize.Default;
        }
        else
        {
            RadioGroup.Size = CheckboxSize.Large;
        }
    }

	private void OnOrientationOptionChanged(object sender, TMRadioButtonEventArgs e)
	{
        if (e.RadioButtonIndex == 0)
        {
            RadioGroup.Orientation = StackOrientation.Vertical;
        }
        else
        {
            RadioGroup.Orientation = StackOrientation.Horizontal;
        }
    }
    public ICommand AddRadioButtonCommand => new Command<string>(AddRadioButton);
    private void AddRadioButton(string text)
    {
        RadioButtons.Add(text);
    }  
}
