using DemoApp.Constant;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Controls.Toast;
using Trimble.Modus.Components.Enums;

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
        TMModal tmModal = new(string.IsNullOrEmpty(ModalTitle.Text) ? "" : ModalTitle.Text);
        tmModal.TitleIcon = IconCheckBox.IsChecked ? ImageSource.FromFile(ImageConstants.ModusPlaceholderImage) : null;
        tmModal.FullWidthButton = FullWidthButtonCheckBox.IsChecked;
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
        try
        {
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
            TMToast tMToast = new("Error", ex.Message);
            tMToast.theme = ToastTheme.Danger;
            tMToast.Show();
        }
    }
}
