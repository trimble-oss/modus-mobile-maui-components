using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public class SpinnerSamplePageViewModel : ObservableObject
    {
        public ICommand SpinnerCommand { get; set; }
        private SpinnerColor _spinnerColor;
        public SpinnerColor SpinnerColor {
            get
            {
                return _spinnerColor;
            }
            set
            {
                _spinnerColor = value;
                OnPropertyChanged(nameof(SpinnerColor));
            }
        }
        public SpinnerSamplePageViewModel()
        {
            SpinnerColor = SpinnerColor.Primary;
            SpinnerCommand = new Command(methodsCommand);        }

        private void methodsCommand(object obj)
        {
            TMRadioButtonEventArgs e = (TMRadioButtonEventArgs)obj;
            SpinnerColor = ((string)e.Value == "Primary") ? SpinnerColor.Primary : SpinnerColor.Secondary;
        }
    }
}
