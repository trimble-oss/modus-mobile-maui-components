using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Views;
using DemoApp.Views.PopupSamples;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Popup.Services;

namespace DemoApp.ViewModels
{
    public partial class SamplePopupPageViewModel : ObservableObject
    {
        [RelayCommand]
        private void ArrowUpClicked(TMButton sender)
        {
            PopupService.Instance.PresentAsync(new SampleToolTip(sender, Trimble.Modus.Components.Enums.ModalPosition.Top));
        }
        [RelayCommand]
        private void ArrowDownClicked(TMButton sender)
        {
            PopupService.Instance.PresentAsync(new SampleToolTip(sender, Trimble.Modus.Components.Enums.ModalPosition.Bottom));
        }
        [RelayCommand]
        private void ArrowLeftClicked(TMButton sender)
        {
            PopupService.Instance.PresentAsync(new SampleToolTip(sender, Trimble.Modus.Components.Enums.ModalPosition.Left));
        }
        [RelayCommand]
        private void ArrowRightClicked(TMButton sender)
        {
            PopupService.Instance.PresentAsync(new SampleToolTip(sender, Trimble.Modus.Components.Enums.ModalPosition.Right));
        }
        [RelayCommand]
        private void CenterButtonClicked(TMButton sender)
        {
            PopupService.Instance.PresentAsync(new SampleCustomPopup());
        }
    }
}
