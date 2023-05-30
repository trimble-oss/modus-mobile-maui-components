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

