using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : Popup.Pages.PopupPage

{
    private const int DELAYTIME = 5000;

    public ImageSource LeftIconSource { get; set; }

    public string Message { get; set; }

    public string RightIconText { get; set; }

    public double ToastWidthRequest { get; set; }

    public Color ToastBackground { get; set; }

    public Color TextColor { get; set; }


    PopupNavigation popupNavigation;

    internal TMToastContents(string message, string actionButtonText, Object popupNavigation,ToastTheme theme,Action action )
    {
  
        InitializeComponent();
        SetTheme(theme.ToString());
        this.popupNavigation = (PopupNavigation) popupNavigation;
        PopupData(message, actionButtonText, action);
        BindingContext = this;
        Close();
    }

    private void SetTheme(String toastTheme)
    {
        ToastTheme theme = (ToastTheme)Enum.Parse(typeof(ToastTheme), toastTheme);
        switch (theme)
        {
            case ToastTheme.Dark:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.dark.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["White"];
                break;

            case ToastTheme.Primary:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.primary.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["ToastTextBlue"];
                break;

            case ToastTheme.Secondary:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.secondary.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Danger:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.danger.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Warning:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.warning.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Success:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.input_valid_icon.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Default:
            default:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.default.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;
        }
      
    }

    public TMToastContents()
    {
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
         popupNavigation.RemovePageAsync(this, true);
       
    }
    public void Close()
    {
        Task.Run(async () => {
            await Task.Delay(DELAYTIME);
            await popupNavigation.RemovePageAsync(this, true);
         });
    }

    private void PopupData(string message, string actionButtonText, Action action)
    {
      
        RightIconText = actionButtonText;
 
        actionButton.Text = RightIconText;
        actionButton.TextColor = TextColor;
        actionButton.BackgroundColor = ToastBackground;

        if (string.IsNullOrEmpty(RightIconText)) {
             closeButton.Clicked += (sender, args) => {
                action?.Invoke();
            };

            if (ToastBackground.Equals((Color)BaseComponent.colorsDictionary()["Primary"]))
            {
                closeButton.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.blue_close_icon.png");
            }
            else if (ToastBackground.Equals((Color)BaseComponent.colorsDictionary()["Dark"]))
            {
                closeButton.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.white_close_icon.png");
            }
            else
            {
                closeButton.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
            }
            closeButton.IsVisible = true;
            actionButton.IsVisible = false;
        }
        else
        {
            closeButton.IsVisible = false;
            actionButton.IsVisible = true;
            actionButton.Clicked += (sender, args) => {
                action?.Invoke();
            };
        }
     
        var idiom = Device.Idiom;
        setWidth(idiom);
        Message = GetWrappedLabelText(message,idiom);
    }

    private void setWidth(TargetIdiom idiom)
    {
        double minimumTabletWidth = 480; 
        double maximumTabletWidthPercentage = 0.7;
        double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
        if (idiom == TargetIdiom.Phone)
        {
            toastLayout.Padding = new Thickness(16,0,16,10);
        
        }
        else if (idiom == TargetIdiom.Tablet)
        {
            toastLayout.Padding = new Thickness(0, 0, 0, 10);
            toastLayout.MinimumWidthRequest = minimumTabletWidth;
            toastLayout.MaximumWidthRequest = deviceWidth * maximumTabletWidthPercentage;
            toastLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;
      
        }

    }
    private string GetWrappedLabelText(string text, TargetIdiom idiom)
    {
        const string ellipsis = "...";
        if (idiom == TargetIdiom.Phone)
        {
            if (text.Length > 106)
            {
                text = text.Substring(0, 106) + ellipsis;
            }
        }
        else if (idiom == TargetIdiom.Tablet)
        {
            if (text.Length > 206)
            {
                text = text.Substring(0, 206) + ellipsis;
            }
        }
        return text;
    }



}
