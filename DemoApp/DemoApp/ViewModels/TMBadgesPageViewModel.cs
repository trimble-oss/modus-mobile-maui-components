using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Views
{
    public partial class BadgeSamplePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private BadgeSize _badgeSize;
        [ObservableProperty]
        private string _primaryBadgeValue = "Primary";
        [ObservableProperty]
        private BadgeColor _primaryBadgeColor = BadgeColor.Primary;
        [ObservableProperty]
        private BadgeShape _primaryBadgeShape = BadgeShape.Rectangle;

        public BadgeSamplePageViewModel()
        {
            BadgeSize = BadgeSize.Medium;
        }
   
        [RelayCommand]
        private void BadgeSizeChanged(TMRadioButtonEventArgs e)
        {
            switch (e.RadioButtonIndex)
            {
                case 0:
                    BadgeSize = BadgeSize.Small;
                    break;
                case 2:
                    BadgeSize = BadgeSize.Large;
                    break;
                case 1:
                default:
                    BadgeSize = BadgeSize.Medium;
                    break;
            }
        }

    }
}
