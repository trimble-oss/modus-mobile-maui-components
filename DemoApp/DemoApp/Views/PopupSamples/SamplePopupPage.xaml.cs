using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class SamplePopupPage : ContentPage
{
    SamplePopupPageViewModel _viewModel = new SamplePopupPageViewModel();
	public SamplePopupPage()
	{
		InitializeComponent();
        BindingContext = _viewModel;
    }
}
