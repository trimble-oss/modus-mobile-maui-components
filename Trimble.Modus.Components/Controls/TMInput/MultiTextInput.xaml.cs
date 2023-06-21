namespace Trimble.Modus.Components;

using Trimble.Modus.Components.Controls.BaseInput;

public partial class MultiTextInput : BaseInput
{
	public MultiTextInput()
	{
		InitializeComponent();
	}
    protected override void RetrieveAndProcessChildElement()
    {
        base.RetrieveAndProcessChildElement();

        // Additional logic specific to TMInput
        InputBorder = (Border)GetTemplateChild("inputBorder");
        HelperIcon = (Image)GetTemplateChild("inputHelperIcon");
        HelperLabel = (Label)GetTemplateChild("inputHelperLabel");
        HelperLayout = (HorizontalStackLayout)GetTemplateChild("inputHelperLayout");
        InputLabel = (Label)GetTemplateChild("inputLabel");
    }

    internal override InputView GetCoreContent()
    {
        return this.FindByName<BorderlessEditor>("inputBorderlessEditor"); ;
    }

    private void InputBorderlessEditor_Focused(object sender, FocusEventArgs e)
    {
        if (sender is BorderlessEditor)
        {
            SetBorderColor(this);
        }
    }

    private void InputBorderlessEditor_Unfocused(object sender, FocusEventArgs e)
    {
        if (sender is BorderlessEditor)
        {
            SetBorderColor(this);
        }
    }
}
