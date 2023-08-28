using DemoApp.ViewModels;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Views;

public partial class SegmentedControlSamplePage : ContentPage
{
    private SegmentedControlSamplePageViewModel _segmentedControlViewModel = new();

    public SegmentedControlSamplePage()
    {
        InitializeComponent();
        BindingContext = _segmentedControlViewModel;
    }

    private void TMRadioButtonGroup_SelectedRadioButtonChanged(
        object sender,
        TMRadioButtonEventArgs e
    )
    {
        _segmentedControlViewModel.Size = e.Value switch
        {
            "Small" => SegmentedControlSize.Small,
            "Medium" => SegmentedControlSize.Medium,
            "Large" => SegmentedControlSize.Large,
            "XLarge" => SegmentedControlSize.XLarge,
            _ => SegmentedControlSize.Small,
        };
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        _segmentedControlViewModel.RoundedCorners = e.Value;
    }

    private void Theme_Switch_Toggled(object sender, ToggledEventArgs e)
    {
        _segmentedControlViewModel.SegmentTheme = e.Value
            ? SegmentColorTheme.Secondary
            : SegmentColorTheme.Primary;
    }

    private void Enable_Switch_Toggled(object sender, ToggledEventArgs e)
    {
        _segmentedControlViewModel.IsEnabled = e.Value;
    }
}
