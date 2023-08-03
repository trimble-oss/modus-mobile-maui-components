using DemoApp.ViewModels;

namespace DemoApp.Views.TabViewItems;

public class TabViewOneViewModel
{
    public string HeaderText { get; set; } = "First Page";
}

public partial class TabViewOne : ContentView
{
    public TabViewOne()
    {
        InitializeComponent();
        BindingContext = new TMListViewPageViewModel();
    }
}

