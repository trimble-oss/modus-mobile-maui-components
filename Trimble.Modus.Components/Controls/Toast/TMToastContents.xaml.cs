using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : Popup.Pages.PopupPage

{
    #region Private Properties

    private const int DELAYTIME = 5000;

    #endregion

    #region Public Properties

    public ImageSource LeftIconSource { get; set; }

    public string Message { get; set; }

    public string RightIconText { get; set; }

    public double ToastWidthRequest { get; set; }

    public Color ToastBackground { get; set; }

    public Color TextColor { get; set; }

    #endregion

    internal TMToastContents(string message, string actionButtonText, ToastTheme theme, Action action, bool isDismissable)
    {
        InitializeComponent();
        SetTheme(theme.ToString());
        PopupData(message, actionButtonText, action, isDismissable);
        BindingContext = this;
        CloseAfterDelay();
    }

    #region Private Methods
    private void SetTheme(String toastTheme)
    {
        ToastTheme theme = (ToastTheme)Enum.Parse(typeof(ToastTheme), toastTheme);
        switch (theme)
        {
            case ToastTheme.Dark:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromFile(ImageConstants.ToastDarkThemeIcon);
                TextColor = (Color)BaseComponent.colorsDictionary()["White"];
                break;

            case ToastTheme.Primary:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromFile(ImageConstants.BlueInfoIcon);
                TextColor = (Color)BaseComponent.colorsDictionary()["ToastTextBlue"];
                break;

            case ToastTheme.Secondary:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromFile(ImageConstants.SolidHelpIcon);
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Danger:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromFile(ImageConstants.ToastDangerIcon);
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Warning:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromFile(ImageConstants.WarningIcon);
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Success:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromFile(ImageConstants.ValidIcon);
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;
                
            default:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromFile(ImageConstants.GreyInfoIcon);
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;
        }

    }
    private void CloseButtonClicked(object sender, EventArgs e)

    {
        PopupService.Instance.RemovePageAsync(this, true);

    }
    public void CloseAfterDelay()
    {
        Task.Run(async () =>
        {
            await Task.Delay(DELAYTIME);
            await PopupService.Instance.RemovePageAsync(this, true);
        });
    }

    private void PopupData(string message, string actionButtonText, Action action, bool isDismissable)
    {
        RightIconText = actionButtonText;
        actionButton.Text = RightIconText;
        actionButton.TextColor = TextColor;
        actionButton.BackgroundColor = ToastBackground;

        if (string.IsNullOrEmpty(RightIconText))
        {
            if (isDismissable)
            {
                closeButton.Clicked += (sender, args) =>
                {
                    action?.Invoke();
                };

                if (ToastBackground.Equals(ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale)))
                {
                    closeButton.Source = ImageSource.FromFile(ImageConstants.ToastBlueCloseIcon);
                }
                else if (ToastBackground.Equals(ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray)))
                {
                    closeButton.Source = ImageSource.FromFile(ImageConstants.ToastWhiteCloseIcon);
                }
                else
                {
                    closeButton.Source = ImageSource.FromFile(ImageConstants.ToastBlackCloseIcon);
                }
                closeButton.IsVisible = true;
                actionButton.IsVisible = false;
            }
            else
            {
                closeButton.IsVisible = false;
                actionButton.IsVisible = false;
            }
        }
        else
        {
            closeButton.IsVisible = false;
            actionButton.IsVisible = true;
            actionButton.Clicked += (sender, args) =>
            {
                action?.Invoke();
            };
        }

        var idiom = DeviceInfo.Current.Idiom;
        setWidth(idiom);
        Message = message;
    }

    private void setWidth(DeviceIdiom idiom)
    {
        double minimumTabletWidth = 480;
        double maximumTabletWidthPercentage = 0.7;
        double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
        if (idiom == DeviceIdiom.Phone)
        {
            toastLayout.Padding = new Thickness(16, 0, 16, 10);

        }
        else if (idiom == DeviceIdiom.Tablet)
        {
            toastLayout.Padding = new Thickness(0, 0, 0, 10);
            toastLayout.MinimumWidthRequest = minimumTabletWidth;
            toastLayout.MaximumWidthRequest = deviceWidth * maximumTabletWidthPercentage;
            toastLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;

        }
    }
    #endregion
}
