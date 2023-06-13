using DemoApp.ViewModels;
using System.Text.RegularExpressions;
using Trimble.Modus.Components.Controls;

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

    private Tuple<bool, string> TMInputValidation(object sender)
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

    private void IsReadOnly_Toggled(object sender, ToggledEventArgs e)
    {
        _inputPageViewModel.IsReadOnly = e.Value;
    }

    private void IsEnabled_Toggled(object sender, ToggledEventArgs e)
    {
        _inputPageViewModel.IsEnabled = e.Value;
    }
}
