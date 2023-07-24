using DemoApp.Resources;
using Trimble.Modus.Components;
using DemoApp.ViewModels;

namespace DemoApp.Views
{
    public partial class TMListViewPage : ContentPage
    {
        #region Private Fields
        private TMListViewPageViewModel tmListViewPageViewModel;
        #endregion
        #region Constructor
        public TMListViewPage()
        {
            InitializeComponent();
            tmListViewPageViewModel = new TMListViewPageViewModel(TextCell);
            BindingContext = tmListViewPageViewModel;
        }
        #endregion

        #region Event Handlers
        private void ListItemSelected(object sender, SelectableItemEventArgs e)
        {
            Console.WriteLine(((User)e.SelectableItem).Name + " " + ((User)e.SelectableItem).Address + " " + textCellList.selectableItems.Count);
        }

        private void OnCellGroupButtonChanged(object sender, TMRadioButtonEventArgs e)
        {
            tmListViewPageViewModel.ItemTemplate = e.RadioButtonIndex switch
            {
                0 => TextCell,
                1 => ViewCell,
                _ => TextCell
            };
        }
        private void OnSelectionGroupButtonChanged(object sender, TMRadioButtonEventArgs e)
        {
            tmListViewPageViewModel.SelectionMode = e.RadioButtonIndex switch
            {
                0 => Trimble.Modus.Components.Enums.ListSelectionMode.Single,
                1 => Trimble.Modus.Components.Enums.ListSelectionMode.Multiple,
                2 => Trimble.Modus.Components.Enums.ListSelectionMode.None,
                _ => Trimble.Modus.Components.Enums.ListSelectionMode.Single,
            };
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
