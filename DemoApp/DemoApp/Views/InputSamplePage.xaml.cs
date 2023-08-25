using DemoApp.ViewModels;

namespace DemoApp;

public partial class InputSamplePage : ContentPage
{
    private InputSamplePageViewModel _inputPageViewModel;

    public InputSamplePage()
    {
        _inputPageViewModel = new InputSamplePageViewModel();
        InitializeComponent();
        BindingContext = _inputPageViewModel;
    }
}
