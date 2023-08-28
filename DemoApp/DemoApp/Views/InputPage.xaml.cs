using DemoApp.ViewModels;

namespace DemoApp;

public partial class InputPage : ContentPage
{
    private InputSamplePageViewModel _inputPageViewModel;

    public InputPage()
    {
        _inputPageViewModel = new InputSamplePageViewModel();
        InitializeComponent();
        BindingContext = _inputPageViewModel;
    }
}
