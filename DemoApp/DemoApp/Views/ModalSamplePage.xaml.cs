using DemoApp.Constant;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Views;

public partial class ModalSamplePage : ContentPage
{
    public ModalSamplePage()
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
                tmModal.AddPrimaryAction(PrimaryButtonTitle.Text, () =>
                {
                    Console.WriteLine("Primary Action");
                });
            }
            if (!string.IsNullOrEmpty(SecondaryButtonTitle.Text))
            {
                tmModal.AddSecondaryAction(SecondaryButtonTitle.Text, () =>
                {
                    Console.WriteLine("Secondary Action");
                });
            }
            if (!string.IsNullOrEmpty(TertiaryButtonTitle.Text))
            {
                tmModal.AddTertiaryAction(TertiaryButtonTitle.Text, () =>
                {
                    Console.WriteLine("Tertiary Action");
                });
            }
            if (!string.IsNullOrEmpty(DangerButtonTitleEntry.Text))
            {
                tmModal.AddDangerAction(DangerButtonTitleEntry.Text, () =>
                {
                    Console.WriteLine("Danger Action");
                });
            }
            tmModal.Show();
        }
        catch (Exception ex)
        {
            TMToast tMToast = new(ex.Message);
            tMToast.theme = ToastTheme.Danger;
            tMToast.Show();
        }
    }
}
