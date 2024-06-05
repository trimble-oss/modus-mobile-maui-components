using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Constant;
using DemoApp.Models;
using System.Collections.ObjectModel;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    internal partial class ChipsSamplePageViewModel : ObservableObject
    {
        private int count = 0;
        public ObservableCollection<ChipsItem> ChipsCollection { get; } = new ObservableCollection<ChipsItem>();

        [ObservableProperty]
        private ChipSize chipSize;

        [ObservableProperty]
        private ChipState chipState;

        [ObservableProperty]
        private ChipStyle chipStyle;

        [ObservableProperty]
        private ChipType chipType;

        [ObservableProperty]
        private bool isEnabled = true;

        public ChipsSamplePageViewModel()
        {
            ChipsCollection.Add(new ChipsItem("Chips 1", ImageConstants.AccountIcon));
            ChipsCollection.Add(new ChipsItem("Chips 2", ImageConstants.ContactIcon));
            ChipsCollection.Add(new ChipsItem("Chips 3", ImageConstants.DarkGalleryIcon));
            count = ChipsCollection.Count;
        }

        [RelayCommand]
        public void SizeSelectionChanged(TMRadioButtonEventArgs e)
        {
            ChipSize = e.RadioButtonIndex == 0 ? ChipSize.Default : ChipSize.Small;
        }

        [RelayCommand]
        public void EnableSelectionChanged(TMRadioButtonEventArgs e)
        {
            IsEnabled = e.RadioButtonIndex == 0;
        }

        [RelayCommand]
        public void StateSelectionChanged(TMRadioButtonEventArgs e)
        {
            ChipState = e.RadioButtonIndex == 0 ? ChipState.Default : ChipState.Error;
        }

        [RelayCommand]
        public void StyleSelectionChanged(TMRadioButtonEventArgs e)
        {
            ChipStyle = e.RadioButtonIndex == 0 ? ChipStyle.Fill : ChipStyle.Outline;
        }

        [RelayCommand]
        public void TypeSelectionChanged(TMRadioButtonEventArgs e)
        {
            ChipType = e.RadioButtonIndex == 0 ? ChipType.Filter : ChipType.Input;
        }

        [RelayCommand]
        public void ClickChip(object tMChips)
        {
            Console.WriteLine("Clicked " + ((TMChips)tMChips).Title);
        }

        [RelayCommand]
        public void CloseChip(object tMChips)
        {
            Console.WriteLine("Removed " + ((TMChips)tMChips).Title);
            var chipToRemove = ChipsCollection.FirstOrDefault(c => c.Title == ((TMChips)tMChips).Title);
            if (chipToRemove != null)
            {
                ChipsCollection.Remove(chipToRemove);
            }
        }
        [RelayCommand]
        public void AddChip()
        {
            count++;
            ChipsCollection.Add(new ChipsItem("Chips " + count, ImageConstants.Email));
        }
    }
}
