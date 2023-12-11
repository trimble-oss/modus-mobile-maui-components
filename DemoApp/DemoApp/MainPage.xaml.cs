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
            var items = new List<string>
            {
                "Accordion",
                "Badge",
                "Button",
                "Card",
                "CheckBox",
                "Chips",
                "DataGrid",
                "Display Alert",
                "DropDown",
                "Input",
                "MultiLineInput",
                "NumberInput",
                "ListView",
                "Modal",
                "PopupView",
                "ProgressBar",
                "RadioButton",
                "SegmentedControl",
                "Slider",
                "Spinner",
                "Switch",
                "TabbedPage",
                "Toast",
                "Message",
                "Alert"
            };
            ControlNames = new ObservableCollection<string>(items.OrderBy(item => item));
            BindingContext = this;
        }
        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is string name)
            {
                switch (name)
                {
                    case "Button":
                        Navigation.PushAsync(new ButtonSamplePage());
                        break;
                    case "Input":
                        Navigation.PushAsync(new InputSamplePage());
                        break;
                    case "MultiLineInput":
                        Navigation.PushAsync(new MultiLineInputSamplePage());
                        break;
                    case "Modal":
                        Navigation.PushAsync(new ModalSamplePage());
                        break;
                    case "Toast":
                        Navigation.PushAsync(new ToastSamplePage());
                        break;
                    case "CheckBox":
                        Navigation.PushAsync(new CheckBoxSamplePage());
                        break;
                    case "Card":
                        Navigation.PushAsync(new CardSamplePage());
                        break;
                    case "RadioButton":
                        Navigation.PushAsync(new RadioButtonSamplePage());
                        break;
                    case "NumberInput":
                        Navigation.PushAsync(new NumberInputSamplePage());
                        break;
                    case "Spinner":
                        Navigation.PushAsync(new SpinnerSamplePage());
                        break;
                    case "SegmentedControl":
                        Navigation.PushAsync(new SegmentedControlSamplePage());
                        break;
                    case "ListView":
                        Navigation.PushAsync(new ListViewSamplePage());
                        break;
                    case "DataGrid":
                        Navigation.PushAsync(new TableViewSamplePage());
                        break;
                    case "PopupView":
                        Navigation.PushAsync(new SamplePopupPage());
                        break;
                    case "Accordion":
                        Navigation.PushAsync(new AccordionSamplePage());
                        break;
                    case "TabbedPage":
                        Navigation.PushAsync(new TabbedPageSamplePage());
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
                    case "Slider":
                        Navigation.PushAsync(new SliderSamplePage());
                        break; 
                    case "Switch":
                        Navigation.PushAsync(new SwitchSamplePage());
                        break;
                    case "Display Alert":
                        Navigation.PushAsync(new DisplayAlertSamplePage());
                        break;
                    case "DropDown":
                        Navigation.PushAsync(new DropDownSamplePage());
                        break;
                    case "Message":
                        Navigation.PushAsync(new MessageSamplePage());
                        break;
                    case "Alert":
                        Navigation.PushAsync(new AlertSamplePage());
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
