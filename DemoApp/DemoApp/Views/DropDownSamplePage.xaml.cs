using DemoApp.ViewModels;
namespace DemoApp.Views;

public partial class DropDownSamplePage : ContentPage
{
    public DropDownSamplePage()
    {
        InitializeComponent();
        BindingContext = new DropDownSampleViewModel();
    }
}
