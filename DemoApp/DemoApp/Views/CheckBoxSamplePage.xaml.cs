using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp;

public partial class CheckBoxSamplePage : ContentPage
{
    private string _selectedSize = "Default";
    private IList<TMCheckBox> childCheckboxes;
    private bool updatingCheckboxes = false;

    public bool ParentChecked { get; set; }
    public CheckBoxSamplePage()
    {
        InitializeComponent();
        childCheckboxes = new List<TMCheckBox>
            {
                ChildCheckBox1,
                ChildCheckBox2,
                ChildCheckBox3
            };
    }
    private void IndeterminateToggled(object sender, ToggledEventArgs e)
    {
        CheckBox.IsIndeterminate = !CheckBox.IsIndeterminate;
    }
    private void DisableToggled(object sender, ToggledEventArgs e)
    {
        CheckBox.IsDisabled = !CheckBox.IsDisabled;
    }

    private void SizeSet(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _selectedSize = radioButton.Value.ToString();
            CheckBox.Size = (CheckboxSize)Enum.Parse(typeof(CheckboxSize), _selectedSize);
        }
    }

    private void UpdateParentCheckboxState()
    {
        if (updatingCheckboxes)
            return;

        int checkedCount = 0;
        int uncheckedCount = 0;

        foreach (var childCheckbox in childCheckboxes)
        {
            if (childCheckbox.IsChecked)
                checkedCount++;
            else
                uncheckedCount++;
        }

        updatingCheckboxes = true;

        if (checkedCount == childCheckboxes.Count)
        {
            ParentCheckBox.IsIndeterminate = false;
            ParentCheckBox.IsChecked = true;
        }
        else if (uncheckedCount == childCheckboxes.Count)
        {

            ParentCheckBox.IsIndeterminate = false;
            ParentCheckBox.IsChecked = false;


        }
        else
        {
            ParentCheckBox.IsChecked = false;
            ParentCheckBox.IsIndeterminate = true;
        }

        updatingCheckboxes = false;
    }

    private void ChildCheckboxCheckedChanged(object sender, EventArgs e)
    {
        if (updatingCheckboxes)
            return;

        var childCheckbox = sender as TMCheckBox;
        UpdateParentCheckboxState();


    }

    private void ParentCheckBoxCheckedChanged(object sender, EventArgs e)
    {
        if (updatingCheckboxes)
            return;

        updatingCheckboxes = true;

        foreach (var checkbox in childCheckboxes)
        {
            checkbox.IsChecked = ParentCheckBox.IsChecked;
        }

        updatingCheckboxes = false;
    }
}
