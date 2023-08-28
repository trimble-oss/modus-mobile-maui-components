using DemoApp.ViewModels;

namespace DemoApp;

public partial class InputPage : ContentPage
{
    private InputPageViewModel _inputPageViewModel;

    public InputPage()
    {
        _inputPageViewModel = new InputPageViewModel();
        InitializeComponent();
        BindingContext = _inputPageViewModel;
    }
}
