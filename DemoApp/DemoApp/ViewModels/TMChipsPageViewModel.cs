using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Constant;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    internal partial class TMChipsPageViewModel : ObservableObject
    {
        private int count = 0;
        public ObservableCollection<TMChips> ChipsCollection { get; } = new ObservableCollection<TMChips>();

        public TMChipsPageViewModel()
        {
            ChipsCollection.Add(new TMChips() { Title = "Chip 1", ChipType = ChipType.Input, LeftIconSource = ImageConstants.AccountIcon, ChipSize = ChipSize.Small });
            ChipsCollection.Add(new TMChips() { Title = "Chip 2", ChipType = ChipType.Input, LeftIconSource = ImageConstants.ContactIcon, ChipSize = ChipSize.Small }); 
            ChipsCollection.Add(new TMChips() { Title = "Chip 3", ChipType = ChipType.Input, LeftIconSource = ImageConstants.SearchIcon, ChipSize = ChipSize.Small });
         
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
            ChipsCollection.Add(new TMChips() { Title = "Additional Chip " + count, ChipType = ChipType.Input, LeftIconSource = ImageConstants.AccountIcon});
        }
    }
}
