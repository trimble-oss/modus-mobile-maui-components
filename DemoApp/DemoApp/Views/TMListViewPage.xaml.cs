using System.Collections.ObjectModel;
using Trimble.Modus.Components;

namespace DemoApp.Views;

public partial class TMListViewPage : ContentPage
{

    public TMListViewPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

}

public class Person
{
    public string Title { get; set; }
    public int Age { get; set; }
    public ImageSource LeftIconSource { get; set; }

    public ImageSource RightIconSource { get; set; }
    public string Description { get; set; }
}
