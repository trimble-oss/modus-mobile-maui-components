using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Constant;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public class DemoClass{
        public string Text { get; set; }

        public ImageSource Image { get; set; }
        public DemoClass(string text, ImageSource image)
        {
            Text = text;
            Image= image;
        }
    }
    internal partial class SegmentedControlViewModel : ObservableObject
    {
        [ObservableProperty]
        private SegmentedControlSize _size;

        [ObservableProperty]
        private bool _roundedCorners;

        [ObservableProperty]
        private SegmentColorTheme _segmentTheme = SegmentColorTheme.Primary;

        [ObservableProperty]
        private bool _isEnabled = true;

        [ObservableProperty]
        private ObservableCollection<string> _segmentItems = new ObservableCollection<string>();

        [ObservableProperty]
        private ObservableCollection<ImageSource> _segmentImageItems =
            new ObservableCollection<ImageSource>();
        [ObservableProperty]
        private ObservableCollection<DemoClass> _segmentDemoItems =
            new ObservableCollection<DemoClass>();
        [ObservableProperty]
        private bool roundedCornersSwitch,enabledSwitch,secondaryThemeSwitch;
        public SegmentedControlViewModel()
        {
            _segmentItems = new() { "One", "Two", "Three", "Four" };
            _segmentImageItems = new()
            {
                ImageSource.FromFile(ImageConstants.GalleryIcon),
                ImageSource.FromFile(ImageConstants.ModusPlaceholderImage),
                ImageSource.FromFile(ImageConstants.AccountIcon)
            };
            _segmentDemoItems = new ObservableCollection<DemoClass>()
            {
                new DemoClass("One",ImageSource.FromFile(ImageConstants.GalleryIcon)),
                new DemoClass("Two",ImageSource.FromFile(ImageConstants.ModusPlaceholderImage)),
                new DemoClass("Three",ImageSource.FromFile(ImageConstants.AccountIcon))
            };
            RoundedCornersSwitch = false;
            EnabledSwitch = true;
        }
        [RelayCommand]
        private void RadioButton(TMRadioButtonEventArgs e)
        {
            Size = e.Value switch
            {
                "Small" => SegmentedControlSize.Small,
                "Medium" => SegmentedControlSize.Medium,
                "Large" => SegmentedControlSize.Large,
                "XLarge" => SegmentedControlSize.XLarge,
                _ => SegmentedControlSize.Small,
            };
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            switch (e.PropertyName)
            {
                case "RoundedCornersSwitch":
                    RoundedCorners = RoundedCornersSwitch;
                    break;

                case "SecondaryThemeSwitch":
                    SegmentTheme = SecondaryThemeSwitch ? SegmentColorTheme.Secondary : SegmentColorTheme.Primary;
                    break;

                case "EnabledSwitch":
                    IsEnabled = EnabledSwitch;
                    break;
                default:
                    break;
            }
        }
    }
}
