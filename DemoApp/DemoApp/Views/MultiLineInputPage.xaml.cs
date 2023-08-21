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
}
