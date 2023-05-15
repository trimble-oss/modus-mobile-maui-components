
using Mopups;
using Mopups.Services;
using Trimble.Modus.Components.Controls.Toast;

namespace DemoApp;

public partial class TMToastPage : ContentPage
{
	public TMToastPage()
	{
        Button button = new Button();
        button.Clicked += Button_Clicked;
        Content = new StackLayout
        {
            BackgroundColor = Colors.Brown,
            Children =
        {
            new Label { Text = "Hello, World!" },
            button  
        }
        };
    }
    private void Button_Clicked(object sender, EventArgs e)

    {
        var toast = new TMToast();
        toast.Show("icon.png","ToastMessage","Close");

    }
}