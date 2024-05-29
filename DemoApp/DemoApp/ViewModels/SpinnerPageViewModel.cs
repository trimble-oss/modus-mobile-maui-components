using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public partial class SpinnerSamplePageViewModel : ObservableObject
    {

        [ObservableProperty] private SpinnerColor spinnerColor;
        public SpinnerSamplePageViewModel()
        {
            SpinnerColor = SpinnerColor.Primary;
        }
      
        [RelayCommand]
        private void SpinnerSelectionChanged(TMRadioButtonEventArgs e)
        {
            SpinnerColor = e.RadioButtonIndex == 0 ? SpinnerColor.Primary : SpinnerColor.Secondary;
        }
    }
}
