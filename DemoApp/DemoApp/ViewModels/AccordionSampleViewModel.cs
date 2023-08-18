using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Helper;
using DemoApp.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.ObjectModel;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public partial class AccordionSampleViewModel : ObservableObject
    {
        [ObservableProperty]
        private AccordionSize _size;
        [ObservableProperty]
        private AccordionChevronPosition _chevronPosition;
        [ObservableProperty]
        private IEnumerable _items ;

        public AccordionSampleViewModel()
        {
            InitializeUsers();
        }
        private async void InitializeUsers()
        {
            Items = await UserDataCreator.LoadData(20);
        }
        [RelayCommand]
        private void SizeRadioButton(TMRadioButtonEventArgs e)
        {
           Size = (AccordionSize)Enum.Parse(typeof(AccordionSize), e.Value.ToString());
        }
        [RelayCommand]
        private void ChevronPositionRadioButton(TMRadioButtonEventArgs e)
        {
            ChevronPosition = (AccordionChevronPosition)Enum.Parse(typeof(AccordionChevronPosition), e.Value.ToString());
        }
    }
}
