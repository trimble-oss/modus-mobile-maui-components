using DemoApp.InFieldViews;
using System.Collections.ObjectModel;

namespace DemoApp;

public partial class InFieldComponentsPage : ContentPage
{
    public ObservableCollection<string> ControlNames { get; set; }
    public InFieldComponentsPage()
    {
        InitializeComponent();


        var inFieldComponents = new List<string>
        {
            "Battery",
            "Joystick",
        };
        ControlNames = new ObservableCollection<string>(inFieldComponents.OrderBy(item => item));

        BindingContext = this;
    }

    private void TMListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is string name)
        {
            switch (name)
            {
                case "Battery":
                    Navigation.PushAsync(new BatterySamplePage());
                    break;
                default:
                    break;
            }
        }

    }
}
