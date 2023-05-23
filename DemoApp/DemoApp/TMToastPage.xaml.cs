using Trimble.Modus.Components.Controls.Toast;
using Trimble.Modus.Components.Enums;

namespace DemoApp;

public partial class TMToastPage : ContentPage
{
    private ToastTheme toastTheme;
	public TMToastPage()
	{
      InitializeComponent();
      toastPicker.SelectedItem = ToastTheme.ToastBlue;
    }

    private void Button_Clicked(object sender, EventArgs e)

    {
        string rightIconText = IconText.Text;
        string toastMessage = Message.Text;
        var toast = new TMToast();
        if (toastMessage != null)
        {
            toast.Show(toastMessage, "lefticon.png", rightIconText, toastTheme,HandleEvent);
        }
        else
        {
            toast.Show("Enter Toast Message",null,null,ToastTheme.ToastRed);
        }
    }
    void HandleEvent(object sender, EventArgs e)
    {
        Console.WriteLine("Event handled");
    }

    private void ToastPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (toastPicker.SelectedItem is ToastTheme selectedColor)
        {
            toastTheme = selectedColor;
        }
    }
}