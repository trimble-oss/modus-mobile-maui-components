using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public class ButtonPageViewModel : BaseViewModel
    {
        private Trimble.Modus.Components.Enums.Size _selectedFontSize;
        private ButtonStyle _selectedButtonStyle;
        private bool _isDisabled;
        private string _selectedImageOption;
        private string _leftIconSource;
        private string _rightIconSource;

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

        public string SelectedImageOption
        {
            get
            {
                return _selectedImageOption;
            }
            set
            {
                if (value != _selectedImageOption)
                {
                    _selectedImageOption = value;

                    OnPropertyChanged(nameof(SelectedImageOption));
                    OnImagePositionChanged(this);
                }
            }
        }

        public string RightIconSource
        {
            get
            {
                return _rightIconSource;
            }
            set
            {
                _rightIconSource = value;

                OnPropertyChanged(nameof(RightIconSource));
            }
        }
        public string LeftIconSource
        {
            get
            {
                return _leftIconSource;
            }
            set
            {
                _leftIconSource = value;

                OnPropertyChanged(nameof(LeftIconSource));
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
            SelectedImageOption = "None";
            MyCommand = new Command(OnClicked);
        }

        private static void OnClicked(object obj)
        {
            Console.WriteLine(obj.ToString());
        }

        private static void OnImagePositionChanged(ButtonPageViewModel buttonPageViewModel)
        {
            switch (buttonPageViewModel.SelectedImageOption)
            {
                case "Left":
                    buttonPageViewModel.LeftIconSource = "gallery_icon.png";
                    buttonPageViewModel.RightIconSource = null;
                    break;
                case "Right":
                    buttonPageViewModel.LeftIconSource = null;
                    buttonPageViewModel.RightIconSource = "gallery_icon.png";
                    break;
                case "Both":
                    buttonPageViewModel.LeftIconSource = "gallery_icon.png";
                    buttonPageViewModel.RightIconSource = "gallery_icon.png";
                    break;
                default:
                case "None":
                    buttonPageViewModel.LeftIconSource = null;
                    buttonPageViewModel.RightIconSource = null;
                    break;
            }
        }
    }
}
