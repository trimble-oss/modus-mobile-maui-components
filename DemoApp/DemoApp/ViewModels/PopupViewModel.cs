using CommunityToolkit.Mvvm.ComponentModel;
namespace DemoApp.ViewModels
{
    public partial class PopupViewModel : ObservableObject
    {
        [ObservableProperty]
        List<string> _upcomingControls;

        public PopupViewModel()
        {
            UpcomingControls = new List<string>()
            { "Accordians", "Badges", "Chips",
                "Switches", "Progress Bar", "Top Navbars", "Tabs",
                "Sliders", "Scrollbards", "Messages", "Alerts", "Dropdown"  };
        }
    }
}
