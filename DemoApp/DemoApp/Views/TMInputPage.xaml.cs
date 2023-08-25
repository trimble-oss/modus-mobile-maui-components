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
}
