using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DemoApp.ViewModels
{
    public class InputPageViewModel : INotifyPropertyChanged
    {
        public ICommand ShowPasswordCommand { get; private set; }

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

        /// <summary>
        /// This variables is used to raise the event when the property value is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This method is triggered when any of the property is changed
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public InputPageViewModel()
        {
            ShowPasswordCommand = new Command(ChangeShowPasswordState);
            _showPassword = false;
        }

        private void ChangeShowPasswordState()
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
