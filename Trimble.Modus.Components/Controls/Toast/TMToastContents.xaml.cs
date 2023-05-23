using Microsoft.Maui.Platform;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : Popup.Pages.PopupPage

{
    public ImageSource LeftIconSource { get; set; }
    public string Message { get; set; }

    public string RightIconText { get; set; }

    public ImageSource RightIconSource { get; set; }

    public double ToastWidthRequest { get; set; }

    public Color ToastTheme { get; set; }

    public Color TextColor { get; set; }


    PopupNavigation popupNavigation;

    public TMToastContents(ImageSource leftIcon, string message, string rightIconText,Object popupNavigation,ToastTheme theme)
    {
        InitializeComponent();
       
        setTheme(theme.ToString());
        this.popupNavigation = (PopupNavigation) popupNavigation;
        PopupData(leftIcon, message, rightIconText);
        BindingContext = this;
        Close();
    }

    private void setTheme(String theme)
    {
        ToastTheme = (Color)BaseComponent.colorsDictionary()[theme];
        if (string.Equals(theme, "ToastBlue"))
        {
            TextColor = (Color)BaseComponent.colorsDictionary()["ToastTextBlue"];


        }
        if (string.Equals(theme, "ToastBlack"))
        {
            TextColor = (Color)BaseComponent.colorsDictionary()["ToastWhite"];
        }
        else
        {
            TextColor = (Color)BaseComponent.colorsDictionary()["ToastBlack"];
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
            await Task.Delay(5000);
            await popupNavigation.RemovePageAsync(this, true);
         });
    }

    private void PopupData(ImageSource leftIcon, string message, string rightIconText)
    {
       
        LeftIconSource = leftIcon;

        RightIconText = rightIconText;
        TMButton rightIcon = new TMButton();
        rightIcon.Title = RightIconText;
        rightIcon.TextColor = TextColor;
        if (string.IsNullOrEmpty(RightIconText)) {
            if(ToastTheme.Equals((Color)BaseComponent.colorsDictionary()["ToastBlue"]))
            {
                rightIcon.IconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.blue_close_icon.png");
            }
            else if (ToastTheme.Equals((Color)BaseComponent.colorsDictionary()["ToastBlack"]))
            {
                rightIcon.IconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.white_close_icon.png");
            }
            else
            {
                rightIcon.IconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.black_close_icon.png");
            }
        }
        rightIcon.VerticalOptions = LayoutOptions.Center;
        rightIcon.HorizontalOptions = LayoutOptions.End;
        rightIcon.BackgroundColor = this.BackgroundColor;

        rightIcon.Size = Enums.Size.XSmall;
        rightIcon._iconWidth = 16;
        rightIcon._iconHeight = 16;

        rightIcon.BorderColor = Colors.Transparent;
        rightIcon.Clicked += CloseButton_Clicked;
        contentLayout.Children.Add(rightIcon);

        var idiom = Device.Idiom;

        setWidth(rightIcon,idiom);
     
        Message = GetWrappedLabelText(message,idiom);
    }

    private void setWidth(TMButton rightIcon,TargetIdiom idiom)
    {
        double minimumTabletWidth = 480; 
        double maximumTabletWidthPercentage = 0.7;
        double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
    
        if (idiom == TargetIdiom.Phone)
        {
            toastLayout.Padding = new Thickness(16,0,16,10);
            rightIcon.Size = Enums.Size.XSmall;
        }
        else if (idiom == TargetIdiom.Tablet)
        {
            toastLayout.Padding = new Thickness(0, 0, 0, 10);
            toastLayout.MinimumWidthRequest = minimumTabletWidth;
            toastLayout.MaximumWidthRequest = deviceWidth * maximumTabletWidthPercentage;
            toastLayout.HorizontalOptions = LayoutOptions.CenterAndExpand;
            rightIcon.Size = Enums.Size.Large;
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
