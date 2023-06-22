using DemoApp.ViewModels;
using System.Text.RegularExpressions;
using Trimble.Modus.Components;

namespace DemoApp;

public partial class MultiLineInputPage : ContentPage
{
    private InputPageViewModel _inputPageViewModel;

    public MultiLineInputPage()
    {
        _inputPageViewModel = new InputPageViewModel();

        InitializeComponent();
        BindingContext = _inputPageViewModel;

    }
    private Tuple<bool, string> TMMultiLineInputValidation(object sender)
    {
        var input = sender as MultiLineInput;

        // Returns true if characters >100
        if (input.Text.Length >= 100)
        {
            return Tuple.Create(true, "100 or more characters");
        }
        else
        {
            return Tuple.Create(false, "Not enough characters");
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
    private void IsAutoSize_Toggled(object sender, ToggledEventArgs e)
    {
        _inputPageViewModel.IsAutoSize = e.Value;
    }
}
