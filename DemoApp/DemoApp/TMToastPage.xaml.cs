
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
        withImage.Text = "With Image";
        withImage.Margin = new Thickness(5);
        withImage.Clicked += Button_Clicked;
        withoutImage.Text = "Without Image";
        withoutImage.Margin = new Thickness(5);
        withoutImage.Clicked += Button2_Clicked;
        Content = new StackLayout
        {
            Children =
        {
            withImage,
            withoutImage
        }
        };
    }

    private void Button2_Clicked(object sender, EventArgs e)
    {

        var toast = new TMToast();
        toast.Show("icon.png", "ToastMessage", ImageSource.FromFile("icon.png"));
    }

    private void Button_Clicked(object sender, EventArgs e)

    {
        var toast = new TMToast();
        toast.Show("icon.png","ToastMessage","Close");

    }
}