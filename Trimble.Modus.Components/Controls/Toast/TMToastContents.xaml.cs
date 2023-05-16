using Mopups.Services;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : Mopups.Pages.PopupPage

{
    public ImageSource LeftIconSource { get;  set; }
    public string Message { get; set; }

    public string RightIconText { get; set; }

    public ImageSource RightIconSource { get; set; }


    public TMToastContents(ImageSource leftIcon ,string message,string rightIconText)
	{
		InitializeComponent();
        PopupData(leftIcon, message, rightIconText,null, contentLayout);
        BindingContext = this;
        Close();
    }
    public TMToastContents(ImageSource leftIcon, string message, ImageSource rightIcon)
    {
        InitializeComponent();
        PopupData(leftIcon, message, null, rightIcon, contentLayout);
        BindingContext = this;
        Close();
    }
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();
    }
    public void Close()
    {
        Task.Run(async () => {
            await Task.Delay(5000);
            await MopupService.Instance.PopAsync();
        });
    }

    private void PopupData(ImageSource leftIcon, string message, string rightIconText, ImageSource rightIconSource, StackLayout contentLayout)
    {

        LeftIconSource = leftIcon;
        Message = message;
        
        if (rightIconSource == null)
        {
            Button rightIcon = new Button();
            rightIcon.Text = rightIconText;
            rightIcon.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
            rightIcon.TextColor = Colors.White;
            rightIcon.Margin = new Thickness(8, 0, 16, 0);
            rightIcon.FontSize = 14;
            rightIcon.HorizontalOptions = LayoutOptions.End;
            rightIcon.Clicked += CloseButton_Clicked;
            contentLayout.Children.Add(rightIcon);
        }
        else
        {
            ImageButton rightIcon = new ImageButton();
            rightIcon.Source = rightIconSource;
            rightIcon.WidthRequest = 16;
            rightIcon.HeightRequest = 16;
            rightIcon.Margin = new Thickness(8, 0, 16, 0);
            rightIcon.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
            rightIcon.HorizontalOptions = LayoutOptions.End;
            rightIcon.VerticalOptions = LayoutOptions.Center;
            rightIcon.Clicked += CloseButton_Clicked;
            contentLayout.Children.Add(rightIcon);
        }
    } 
}  