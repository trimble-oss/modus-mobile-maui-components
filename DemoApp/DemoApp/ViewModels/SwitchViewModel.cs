using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public partial class SwitchSamplePageViewModel : ObservableObject
    {

        [ObservableProperty] private bool isEnabled = true;
        [ObservableProperty] private SwitchSize switchSize;
        public SwitchSamplePageViewModel()
        {

        }

        [RelayCommand]
        public void EnableSelectionChanged(TMRadioButtonEventArgs e)
        {
            IsEnabled = e.RadioButtonIndex == 0;
        }

        [RelayCommand]
        public void SizeSelectionChanged(TMRadioButtonEventArgs e)
        {
            SwitchSize = e.RadioButtonIndex == 0 ? SwitchSize.Medium : SwitchSize.Large;
        }

        [RelayCommand]
        private void Switch(object e)
        {
            Console.WriteLine("ToggledCommand " + ((TMSwitchEventArgs)e).Value);
        }
    }
}
