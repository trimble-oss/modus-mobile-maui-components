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
        TMModal tMModal = new TMModalBuilder()
            .SetTitleText("Here's a header")
            .SetPrimaryText("Default")
            .SetSecondaryText("Default")
            .SetTertiaryText("Default")
            .SetTitleIcon(ImageSource.FromFile("placeholder.png"))
            .Build();
        return tMModal;
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
            default:
                Console.WriteLine("Default Case");
                break;
        }
    }
}

