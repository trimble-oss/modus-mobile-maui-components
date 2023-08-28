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
                "Spinner",
                "SegmentedControl",
                "ListView",
                "DataGrid",
                "PopupView",
                "TabbedPage",
                "Badge",
                "Accordion",
                "Chips",
                "Switch",
                "ProgressBar"
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
                        Navigation.PushAsync(new ButtonPage());
                        break;
                    case "Input":
                        Navigation.PushAsync(new InputPage());
                        break;
                    case "MultiLineInput":
                        Navigation.PushAsync(new MultiLineInputPage());
                        break;
                    case "Modal":
                        Navigation.PushAsync(new ModalPage());
                        break;
                    case "Toast":
                        Navigation.PushAsync(new ToastPage());
                        break;
                    case "CheckBox":
                        Navigation.PushAsync(new CheckBoxPage());
                        break;
                    case "Card":
                        Navigation.PushAsync(new CardPage());
                        break;
                    case "RadioButton":
                        Navigation.PushAsync(new RadioButtonPage());
                        break;
                    case "NumberInput":
                        Navigation.PushAsync(new NumberInputPage());
                        break;
                    case "Spinner":
                        Navigation.PushAsync(new SpinnerPage());
                        break;
                    case "SegmentedControl":
                        Navigation.PushAsync(new SegmentedControlPage());
                        break;
                    case "ListView":
                        Navigation.PushAsync(new ListViewPage());
                        break;
                    case "DataGrid":
                        Navigation.PushAsync(new TableViewPage());
                        break;
                    case "PopupView":
                        Navigation.PushAsync(new SamplePopupPage());
                        break;
                    case "Accordion":
                        Navigation.PushAsync(new AccordionSamplePage());
                        break;
                    case "TabbedPage":
                        Navigation.PushAsync(new TabbedPageDemo());
                        break;
                    case "Badge":
                        Navigation.PushAsync(new BadgeSamplePage());
                        break;
                    case "ProgressBar":
                        Navigation.PushAsync(new ProgressBarSamplePage());
                        break;
                    case "Chips":
                        Navigation.PushAsync(new ChipsSamplePage());
                        break;
                    case "Switch":
                        Navigation.PushAsync(new SwitchSamplePage());
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
