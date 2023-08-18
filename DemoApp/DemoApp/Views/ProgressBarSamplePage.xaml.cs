using DemoApp.Helpers;
using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class ProgressBarSamplePage : ContentPage
{
    public ProgressBarSamplePageViewModel ViewModel = ServiceProviderUtils.Current.GetService<ProgressBarSamplePageViewModel>();
    public ProgressBarSamplePage()
    {
        InitializeComponent();
        BindingContext = ViewModel;
    }
}
