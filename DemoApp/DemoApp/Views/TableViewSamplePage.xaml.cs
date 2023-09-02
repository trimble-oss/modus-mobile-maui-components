using DemoApp.ViewModels;

namespace DemoApp;

public partial class TableViewSamplePage : ContentPage
{
    TableViewModel _viewModel = new TableViewModel();
    public TableViewSamplePage()
	{
		InitializeComponent();
        BindingContext = _viewModel;
    }
}
