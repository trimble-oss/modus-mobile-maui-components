using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;

namespace DemoApp.ViewModels
{
    public partial class SwitchSamplePageViewModel : ObservableObject
    {
        public SwitchSamplePageViewModel()
        {

        }
        [RelayCommand]
        private void Switch(object e)
        {
            Console.WriteLine("ToggledCommand " + ((TMSwitchEventArgs)e).Value);
        }
    }
}
