using DemoApp.ViewModels;

namespace DemoApp;

public partial class TableView : ContentPage
{
    TableViewModel _viewModel = new TableViewModel();
    public TableView()
	{
		InitializeComponent();
        BindingContext = _viewModel;
    }
}
