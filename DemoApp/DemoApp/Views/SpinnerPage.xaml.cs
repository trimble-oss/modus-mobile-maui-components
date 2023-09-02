using DemoApp.ViewModels;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Views;

public partial class SpinnerPage : ContentPage
{
    private SpinnerSamplePageViewModel spinnerPageViewModel;
    public SpinnerPage()
    {
        spinnerPageViewModel = new SpinnerSamplePageViewModel();
        InitializeComponent();
        BindingContext = spinnerPageViewModel;
    }

    private void SelectedRadioButtonChanged(object sender, Trimble.Modus.Components.TMRadioButtonEventArgs e)
    {
        spinnerPageViewModel.SpinnerColor = ((string)e.Value == "Primary") ? SpinnerColor.Primary : SpinnerColor.Secondary;
    }
}
