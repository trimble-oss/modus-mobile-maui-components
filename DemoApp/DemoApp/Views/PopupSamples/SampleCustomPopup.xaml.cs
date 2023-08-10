using DemoApp.ViewModels;
using Trimble.Modus.Components;

namespace DemoApp.Views.PopupSamples;

public partial class SampleCustomPopup : PopupPage
{
    PopupViewModel _viewModel = new PopupViewModel();
    public SampleCustomPopup()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }
}
