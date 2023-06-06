using Trimble.Modus.Components.Controls.Toast;
using Trimble.Modus.Components.Enums;

namespace DemoApp;

public partial class TMToastPage : ContentPage
{
    private ToastTheme toastTheme;
    public TMToastPage()
    {
        InitializeComponent();
        toastPicker.SelectedItem = ToastTheme.Default;
    }

    private void OnToastActionTapped(object sender, EventArgs e)
    {
        string rightIconText = IconText.Text;
        string toastMessage = Message.Text;
        var toast = new TMToast(toastMessage, rightIconText, HandleEvent)
        {
            theme = toastTheme,
        };
        toast.Show();
    }
    void HandleEvent()
    {
        Console.WriteLine("Event handled");
    }

    private void ToastPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (toastPicker.SelectedItem is ToastTheme selectedColor)
        {
            toastTheme = selectedColor;
        }
    }
}
