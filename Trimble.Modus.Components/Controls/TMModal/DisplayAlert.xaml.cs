using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components;

public partial class DisplayAlert
{
    private bool closeOnDisappearing = true;
    TaskCompletionSource<bool> buttonTappedTask = new TaskCompletionSource<bool>();
    /// <summary>
    /// Gets or sets the text for the title label in the control
    /// </summary>
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(DisplayAlert));

    /// <summary>
    /// Gets or sets the text for the title label in the control
    /// </summary>
    public static readonly BindableProperty MessageTextProperty =
        BindableProperty.Create(nameof(Message), typeof(string), typeof(DisplayAlert));

    /// <summary>
    /// Gets or sets the text for the title label in the control
    /// </summary>
    public static readonly BindableProperty PrimaryTextProperty =
        BindableProperty.Create(nameof(PrimaryText), typeof(string), typeof(DisplayAlert));

    /// <summary>
    /// Gets or sets the text for the title label in the control
    /// </summary>
    public static readonly BindableProperty SecondaryTextProperty =
        BindableProperty.Create(nameof(SecondaryText), typeof(string), typeof(DisplayAlert));


    /// <summary>
    /// Gets or sets title text
    /// </summary>
    public new string Title
    {
        get { return (string)GetValue(TitleTextProperty); }
        set { SetValue(TitleTextProperty, value); }
    }

    public string Message
    {
        get { return (string)GetValue(MessageTextProperty); }
        set { SetValue(MessageTextProperty, value); }
    }

    public string PrimaryText
    {
        get { return (string)GetValue(PrimaryTextProperty); }
        set { SetValue(PrimaryTextProperty, value); }
    }

    public string SecondaryText
    {
        get { return (string)GetValue(SecondaryTextProperty); }
        set { SetValue(SecondaryTextProperty, value); }
    }

    /// <summary>
    /// Occurs when primary button is clicked
    /// </summary>
    public event EventHandler OnPrimaryButtonClicked
    {
        add
        {
            PrimaryButton.Clicked += value;
        }

        remove
        {
            PrimaryButton.Clicked -= value;
        }
    }

    /// <summary>
    /// Occurs when secondary button is clicked
    /// </summary>
    public event EventHandler OnSecondaryButtonClicked
    {
        add
        {
            SecondaryButton.Clicked += value;
        }

        remove
        {
            SecondaryButton.Clicked -= value;
        }
    }


    public DisplayAlert(string title, string message = "", string primaryButtonText = "Okay", string secondaryText = "")
    {
        InitializeComponent();
        BindingContext = this;
        Title = title;
        Message = message;
        PrimaryText = primaryButtonText;
        SecondaryText = secondaryText;
        PrimaryButton.Clicked += ClosePopup;
        SecondaryButton.Clicked += ClosePopup;
        Disappearing += OnDisappearing;
    }

    private void OnDisappearing(object sender, EventArgs e)
    {
        if (closeOnDisappearing)
            ClosePopup(sender, e);
    }

    public Task<bool> Show()
    {
        PopupService.Instance.PresentAsync(this);
        return buttonTappedTask.Task;
    }

    private void ClosePopup(object sender, EventArgs e)
    {
        closeOnDisappearing = false;
        PrimaryButton.Clicked -= ClosePopup;
        SecondaryButton.Clicked -= ClosePopup;
        Task.Run(async () =>
        {
            await PopupService.Instance.DismissAsync();
            buttonTappedTask.SetResult(sender == PrimaryButton);
        });
    }
}
