using DemoApp.ViewModels;

namespace DemoApp.Views.TabViewItems;

public partial class TabViewOne : ContentView
{
    public TabViewOne()
    {
        InitializeComponent();
        BindingContext = new TMListViewPageViewModel();
    }
}

