using DemoApp.ViewModels;
namespace DemoApp.Views;

public partial class AlertSamplePage : ContentPage
{
    public AlertSamplePage()
    {
        InitializeComponent();
        BindingContext = new AlertSamplePageViewModel(alertLayout);
    }
}

