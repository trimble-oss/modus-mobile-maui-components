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
        private int selectedIndexOfEquipment;
        [ObservableProperty]
        private int selectedIndexOfColor;
        [ObservableProperty]
        private List<string> equipmentSource;
        [ObservableProperty]
        public List<string> colorSource;
        public DropDownSampleViewModel()
        {
            EquipmentSource = new List<string>() { "Excavator", "Bulldozer", "Loader", "Grader", "Trencher", "Backhoe", "Compactors", "Crane" };
            ColorSource = new List<string>() { "Red", "Yellow", "Black" };
            selectedIndexOfEquipment = 6;
            selectedIndexOfColor = 2;
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
