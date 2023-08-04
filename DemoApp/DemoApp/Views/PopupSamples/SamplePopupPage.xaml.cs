using DemoApp.Views.PopupSamples;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Popup.Services;

namespace DemoApp.Views;

public partial class SamplePopupPage : ContentPage
{
	public SamplePopupPage()
	{
		InitializeComponent();
	}

    private void ArrowUpClicked(object sender, EventArgs e)
    {
        PopupService.Instance.PresentAsync(new SampleToolTip((sender as ImageButton), Trimble.Modus.Components.Enums.ModalPosition.Top));
    }

    private void ArrowDownClicked(object sender, EventArgs e)
    {
        PopupService.Instance.PresentAsync(new SampleToolTip((sender as ImageButton), Trimble.Modus.Components.Enums.ModalPosition.Bottom));
    }

    private void ArrowLeftClicked(object sender, EventArgs e)
    {
        PopupService.Instance.PresentAsync(new SampleToolTip((sender as ImageButton), Trimble.Modus.Components.Enums.ModalPosition.Left));
    }

    private void ArrowRightClicked(object sender, EventArgs e)
    {
        PopupService.Instance.PresentAsync(new SampleToolTip((sender as ImageButton), Trimble.Modus.Components.Enums.ModalPosition.Right));
    }
    private void CenterButtonClicked(object sender, EventArgs e)
    {
        PopupService.Instance.PresentAsync(new SampleCustomPopup());
    }
}
