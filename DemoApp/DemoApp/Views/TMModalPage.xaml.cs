using Trimble.Modus.Components;
using Trimble.Modus.Components.Popup.Services;

namespace DemoApp.Views;

public partial class TMModalPage : ContentPage
{
	private TMModal tmModal;

	public TMModalPage()
	{
		tmModal = new("");
        InitializeComponent();
	}

    private void ShowModalClicked(object sender, EventArgs e)
    {
        tmModal.Title = ModalTitle.Text;
        tmModal.Message = Message.Text;
        if (InputCheckBox.IsChecked)
        {
            tmModal.AddTextInput((tmInput) =>
            {
                tmInput.TitleText = "Input Text";
                tmInput.Placeholder = "Enter text here";
            });
        }
        if(IconCheckBox.IsChecked == true)
        {
            tmModal.TitleIcon = ImageSource.FromFile("placeholder.png");
        }
        else
        {
            tmModal.TitleIcon = null;
        }

        tmModal.FullWidthButton = FullWidthButtonCheckBox.IsChecked;
        tmModal.Show();
    }

    private void AddNewButton(object sender, EventArgs e)
    {
        try
        {
            tmModal.AddAction(ButtonTitleEntry.Text);
            ShowAlert(Constant.Constants.SuccessString, Constant.Constants.ButtonAddedString);
        }
        catch (Exception ex)
        {
            ShowAlert(Constant.Constants.ErrorString, ex.Message);
        }
    }

    private void AddDangerButton(object sender, EventArgs e)
    {
        try
        {
            tmModal.AddDangerButton(DangerButtonTitleEntry.Text);
            ShowAlert(Constant.Constants.SuccessString, Constant.Constants.ButtonAddedString);
        }
        catch (Exception ex)
        {
            ShowAlert( Constant.Constants.ErrorString, ex.Message);
        }
    }

    private void ShowAlert(string title, string message)
    {
        TMModal popupDialog = new(title, messageText: message);
        popupDialog.AddAction(Constant.Constants.OkString);
        popupDialog.Show();
    }
}