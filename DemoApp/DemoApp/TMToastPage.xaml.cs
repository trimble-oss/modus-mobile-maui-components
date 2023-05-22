
using Mopups;
using Mopups.Services;
using Trimble.Modus.Components.Controls.Toast;

namespace DemoApp;

public partial class TMToastPage : ContentPage
{
	public TMToastPage()
	{
        Button withImage = new Button();
        Button withoutImage = new Button();
        withImage.Text = "Toast";
        withImage.Margin = new Thickness(5);
        withImage.Clicked += Button_Clicked;
        Content = new StackLayout
        {
            Children =
        {
            withImage
        }
        };
    }

    private void Button_Clicked(object sender, EventArgs e)

    {
        var toast = new TMToast();
        toast.Show("ToastMessage This is a toast message This is a toast messageThis is a toast messageThis is a toast message", "icon.png","Close");

    }
}