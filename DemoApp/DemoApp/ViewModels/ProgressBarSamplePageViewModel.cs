using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public partial class ProgressBarSamplePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ProgressBarSize _selectedProgressBarSize;
        [ObservableProperty]
        private string _progressText;
        [ObservableProperty]
        private double _progressValue;
        public ProgressBarSamplePageViewModel()
        {
            SelectedProgressBarSize = ProgressBarSize.Default;
            ProgressText = "Loading";
            ProgressValue = 0.5f;
        }

        [RelayCommand]
        private void ProgressBarSizeChanged(TMRadioButtonEventArgs e)
        {
            switch (e.RadioButtonIndex)
            {
                case 1:
                    SelectedProgressBarSize = ProgressBarSize.Small;
                    break;
                default:
                    SelectedProgressBarSize = ProgressBarSize.Default;
                    break;
            }
        }
    }
}
