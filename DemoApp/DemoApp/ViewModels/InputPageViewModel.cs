using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace DemoApp.ViewModels
{
    public partial class InputSamplePageViewModel : ObservableValidator
    {
        public ICommand ShowPasswordCommand { get; set; }
        [ObservableProperty]
        private bool _isPassword;
        [ObservableProperty]
        private bool _showPassword;
        [ObservableProperty]
        private bool _isEnabled;
        [ObservableProperty]
        private bool _isReadOnly;
        [ObservableProperty]
        private bool _isAutoSize;
        [ObservableProperty]
        [Required]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        private string? _emailAddress;
        [ObservableProperty]
        private string _emailIDErrorText;
        [ObservableProperty]
        private string _emailIDSuccessText;
        [ObservableProperty]
        [Required]
        [MinLength(10, ErrorMessage ="Length should be greater than or equal to 10")]
        private string? _multiLineInput;
        [ObservableProperty]
        private string _multiLineInputErrorText;
        [ObservableProperty]
        private string _multiLineInputSuccessText;

        public InputSamplePageViewModel()
        {
            ShowPasswordCommand = new Command(ChangeShowPasswordState);
            ShowPassword = false;
            IsEnabled = true;
            IsReadOnly = false;
            IsAutoSize = true;
            IsPassword = true;
        }
        private void ChangeShowPasswordState(object obj)
        {
            if (ShowPassword == true)
            {
                ShowPassword = false;
            }
            else
            {
                ShowPassword = true;
            }
        }
        partial void OnEmailAddressChanged(string oldValue, string newValue)
        {
            ValidateAllProperties();
            var emailIdError = GetErrors("EmailAddress").ToList();
            if (emailIdError.Count > 0)
            {
                EmailIDErrorText = emailIdError[0].ErrorMessage;
                EmailIDSuccessText = null;
            }
            else
            {
                EmailIDSuccessText = "Email Address is valid";
                EmailIDErrorText = null;
            }
        }
        partial void OnMultiLineInputChanged(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                MultiLineInputErrorText = null;
                MultiLineInputSuccessText = null;
                return;
            }
            ValidateAllProperties();
            var multiLineInputError = GetErrors("MultiLineInput").ToList();
            if (multiLineInputError.Count > 0)
            {
                MultiLineInputErrorText = multiLineInputError[0].ErrorMessage;
                MultiLineInputSuccessText = null;
            }
            else
            {
                MultiLineInputSuccessText = "Input is valid";
                MultiLineInputErrorText = null;
            }
        }
        [RelayCommand]
        private void Focused(object obj)
        {
            Console.WriteLine("Focused " + ((FocusEventArgs)obj).IsFocused);
        }
        [RelayCommand]
        private void UnFocused(object obj)
        {
            Console.WriteLine("UnFocused " + ((FocusEventArgs)obj).IsFocused);
        }
        [RelayCommand]
        private void TogglePasswordIcon(object obj)
        {
            IsPassword = !IsPassword;
        }
    }
}
