using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    partial class SliderViewModel : ObservableValidator
    {
        [ObservableProperty]
        private string _minimumValueText = "0";
        [ObservableProperty]
        private string _maximumValueText = "10";
        [ObservableProperty]
        private double _minimumValue = 0;
        [ObservableProperty]
        private double _maximumValue = 10;
        [ObservableProperty]
        private bool _showSteps = true;
        [ObservableProperty]
        private bool _showTooltip = true;
        [ObservableProperty]
        private bool _isEnabled = true;
        [ObservableProperty]
        private SliderSize _size = SliderSize.Medium;
        partial void OnMaximumValueTextChanged(string value)
        {
            if(double.TryParse(value, out double result))
            {
                MaximumValue = result;
            }
        }
        partial void OnMinimumValueTextChanged(string value)
        {
            if (double.TryParse(value, out double result))
            {
                MinimumValue = result;
            }
        }

        [RelayCommand]
        private void SizeRadioButton(TMRadioButtonEventArgs e)
        {
            Size = (SliderSize)Enum.Parse(typeof(SliderSize), e.Value.ToString());
        }
    }
}
