using DemoApp.ViewModels;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Popup.Animations;
using Trimble.Modus.Components.Popup.Enums;

namespace DemoApp.Views.PopupSamples;

public partial class SampleCustomPopup : PopupPage
{
    PopupViewModel _viewModel = new PopupViewModel();
    public SampleCustomPopup()
    {
        InitializeComponent();
        BindingContext = _viewModel;
        Animation = new MoveAnimation() { PositionIn = MoveAnimationOptions.Left, PositionOut = MoveAnimationOptions.Left, DurationIn = 500, DurationOut = 500 };
    }
}
