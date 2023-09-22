using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
