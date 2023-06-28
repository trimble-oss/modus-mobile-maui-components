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
        
        public bool ShowPassword
        {
            get
            {
                return _showPassword;
            }
            set
            {
                _showPassword = value;
                OnPropertyChanged(nameof(ShowPassword));
            }
        }
        public bool IsAutoSize
        {
            get
            {
                return _isAutoSize;
            }
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
            get
            {
                return _isEnabled;
            }
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
            get
            {
                return _isReadOnly;
            }
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
