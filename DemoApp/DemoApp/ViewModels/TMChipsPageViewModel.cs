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

        public ChipsSamplePageViewModel()
        {
            ChipsCollection.Add(new ChipsItem("Chips 1", ChipType.Input, ChipSize.Default, ImageConstants.AccountIcon, ClickChipCommand, CloseChipCommand));
            ChipsCollection.Add(new ChipsItem("Chips 2", ChipType.Input, ChipSize.Default, ImageConstants.AccountIcon, ClickChipCommand, CloseChipCommand));
            ChipsCollection.Add(new ChipsItem("Chips 3", ChipType.Input, ChipSize.Default, ImageConstants.AccountIcon, ClickChipCommand, CloseChipCommand));
            ChipsCollection.Add(new ChipsItem("Chips 4", ChipType.Input, ChipSize.Default, ImageConstants.AccountIcon, ClickChipCommand, CloseChipCommand));
        }
        [RelayCommand]
        public void ClickChip(object tMChips)
        {
            Console.WriteLine("Clicked "+((TMChips)tMChips).Title);
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
            ChipsCollection.Add(new ChipsItem("Additional Chips "+count, ChipType.Input, ChipSize.Default, ImageConstants.AccountIcon, ClickChipCommand, CloseChipCommand));
        }
    }
}
