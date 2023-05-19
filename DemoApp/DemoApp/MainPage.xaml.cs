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
            default:
                Console.WriteLine("Default Case");
                break;
        }
    }
}

