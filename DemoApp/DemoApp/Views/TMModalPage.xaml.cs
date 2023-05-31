using Trimble.Modus.Components;
using Trimble.Modus.Components.Popup.Services;
using static System.Net.Mime.MediaTypeNames;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace DemoApp.Views;

public partial class TMModalPage : ContentPage
{
	public TMModalPage()
	{
        InitializeComponent();
	}

    /// <summary>
    /// Construct and display the modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ShowModalClicked(object sender, EventArgs e)
    {
        try
        {
            TMModal tmModal = new(string.IsNullOrEmpty(ModalTitle.Text) ? "" : ModalTitle.Text);
            tmModal.TitleIcon = IconCheckBox.IsChecked ? ImageSource.FromFile(Constant.ImageConstants.ModusPlaceholderImage) : null;
            tmModal.Message = Message.Text;
            tmModal.FullWidthButton = FullWidthButtonCheckBox.IsChecked;
            if (InputCheckBox.IsChecked)
            {
                tmModal.AddTextInput((tmInput) =>
                {
                    tmInput.TitleText = "Input Text";
                    tmInput.Placeholder = "Enter text here";
                });
            }

            if (!string.IsNullOrEmpty(PrimaryButtonTitle.Text))
            {
                tmModal.AddPrimaryAction(PrimaryButtonTitle.Text);
            }
            if (!string.IsNullOrEmpty(SecondaryButtonTitle.Text))
            {
                tmModal.AddSecondaryAction(SecondaryButtonTitle.Text);
            }
            if (!string.IsNullOrEmpty(TertiaryButtonTitle.Text))
            {
                tmModal.AddTertiaryAction(TertiaryButtonTitle.Text);
            }
            if (!string.IsNullOrEmpty(DangerButtonTitleEntry.Text))
            {
                tmModal.AddDangerAction(DangerButtonTitleEntry.Text);
            }

            tmModal.Show();
        }
        catch (Exception ex)
        {
            Toast.Make(ex.Message).Show();
        }
    }
}