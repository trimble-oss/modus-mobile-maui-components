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
        TMModal tmModal = new TMModal("This is a header", ImageSource.FromFile("placeholder.png"));
        tmModal.AddAction("Default", async (obj, args) =>
        {
            await DisplayAlert("Alert", "Modal Closed", "Cancel");
        });
        tmModal.AddAction("Default");
        tmModal.AddAction("Default");
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
                PopupService.Instance.PushAsync(CreateTMModal());
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

