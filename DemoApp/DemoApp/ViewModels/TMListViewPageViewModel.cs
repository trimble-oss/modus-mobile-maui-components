using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Helper;
using DemoApp.Models;
using System.Collections.ObjectModel;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;
using SelectionChangedEventArgs = Trimble.Modus.Components.SelectionChangedEventArgs;

namespace DemoApp.ViewModels
{
    internal partial class ListViewSamplePageViewModel : ObservableObject
    {
        #region Private Fields
        [ObservableProperty]
        private ListSelectionMode selectionMode;

        [ObservableProperty]
        private ObservableCollection<User> itemSource;

        [ObservableProperty]
        private User selectedItem;

        [ObservableProperty]
        private ObservableCollection<object> selectedItems;
        #endregion

        #region Constructor
        public ListViewSamplePageViewModel()
        {
            InitialzeUsers();
            SelectionMode = ListSelectionMode.Single;
        }

        #endregion
        #region Private Methods

        private async void InitialzeUsers()
        {
            ItemSource = await UserDataCreator.LoadData();
            SelectionMode = ListSelectionMode.Single;
        }

        [RelayCommand]
        private void ItemSelected(SelectionChangedEventArgs e)
        {
            if (e.PreviousSelection == null)
            {
                Console.WriteLine("null");
            }

            //foreach(var item in e.PreviousSelection)
            //{
            //    Console.WriteLine(" PreviousSelections " + ((User)item).Name +" Index "+ e.SelectedIndex);
            //}
            //foreach (var item in e.CurrentSelection)
            //{
            //    Console.WriteLine(" CurrentSelections " + ((User)item).Name + " Index " + e.SelectedIndex);
            //}
        }

        [RelayCommand]
        private void SelectionGroup(TMRadioButtonEventArgs parameter)
        {
            if (parameter is TMRadioButtonEventArgs radioButton)
            {
                SelectionMode = radioButton.RadioButtonIndex switch
                {
                    1 => ListSelectionMode.Multiple,
                    2 => ListSelectionMode.None,
                    _ => ListSelectionMode.Single,
                };
            }
        }

        partial void OnSelectionModeChanged(ListSelectionMode value)
        {
            if (value == ListSelectionMode.Single)
            {
                SelectedItem = ItemSource[0];
            }
            else if (value == ListSelectionMode.Multiple)
            {
                SelectedItems = new ObservableCollection<object>() { ItemSource[5], ItemSource[6], };
            }
        }

        [RelayCommand]
        private void PhoneClicked()
        {
            Console.WriteLine("Phone Clicked");
        }
        [RelayCommand]
        private void EmailClicked()
        {
            Console.WriteLine("Email Clicked");
        }
        #endregion

    }
}
