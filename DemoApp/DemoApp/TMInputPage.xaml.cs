using Trimble.Modus.Components;
using System.Text.RegularExpressions;
using DemoApp.ViewModels;

namespace DemoApp;

public partial class TMInputPage : ContentPage
{
    private InputPageViewModel _inputPageViewModel;
	public TMInputPage()
	{
        _inputPageViewModel = new InputPageViewModel();

        InitializeComponent();
        BindingContext = _inputPageViewModel;
        
    }
    private void modusInput_Focused(object sender, EventArgs e)
    {
        Console.WriteLine("Container focused");

    }

    private Tuple<bool, string> CustomValidateInput(object sender)
    {
        var input = sender as TMInput;
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        // Create Regex object
        Regex regex = new Regex(pattern);

        // Match the input string against the pattern
        Match match = regex.Match(input.Text);

        // Return true if the input string matches the pattern, otherwise false
        //return match.Success;
        if (match.Success)
        {
            return Tuple.Create(true, "Valid Text");
        }
        else
        {
            return Tuple.Create(false, "Invalid Text");
        }
    }
}