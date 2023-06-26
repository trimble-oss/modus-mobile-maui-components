using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public class SpinnerPageViewModel : BaseViewModel
    {
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
            SpinnerColor = SpinnerColor.White;
        }
    }
}
