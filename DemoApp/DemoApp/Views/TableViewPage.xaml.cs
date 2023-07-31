using DemoApp.ViewModels;

namespace DemoApp;

public partial class TableViewPage : ContentPage
{
    TableViewModel _viewModel = new TableViewModel();
    public TableViewPage()
	{
		InitializeComponent();
        BindingContext = _viewModel;
    }
}
