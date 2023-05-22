using Trimble.Modus.Components;
using Trimble.Modus.Components.Popup.Pages;

namespace DemoApp;

public partial class TMInputPage : ContentPage
{
	public TMInputPage()
	{
		InitializeComponent();
    }
    private void modusInput_Focused(object sender, EventArgs e)
    {
        Console.WriteLine("Container focused");

    }

    private Tuple<bool, string> CustomValidateInput(object sender)
    {
        var input = sender as TMInput;
        if(input.Text.Length > 3)
        {
            return Tuple.Create(true, "Valid Text");
        }
        else
        {
            return Tuple.Create(false, "Invalid Text");
        }
    }
}