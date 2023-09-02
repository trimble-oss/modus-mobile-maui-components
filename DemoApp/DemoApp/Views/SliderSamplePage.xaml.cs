using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class SliderSamplePage : ContentPage
{
    SliderViewModel _viewModel = new SliderViewModel();
	public SliderSamplePage()
	{
		InitializeComponent();
        BindingContext = _viewModel;
	}
}
