using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DemoApp.ViewModels
{
    public class InputPageViewModel : BaseViewModel
    {
        public ICommand ShowPasswordCommand { get; set; }

        private bool _showPassword;
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
        public InputPageViewModel()
        {
            ShowPasswordCommand = new Command(ChangeShowPasswordState);
            ShowPassword = false;
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
