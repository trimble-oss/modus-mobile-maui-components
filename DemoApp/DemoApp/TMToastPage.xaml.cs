
using Mopups;
using Mopups.Services;
using Trimble.Modus.Components.Controls.Toast;

namespace DemoApp;

public partial class TMToastPage : ContentPage
{
	public TMToastPage()
	{
        Button green = new Button();
        Button blue = new Button();
        Button black = new Button();
        green.Text = "Green";
        green.Margin = new Thickness(5);
        green.Clicked += Green_Clicked;
        blue.Text = "Blue";
        blue.Margin = new Thickness(5);
        blue.Clicked += Blue_Clicked;
        black.Text = "Black";
        black.Margin = new Thickness(5);
        black.Clicked += Black_Clicked;
        Content = new StackLayout
        {
            Children =
        {
            green, blue,black,
        }
        };
    }

    private void Green_Clicked(object sender, EventArgs e)

    {
        var toast = new TMToast();
        toast.Show("ToastMessage This is a toast message This is a toast messageThis is a toast messageThis is a toast message", "icon.png","Close",Trimble.Modus.Components.Enums.ToastTheme.ToastGreen);

    }
    private void Blue_Clicked(object sender, EventArgs e)

    {
        var toast = new TMToast();
        toast.Show("ToastMessage This is a toast message This is a toast messageThis is a toast messageThis is a toast message", "icon.png", "Close", Trimble.Modus.Components.Enums.ToastTheme.ToastBlue);

    }
    private void Black_Clicked(object sender, EventArgs e)

    {
        var toast = new TMToast();
        toast.Show("ToastMessage This is a toast message This is a toast messageThis is a toast messageThis is a toast message", "icon.png", "Close", Trimble.Modus.Components.Enums.ToastTheme.ToastBlack);

    }
}