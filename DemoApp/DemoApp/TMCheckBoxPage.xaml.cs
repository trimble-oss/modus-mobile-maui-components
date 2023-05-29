using Trimble.Modus.Components.Enums;

namespace DemoApp;

public partial class TMCheckBoxPage : ContentPage
{
    private string _selectedSize = "Default";
    public TMCheckBoxPage()
	{
		InitializeComponent();
	}
    private void Disable_Toggled(object sender, ToggledEventArgs e)
    {
		CheckBox.IsDisabled = !CheckBox.IsDisabled;
    }
    private void Indeterminate_Toggled(object sender, ToggledEventArgs e)
    {
        CheckBox.IsIndeterminate = !CheckBox.IsIndeterminate;
    }
    private void Size_Changed(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _selectedSize = radioButton.Value.ToString();
            CheckBox.Size = (CheckboxSize)Enum.Parse(typeof(CheckboxSize), _selectedSize);
        }
    }
}