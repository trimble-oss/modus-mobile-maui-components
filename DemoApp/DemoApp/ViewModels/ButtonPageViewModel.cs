using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Constant;
using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public partial class ButtonSamplePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private Trimble.Modus.Components.Enums.Size _selectedFontSize;
        [ObservableProperty]
        private ButtonStyle _selectedButtonStyle;
        [ObservableProperty]
        private bool _isDisabled;
        [ObservableProperty]
        private bool _isLoading;
        [ObservableProperty]
        private string _selectedImageOption;
        [ObservableProperty]
        private string _leftIconSource;
        [ObservableProperty]
        private string _rightIconSource;
        [ObservableProperty]
        private LayoutOptions _fullWidthAlignment;

        public ButtonSamplePageViewModel()
        {
            SelectedFontSize = Trimble.Modus.Components.Enums.Size.Default;
            SelectedButtonStyle = ButtonStyle.Fill;
            SelectedImageOption = "None";
            FullWidthAlignment = LayoutOptions.Start;
        }
        [RelayCommand]
        private void Clicked(object obj)
        {
            Console.WriteLine(obj.ToString());
        }
        partial void OnSelectedImageOptionChanged(string value)
        {
            switch (value)
            {
                case "Left":
                    LeftIconSource = ImageConstants.GalleryIcon;
                    RightIconSource = null;
                    break;
                case "Right":
                    LeftIconSource = null;
                    RightIconSource = ImageConstants.GalleryIcon;
                    break;
                case "Both":
                    LeftIconSource = ImageConstants.GalleryIcon;
                    RightIconSource = ImageConstants.GalleryIcon;
                    break;
                default:
                case "None":
                    LeftIconSource = null;
                    RightIconSource = null;
                    break;
            }
        }
    }
}
