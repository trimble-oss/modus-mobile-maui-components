using CommunityToolkit.Mvvm.ComponentModel;
using DemoApp.Models;
using Newtonsoft.Json;
using Trimble.Modus.Components.Enums;
using System.Collections;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;
using System.Collections.ObjectModel;
using SelectionChangedEventArgs = Trimble.Modus.Components.SelectionChangedEventArgs;
using DemoApp.Helper;

namespace DemoApp.ViewModels
{
    internal partial class TMListViewSamplePageViewModel : ObservableObject
    {
        #region Private Fields
        [ObservableProperty]
        private ListSelectionMode selectionMode;

        [ObservableProperty]
        private IEnumerable itemSource;
        #endregion

        #region Constructor
        public TMListViewSamplePageViewModel()
        {
            InitialzeUsers();
            SelectionMode = ListSelectionMode.Single;
        }

        #endregion
        #region Private Methods
        private async void InitialzeUsers()
        {
            ItemSource = await UserDataCreator.LoadData();
        }
        [RelayCommand]
        private void ItemSelected(SelectionChangedEventArgs e)
        {
            if(e.PreviousSelection == null)
            {
                Console.WriteLine("null");
            }
           
            foreach(var item in e.PreviousSelection)
            {
                Console.WriteLine(" PreviousSelections " + ((User)item).Name +" Index "+ e.SelectedIndex);
            }
            foreach (var item in e.CurrentSelection)
            {
                Console.WriteLine(" CurrentSelections " + ((User)item).Name + " Index " + e.SelectedIndex);
            }
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
