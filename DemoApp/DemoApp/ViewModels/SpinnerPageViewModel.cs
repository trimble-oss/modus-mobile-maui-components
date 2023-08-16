using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public class SpinnerPageViewModel : BaseViewModel
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
        public SpinnerPageViewModel()
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
