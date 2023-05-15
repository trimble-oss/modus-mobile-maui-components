using Microsoft.Maui.Controls.Compatibility;
using Mopups.Services;

namespace Trimble.Modus.Components.Controls.Toast;

public partial class TMToastContents : Mopups.Pages.PopupPage

{ 
    public TMToastContents(Image leftIcon ,string message,string rightIconText)
	{
		InitializeComponent();
        BindingContext = new PopupData { LeftIcon = leftIcon  , Message = message ,RightIconText = rightIconText };
       
    }
    public TMToastContents(Image leftIcon, string message, Image rightIcon)
    {
        InitializeComponent();
        
        BindingContext = new PopupData { LeftIcon = leftIcon ,Message = message, RightIcon = rightIcon };

    }
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();
    }
    public async Task Close(int time)
    { 
        await Task.Delay(30000);
        await MopupService.Instance.PopAsync();
       
    }
    public class PopupData
    {
        public string Message { get; set; }

        public string RightIconText { get; set; }
        public Image LeftIcon { get; set; }

        public Image RightIcon { get; set; }
    }
}  