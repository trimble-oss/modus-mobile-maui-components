using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class ChipsSamplePage : ContentPage
{
    public ChipsSamplePage()
    {
        InitializeComponent();
        BindingContext = new ChipsSamplePageViewModel();
    }
}
