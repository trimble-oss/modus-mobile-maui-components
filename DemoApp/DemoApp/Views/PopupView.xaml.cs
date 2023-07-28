using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class PopupView
{
    PopupViewModel _viewModel = new PopupViewModel();
	public PopupView()
	{
		InitializeComponent();
        BindingContext = _viewModel;
	}
}
