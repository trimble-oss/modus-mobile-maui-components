using CommunityToolkit.Mvvm.ComponentModel;
using DemoApp.Constant;
using System.Collections.ObjectModel;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
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

        public SegmentedControlViewModel()
        {
            _segmentItems = new() { "Item 1", "item 2", "Item 3", "Item 4" };
            _segmentImageItems = new()
            {
                ImageSource.FromFile(ImageConstants.GalleryIcon),
                ImageSource.FromFile(ImageConstants.ModusPlaceholderImage),
                ImageSource.FromFile(ImageConstants.AccountIcon)
            };
        }
    }
}
