using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trimble.Modus.Components;
using DropDownSelectionChangedEventArgs = Trimble.Modus.Components.DropDownSelectionChangedEventArgs;


namespace DemoApp.ViewModels
{
    public partial class DropDownSampleViewModel : ObservableObject
    {
        [ObservableProperty]
        private int selectedIndex;
        [ObservableProperty]
        private List<string> dropdownSource;
        [ObservableProperty]
        public List<string> colorSource;
        public DropDownSampleViewModel()
        {
            DropdownSource = new List<string>() { "Excavator", "Bulldozer", "Loader", "Grader", "Trencher", "Backhoe", "Compactors", "Crane" };
            ColorSource = new List<string>() { "Red", "Yellow", "Black" };
            SelectedIndex = 6; ;
        }
        [RelayCommand]
        private void ItemSelected(DropDownSelectionChangedEventArgs e)
        {
            if (e.PreviousSelection == null)
            {
                Console.WriteLine("PreviousSelection is null");
            }
            else
                Console.WriteLine("PreviousSelection " + e.PreviousSelection.ToString());
            Console.WriteLine("CurrentSelection " + e.CurrentSelection.ToString());
        }
    }
}
