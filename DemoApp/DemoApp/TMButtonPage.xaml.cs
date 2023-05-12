namespace DemoApp;

public partial class TMButtonPage : ContentPage
{
	public TMButtonPage()
	{
		InitializeComponent();
	}

    private void button_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("Clicked");
    }

 
}