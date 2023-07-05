using System.Windows.Input;

namespace DemoApp.ViewModels
{
    public class InputPageViewModel : BaseViewModel
    {
        public ICommand ShowPasswordCommand { get; set; }

        private bool _showPassword;
        private bool _isEnabled;
        private bool _isReadOnly;
        private bool _isAutoSize;
        private bool _isRequired;

        public bool IsRequired
        {
            get => _isRequired;
            set
            {
                _isRequired = value;
                OnPropertyChanged(nameof(IsRequired));
            }
        }
        public bool ShowPassword
        {
            get => _showPassword;
            set
            {
                _showPassword = value;
                OnPropertyChanged(nameof(ShowPassword));
            }
        }
        public bool IsAutoSize
        {
            get => _isAutoSize;
            set
            {
                if (_isAutoSize != value)
                {
                    _isAutoSize = value;
                    OnPropertyChanged(nameof(IsAutoSize));
                }
            }
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                if (_isReadOnly != value)
                {
                    _isReadOnly = value;
                    OnPropertyChanged(nameof(IsReadOnly));
                }
            }
        }
        public InputPageViewModel()
        {
            ShowPasswordCommand = new Command(ChangeShowPasswordState);
            ShowPassword = false;
            IsEnabled = true;
            IsReadOnly = false;
            IsAutoSize = true;
            IsRequired = false;
        }

        private void ChangeShowPasswordState(object obj)
        {
            Console.WriteLine("In Command ");
            if (_showPassword == true)
            {
                ShowPassword = false;
            }
            else
            {
                ShowPassword = true;
            }
        }
    }
}
