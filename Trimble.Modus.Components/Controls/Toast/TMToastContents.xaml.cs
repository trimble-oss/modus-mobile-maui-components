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

    public ImageSource RightIconSource { get; set; }

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
            case ToastTheme.Default:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Dark:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["White"];
                break;

            case ToastTheme.Primary:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["ToastTextBlue"];
                break;

            case ToastTheme.Secondary:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Danger:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Warning:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            case ToastTheme.Success:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["Black"];
                break;

            default:
                ToastBackground = (Color)BaseComponent.colorsDictionary()[toastTheme];
                LeftIconSource =  ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
                TextColor = (Color)BaseComponent.colorsDictionary()["ToastTextBlue"];
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

 
        var button = new Button();
        button.Text = RightIconText;
        button.TextColor = TextColor;
        button.Padding = new Thickness(8, 0, 16, 0);
        button.BackgroundColor = ToastBackground;
        button.VerticalOptions = LayoutOptions.Center;
        button.HorizontalOptions = LayoutOptions.Center;
        
        var imageButton = new ImageButton
        {
            WidthRequest = 16,
            HeightRequest = 16,
            Margin = new Thickness(8,0,16,0),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            };

           
        
        imageButton.Clicked += CloseButton_Clicked;
        button.Clicked += CloseButton_Clicked;
        if (string.IsNullOrEmpty(RightIconText)) {
             imageButton.Clicked += (sender, args) => {
                action?.Invoke();
            };

            if (ToastBackground.Equals((Color)BaseComponent.colorsDictionary()["Primary"]))
            {
                imageButton.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.blue_close_icon.png");
            }
            else if (ToastBackground.Equals((Color)BaseComponent.colorsDictionary()["Dark"]))
            {
                imageButton.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.white_close_icon.png");
            }
            else
            {
                imageButton.Source = ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
            }
            contentLayout.Children.Add(imageButton);
        }
        else
        {   
            button.Clicked += (sender, args) => {
                action?.Invoke();
            };
            contentLayout.Children.Add(button);

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
