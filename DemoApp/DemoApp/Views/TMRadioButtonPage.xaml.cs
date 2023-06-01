using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Views;

public partial class TMRadioButtonPage : ContentPage
{
	public TMRadioButtonPage()
	{
		InitializeComponent();
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
		if (e.RadioButtonIndex  == 0){
			RadioGroup.Size = CheckboxSize.Default;
		}
		else{
			RadioGroup.Size = CheckboxSize.Large;
		}
	}
}