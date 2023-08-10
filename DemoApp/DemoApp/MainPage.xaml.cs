using DemoApp.Views;
using System.Collections.ObjectModel;
using Trimble.Modus.Components.Popup.Services;
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
                "MultiLineInput",
                "NumberInput",
                "Modal",
                "Toast",
                "CheckBox",
                "Card",
                "RadioButton",
                "DataGrid",
                "TabbedPage"
                "PopupView"
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
                    case "MultiLineInput":
                        Navigation.PushAsync(new MultiLineInputPage());
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
                    case "Card":
                        Navigation.PushAsync(new TMCardPage());
                        break;
                    case "RadioButton":
                        Navigation.PushAsync(new TMRadioButtonPage());
                        break;
                    case "NumberInput":
                        Navigation.PushAsync(new TMNumberInputPage());
                        break;
                    case "Spinner":
                        Navigation.PushAsync(new TMSpinnerPage());
                        break;
                    case "SegmentedControl":
                        Navigation.PushAsync(new TMSegmentedControlPage());
                        break;
                    case "ListView":
                        Navigation.PushAsync(new TMListViewPage());
                        break;
                    case "DataGrid":
                        Navigation.PushAsync(new TableViewPage());
                        break;
                    case "TabbedPage":
                        Navigation.PushAsync(new TabbedPageDemo());
                    case "PopupView":
                        Navigation.PushAsync(new SamplePopupPage());
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
