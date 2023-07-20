using DemoApp.Resources;
using Trimble.Modus.Components;
using DemoApp.ViewModels;

namespace DemoApp.Views
{
    public partial class TMListViewPage : ContentPage
    {
        #region Private Fields
        private TMListViewPageViewModel viewModel;
        #endregion

        #region Properties
        public DataTemplate TextCellTemplate { get; set; }
        public DataTemplate ViewCellTemplate { get; set; }
        #endregion

        #region Constructor
        public TMListViewPage()
        {
            InitializeComponent();
            viewModel = new TMListViewPageViewModel();
            TextCellTemplate = TextCell;
            ViewCellTemplate = ViewCell;
            viewModel.ItemTemplate = TextCellTemplate;
            BindingContext = viewModel;
        }
        #endregion

        #region Event Handlers
        private void ListItemSelected(object sender, SelectableItemEventArgs e)
        {
            Console.WriteLine(((User)e.SelectableItem).Name + " " + ((User)e.SelectableItem).Address + " " + textCellList.selectableItems.Count);
        }

        private void OnCellGroupButtonChanged(object sender, TMRadioButtonEventArgs e)
        {
            if (e.RadioButtonIndex == 0)
            {
                viewModel.ItemTemplate = TextCell;
            }
            else if (e.RadioButtonIndex == 1)
            {
                viewModel.ItemTemplate = ViewCell;
            }
        }

        private void OnSelectionGroupButtonChanged(object sender, TMRadioButtonEventArgs e)
        {
            if (e.RadioButtonIndex == 0)
            {
                viewModel.SelectionMode = Trimble.Modus.Components.Enums.ListSelectionMode.Single;
            }
            else if (e.RadioButtonIndex == 1)
            {
                viewModel.SelectionMode = Trimble.Modus.Components.Enums.ListSelectionMode.Multiple;
            }
            else if (e.RadioButtonIndex == 2)
            {
                viewModel.SelectionMode = Trimble.Modus.Components.Enums.ListSelectionMode.None;
            }
        }

        private void OnPhoneClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Phone Clicked");
        }

        private void OnEmailClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Email Clicked");
        }
        #endregion
    }
}
