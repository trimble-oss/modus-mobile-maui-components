using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public class ButtonPageViewModel : BaseViewModel
    {
        private Trimble.Modus.Components.Enums.Size _selectedFontSize;
        private ButtonStyle _selectedButtonStyle;
        private bool _isDisabled;

        public Trimble.Modus.Components.Enums.Size SelectedFontSize
        {
            get
            {
                return _selectedFontSize;
            }
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
            }
        }
        public ButtonStyle SelectedButtonStyle
        {
            get
            {
                return _selectedButtonStyle;
            }
            set
            {
                _selectedButtonStyle = value;
                OnPropertyChanged(nameof(SelectedButtonStyle));
            }
        }

        public bool IsDisabled
        {
            get => _isDisabled;
            set
            {
                if (_isDisabled != value)
                {
                    _isDisabled = value;
                }
                OnPropertyChanged(nameof(IsDisabled));
            }
        }
        public ICommand MyCommand { get; set; }

        public ButtonPageViewModel()
        {
            SelectedFontSize = Trimble.Modus.Components.Enums.Size.Default;
            SelectedButtonStyle = ButtonStyle.Fill;
            MyCommand = new Command(OnClicked);
        }

        private static void OnClicked(object obj)
        {
            Console.WriteLine(obj.ToString());
        }
    }
}
