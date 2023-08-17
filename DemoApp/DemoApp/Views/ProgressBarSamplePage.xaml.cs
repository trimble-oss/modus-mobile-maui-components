using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class ProgressBarSamplePage : ContentPage
{
    public ProgressBarSamplePageViewModel ViewModel = new();
    public ProgressBarSamplePage()
    {
        InitializeComponent();
        BindingContext = ViewModel;
    }
}
