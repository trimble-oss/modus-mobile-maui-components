using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;

namespace DemoApp.Views
{
    public partial class TMBadgesPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private BadgeSize _size;
        public TMBadgesPageViewModel()
        {
            Size = BadgeSize.Large;
        }
   
        [RelayCommand]
        private void SizeChanged(TMRadioButtonEventArgs e)
        {
            switch (e.RadioButtonIndex)
            {
                case 0:
                    Size = BadgeSize.Small;
                    break;
                case 2:
                    Size = BadgeSize.Large;
                    break;
                case 1:
                default:
                    Size = BadgeSize.Medium;
                    break;
            }
        }

    }
}
