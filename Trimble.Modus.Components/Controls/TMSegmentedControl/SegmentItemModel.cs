using CommunityToolkit.Mvvm.ComponentModel;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components.Model
{
    public partial class SegmentItemModel : ObservableObject
    {
        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private bool _showSeparator;

        [ObservableProperty]
        private SegmentColorTheme _colorTheme;

        [ObservableProperty]
        private ImageSource _icon;

        [ObservableProperty]
        private int _itemIndex;

        [ObservableProperty]
        private SegmentedControlSize _size;
    }
}
