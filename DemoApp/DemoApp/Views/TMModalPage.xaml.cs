using Trimble.Modus.Components;
using Trimble.Modus.Components.Popup.Services;

namespace DemoApp.Views;

public partial class TMModalPage : ContentPage
{
	private TMModal _tmModal;
    private bool _inputAdded = false;

	public TMModalPage()
	{
        InitializeComponent();
        _tmModal = new("");
        ModalTitle.TextChanged += OnModalTitleChanged;
	}

    /// <summary>
    /// Initialize new modal with title
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnModalTitleChanged(object sender, TextChangedEventArgs e)
    {
        _tmModal = new( string.IsNullOrEmpty(e.NewTextValue)? "" : e.NewTextValue );
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
            _tmModal.TitleIcon = IconCheckBox.IsChecked ? ImageSource.FromFile(Constant.ImageConstants.ModusPlaceholderImage) : null;
            _tmModal.Message = Message.Text;
            _tmModal.FullWidthButton = FullWidthButtonCheckBox.IsChecked;
            if (InputCheckBox.IsChecked && !_inputAdded)
            {
                _tmModal.AddTextInput((tmInput) =>
                {
                    tmInput.TitleText = Constant.Constants.InputText;
                    tmInput.Placeholder = Constant.Constants.PlaceholderText;
                });
                _inputAdded = true;
            }

            _tmModal.Show();
        }
        catch (Exception ex)
        {
            ShowAlert(Constant.Constants.ErrorString, ex.Message);
        }

    }

    /// <summary>
    /// Add primary button to the modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddPrimaryButton(object sender, EventArgs e)
    {
        try
        {
            _tmModal.AddPrimaryAction(PrimaryButtonTitle.Text);
            ShowAlert(Constant.Constants.SuccessString, Constant.Constants.ButtonAddedString);
        }
        catch (Exception ex)
        {
            ShowAlert(Constant.Constants.ErrorString, ex.Message);
        }
    }

    /// <summary>
    /// Add secondary button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddSecondaryButton(object sender, EventArgs e)
    {
        try
        {
            _tmModal.AddSecondaryAction(SecondaryButtonTitle.Text);
            ShowAlert(Constant.Constants.SuccessString, Constant.Constants.ButtonAddedString);
        }
        catch (Exception ex) 
        {
            ShowAlert(Constant.Constants.ErrorString, ex.Message);
        }
    }

    /// <summary>
    /// Add tertiary button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddTertiaryButton(object sender, EventArgs e)
    {
        try
        {
            _tmModal.AddTertiaryAction(TertiaryButtonTitle.Text);
            ShowAlert(Constant.Constants.SuccessString, Constant.Constants.ButtonAddedString);
        }
        catch (Exception ex)
        {
            ShowAlert(Constant.Constants.ErrorString, ex.Message);
        }
    }

    /// <summary>
    /// Add danger button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddDangerButton(object sender, EventArgs e)
    {
        try
        {
            _tmModal.AddDangerAction(DangerButtonTitleEntry.Text);
            ShowAlert(Constant.Constants.SuccessString, Constant.Constants.ButtonAddedString);
        }
        catch (Exception ex)
        {
            ShowAlert(Constant.Constants.ErrorString, ex.Message);
        }
    }

    /// <summary>
    /// Display alert messages
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    private void ShowAlert(string title, string message)
    {
        TMModal popupDialog = new(title, messageText: message);
        popupDialog.AddPrimaryAction(Constant.Constants.OkString);
        popupDialog.Show();
    }

    private void InputCheckboxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value)
        {
            _tmModal = new(string.IsNullOrEmpty(ModalTitle.Text) ? "" : ModalTitle.Text);
            _inputAdded = false;
        }
    }
}