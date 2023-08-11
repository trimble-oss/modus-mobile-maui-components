using DemoApp.ViewModels;

namespace DemoApp.Views.TabViewItems;

public partial class TabViewTwo : ContentView
{
    public TabViewTwo()
    {
        InitializeComponent();
        BindingContext = new SpinnerPageViewModel();
    }
}

