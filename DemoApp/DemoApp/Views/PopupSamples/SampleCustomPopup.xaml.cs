using DemoApp.ViewModels;
using Trimble.Modus.Components;

namespace DemoApp.Views.PopupSamples;

public partial class SampleCustomPopup : PopupPage
{
    PopupViewModel _viewModel = new PopupViewModel();
    public SampleCustomPopup(View anchorView, Trimble.Modus.Components.Enums.ModalPosition poisiton) : base(anchorView, poisiton)
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }
}
