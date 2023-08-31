using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class DisplayAlertSamplePage : ContentPage
{
    DisplayAlertSampleViewModel _viewModel = new DisplayAlertSampleViewModel();
    public DisplayAlertSamplePage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }
}
