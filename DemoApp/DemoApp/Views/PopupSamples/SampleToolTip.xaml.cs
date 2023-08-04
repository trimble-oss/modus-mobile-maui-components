using DemoApp.ViewModels;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Popup.Pages;

namespace DemoApp.Views;

public partial class SampleToolTip : PopupPage
{
	public SampleToolTip(View anchorView, Trimble.Modus.Components.Enums.ModalPosition poisiton) : base(anchorView, poisiton)
	{
		InitializeComponent();
	}
}
