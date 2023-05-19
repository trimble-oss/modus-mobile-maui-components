using Microsoft.Maui.Media;
using Mopups.Services;
using System.Xml.Linq;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : Mopups.Pages.PopupPage

{
    public ImageSource LeftIconSource { get; set; }
    public string Message { get; set; }

    public string RightIconText { get; set; }

    public ImageSource RightIconSource { get; set; }


    public TMToastContents(ImageSource leftIcon, string message, string rightIconText)
    {
        InitializeComponent();
        PopupData(leftIcon, message, rightIconText);
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

    private void PopupData(ImageSource leftIcon, string message, string rightIconText)
    {

        LeftIconSource = leftIcon;
        Message = GetWrappedLabelText(message, 3);
        RightIconText = rightIconText;
        TMButton rightIcon = new TMButton();
        rightIcon.Title = RightIconText;
        if(string.IsNullOrEmpty(RightIconText)) {
        rightIcon.IconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.input_valid_icon.png");
            rightIcon.Radius = 50;
            rightIcon.HorizontalOptions = LayoutOptions.Center;
        }
        rightIcon.BackgroundColor = this.BackgroundColor;
        rightIcon.Size = Enums.Size.XSmall;
        rightIcon.BorderColor = Colors.Transparent;
        rightIcon.Clicked += CloseButton_Clicked;
        contentLayout.Children.Add(rightIcon);
        

        /* if (rightIconSource == null)
         {
             Button rightIcon = new Button();
             rightIcon.Text = rightIconText;
             rightIcon.Padding = new Thickness(0);
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
         }*/
    }
    string GetWrappedLabelText(string text, int maxLines)
    {
        const int maxCharactersPerLine = 20; // Adjust this value according to your needs
        const string ellipsis = "...";

        string[] words = text.Split(' ');
        int lineCount = 1;
        int currentLineLength = 0;
        string wrappedText = string.Empty;

        foreach (string word in words)
        {
            if (currentLineLength + word.Length > maxCharactersPerLine)
            {
                if (lineCount >= maxLines)
                {
                    if (lineCount > 1) // Add line break only if more than one line
                    {
                        wrappedText += Environment.NewLine;
                    }
                    wrappedText += ellipsis;
                    break;
                }
                wrappedText += Environment.NewLine;
                lineCount++;
                currentLineLength = 0;
            }

            wrappedText += word + " ";
            currentLineLength += word.Length + 1;
        }

        return wrappedText.TrimEnd();
    }

}