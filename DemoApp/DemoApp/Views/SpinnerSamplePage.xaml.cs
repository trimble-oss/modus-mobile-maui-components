using DemoApp.ViewModels;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Views;

public partial class SpinnerSamplePage : ContentPage
{
    private SpinnerSamplePageViewModel spinnerSamplePageViewModel;
    public SpinnerSamplePage()
    {
        spinnerSamplePageViewModel = new SpinnerSamplePageViewModel();
        InitializeComponent();
        BindingContext = spinnerSamplePageViewModel;
    }

    private void SelectedRadioButtonChanged(object sender, Trimble.Modus.Components.TMRadioButtonEventArgs e)
    {
        spinnerSamplePageViewModel.SpinnerColor = ((string)e.Value == "Primary") ? SpinnerColor.Primary : SpinnerColor.Secondary;
    }
}
