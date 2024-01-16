using DemoApp.ViewModels;

namespace DemoApp.Views.TopNavbar;

public partial class TopNavbarMainSamplePage : ContentPage
{
    TopNavbarMainPageViewModel _topNavbarMainPageViewModel = new();
    public TopNavbarMainSamplePage()
    {
        InitializeComponent();
        BindingContext = _topNavbarMainPageViewModel;
    }
}

