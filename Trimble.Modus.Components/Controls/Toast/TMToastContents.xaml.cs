using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : Popup.Pages.PopupPage

{
    public ImageSource LeftIconSource { get; set; }
    public string Message { get; set; }

    public string RightIconText { get; set; }

    public ImageSource RightIconSource { get; set; }

    public double ToastWidthRequest { get; set; }

    public double labelWidth;

    PopupNavigation popupNavigation;

    public TMToastContents(ImageSource leftIcon, string message, string rightIconText,Object popupNavigation)
    {
        InitializeComponent();
        this.popupNavigation = (PopupNavigation) popupNavigation;
        PopupData(leftIcon, message, rightIconText);
        BindingContext = this;
        Close();
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

        string value = "Cricket, often hailed as Cricket, often hailed as the gentleman's game, is a popular sport played with zeal and passion across the globe. Originating in England, it has become a global phenomenon captivating millions of fans. The game is played between two team ";

        RightIconText = null;// rightIconText;
        TMButton rightIcon = new TMButton();
        rightIcon.Title = RightIconText;
        rightIcon.Radius = 50;
        if (string.IsNullOrEmpty(RightIconText)) {
        rightIcon.IconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.vct.png");
           
            
        }
        rightIcon.VerticalOptions = LayoutOptions.Center;
        rightIcon.HorizontalOptions = LayoutOptions.End;
        rightIcon.BackgroundColor = this.BackgroundColor;
        rightIcon.Size = Enums.Size.XSmall;
        rightIcon.BorderColor = Colors.Transparent;
        rightIcon.Clicked += CloseButton_Clicked;
        contentLayout.Children.Add(rightIcon);

        
        setWidth(rightIcon);
     
        Message = GetWrappedLabelText(value, labelWidth);
    }

    private void setWidth(TMButton rightIcon)
    {
        double minimumTabletWidth = 480; 
        double maximumTabletWidthPercentage = 0.7;
        double deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
        var idiom = Device.Idiom;

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
            rightIcon.Size = Enums.Size.Large;
        }

    }
    private string GetWrappedLabelText(string text, double labelWidth)
    {
        const string ellipsis = "...";

        if (text.Length > 226)
        {
            text = text.Substring(0, 226) + ellipsis;
        }
     
        return text;
    }



}
