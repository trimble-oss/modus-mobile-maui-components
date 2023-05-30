using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp;

public partial class TMCheckBoxPage : ContentPage
{
    private string _selectedSize = "Default";
    private IList<TMCheckBox> childCheckboxes;
    public TMCheckBoxPage()
	{
		InitializeComponent();
        childCheckboxes = new List<TMCheckBox>
            {
                ChildCheckBox1,
                ChildCheckBox2,
                ChildCheckBox3
            };
    }
    private void Indeterminate_Toggled(object sender, ToggledEventArgs e)
    {
        CheckBox.IsIndeterminate = !CheckBox.IsIndeterminate;
    }
    private void Disable_Toggled(object sender, ToggledEventArgs e)
    {
        CheckBox.IsDisabled = !ParentCheckBox.IsDisabled;
    }
  
    private void Size_Changed(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _selectedSize = radioButton.Value.ToString();
            CheckBox.Size = (CheckboxSize)Enum.Parse(typeof(CheckboxSize), _selectedSize);
        }
    }

    private void UpdateParentCheckboxState()
    {
        bool anyChildUnchecked = false;

        foreach (var childCheckbox in childCheckboxes)
        {
            if (!childCheckbox.IsChecked)
            {
                anyChildUnchecked = true;
                break;
            }
        }

        if (anyChildUnchecked)
        {
            ParentCheckBox.IsChecked = false;
            ParentCheckBox.IsIndeterminate = true;
        }
        else
        {
            ParentCheckBox.IsChecked = true;
            ParentCheckBox.IsIndeterminate = false;
        }
    }

    private void ChildCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        

        UpdateParentCheckboxState();
    }
}