using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class AccordionSamplePage : ContentPage
{
    AccordionSampleViewModel _viewModel = new AccordionSampleViewModel();
	public AccordionSamplePage()
	{
		InitializeComponent();
        BindingContext = _viewModel;
	}
}
