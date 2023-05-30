using DemoApp.Views;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Popup.Services;

namespace DemoApp;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
       
	}

    private TMModal CreateTMModal()
    {
        TMInput inputRef = null;
        TMModal tmModal = new("This is a header", ImageSource.FromFile("placeholder.png"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit, mollit anim id est laborum.");
        tmModal.FullWidthButton = true;
        tmModal.AddAction("Default", async () =>
        {
            await DisplayAlert("Alert", "You have entered "+inputRef?.Text, "Close");
        });
        tmModal.AddDangerButton("Default");
        tmModal.AddDangerButton("Danger");
        return tmModal;
    }

    private void ButtonClicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        switch (clickedButton.AutomationId)
        {
            case "tmbutton":
                Navigation.PushAsync(new TMButtonPage());
                break;
            case "tminput":
                Navigation.PushAsync(new TMInputPage());
                break;
            case "tmmodal":
                Navigation.PushAsync(new TMModalPage());
                break;
            case "tmtoast":
                Navigation.PushAsync(new TMToastPage());
                break;
            default:
                Console.WriteLine("Default Case");
                break;
        }
    }
}

