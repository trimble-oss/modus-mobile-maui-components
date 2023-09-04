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
            var confirmationDialog = new AlertDialogue(Title, Message, PrimaryButtonText, SecondaryButtonText);
            try
            {
                var result = await confirmationDialog.Show();
                Console.WriteLine(result ? "Primary button tapped" : "Secondary button tapped");
            } catch (Exception ex)
            {
                Console.WriteLine("Invalid result " + ex.Message);
            }
        }
    }
}
