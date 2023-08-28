using DemoApp.ViewModels;
using System.Text.RegularExpressions;
using Trimble.Modus.Components;

namespace DemoApp;

public partial class MultiLineInputSamplePage : ContentPage
{
    private InputSamplePageViewModel _inputPageViewModel;

    public MultiLineInputSamplePage()
    {
        _inputPageViewModel = new InputSamplePageViewModel();

        InitializeComponent();
        BindingContext = _inputPageViewModel;

    }
}
