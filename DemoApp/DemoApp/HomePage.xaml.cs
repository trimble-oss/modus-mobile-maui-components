using System.Collections.ObjectModel;

namespace DemoApp;

public partial class HomePage : ContentPage
{
    public ObservableCollection<string> ControlNames { get; set; }
    public HomePage()
    {
        InitializeComponent();

        var items = new List<string>
            {
                "Modus Controls",
                "In-Field Components",
        };
        ControlNames = new ObservableCollection<string>(items);

        BindingContext = this;
    }

    private void TMListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is string name)
        {
            switch (name)
            {
                case "In-Field Components":
                    Navigation.PushAsync(new InFieldComponentsPage());
                    break;
                default:
                    Navigation.PushAsync(new ModusControlsPage());
                    break;
            }
        }
    }
}
