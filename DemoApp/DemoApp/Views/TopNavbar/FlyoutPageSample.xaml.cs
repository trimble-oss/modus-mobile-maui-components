using DemoApp.ViewModels;
using Trimble.Modus.Components;

namespace DemoApp.Views.TopNavbar;

public partial class FlyoutPageSample : TMFlyoutPage
{
    private FlyoutPageSampleViewModel _viewmodel = new();
    public FlyoutPageSample()
    {
        InitializeComponent();
        BindingContext = _viewmodel;
    }
}

