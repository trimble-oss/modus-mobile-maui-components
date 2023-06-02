using DemoApp.Views;
using System.Collections.ObjectModel;


namespace DemoApp
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> ControlNames { get; set; }
        public MainPage()
        {
            InitializeComponent();
            ControlNames = new ObservableCollection<string>
            {
                "Button",
                "Input",
                "Modal",
                "Toast",
                "CheckBox"
            };
            BindingContext = this;
        }
        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is string name)
            {
                switch (name)
                {
                    case "Button":
                        Navigation.PushAsync(new TMButtonPage());
                        break;
                    case "Input":
                        Navigation.PushAsync(new TMInputPage());
                        break;
                    case "Modal":
                        Navigation.PushAsync(new TMModalPage());
                        break;
                    case "Toast":
                        Navigation.PushAsync(new TMToastPage());
                        break;
                    case "CheckBox":
                        Navigation.PushAsync(new TMCheckBoxPage());
                        break;
                    default:
                        Console.WriteLine("Default Case");
                        break;
                }
            }

       ((ListView)sender).SelectedItem = null;
        }
    }
}
