using DemoApp.ViewModels;

namespace DemoApp.Views.TabViewItems;

public partial class TabViewFour : ContentView
{
    public TabViewFour()
    {
        InitializeComponent();
        BindingContext = new SegmentedControlViewModel();
    }
}

