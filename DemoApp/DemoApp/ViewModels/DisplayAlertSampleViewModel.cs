using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;

namespace DemoApp.ViewModels
{
    public partial class DisplayAlertSampleViewModel : ObservableObject
    {
        [ObservableProperty] string _title;
        [ObservableProperty] string _message;
        [ObservableProperty] string _primaryButtonText;
        [ObservableProperty] string _secondaryButtonText;
        public DisplayAlertSampleViewModel()
        {
            Title = "Alert";
            Message = "Do you want to delete?";
            PrimaryButtonText = "Okay";
            SecondaryButtonText = "";
        }

        [RelayCommand]
        async Task ShowAlert()
        {
            var confirmationDialog = new DisplayAlert(Title, Message, PrimaryButtonText, SecondaryButtonText);
            confirmationDialog.OnPrimaryButtonClicked += delegate { OnPrimarySelected(); };
            confirmationDialog.OnSecondaryButtonClicked += delegate { OnSecondarySelected(); };
            await confirmationDialog.Show();
        }

        void OnPrimarySelected()
        {
            Console.WriteLine("Primary button clicked");
        }

        void OnSecondarySelected()
        {
            Console.WriteLine("Secondary button clicked");
        }
    }
}
