using DemoApp.ViewModels;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;
using Size = Trimble.Modus.Components.Enums.Size;

namespace DemoApp;

public partial class ButtonSamplePage : ContentPage
{
    private readonly ButtonSamplePageViewModel _buttonPageViewModel;

    public ButtonSamplePage()
    {
        _buttonPageViewModel = new ButtonSamplePageViewModel();
        InitializeComponent();
        BindingContext = _buttonPageViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void Style_Changed(object sender, CheckedChangedEventArgs e)
    {
        if (sender is TMRadioButton radioButton && radioButton.IsSelected)
        {
            var selectedStyle = radioButton.Value.ToString();
            _buttonPageViewModel.SelectedButtonStyle = Trimble.Modus.Components.Helpers.AppDataHelper.ParseEnum<ButtonStyle>(selectedStyle);
        }
    }

    private void Size_Changed(object sender, CheckedChangedEventArgs e)
    {
        if (sender is TMRadioButton radioButton && radioButton.IsSelected)
        {
            var selectedSize = radioButton.Value.ToString();
            _buttonPageViewModel.SelectedFontSize = Trimble.Modus.Components.Helpers.AppDataHelper.ParseEnum<Size>(selectedSize);
        }
    }

    private void IsDisabled_Toggled(object sender, TMSwitchEventArgs e)
    {
        _buttonPageViewModel.IsDisabled = e.Value;
    }

    private void ImagePositionChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is TMRadioButton radioButton && radioButton.IsSelected)
        {
            _buttonPageViewModel.SelectedImageOption = radioButton.Value.ToString();
        }
    }

    private void FullWidthToggled(object sender, TMSwitchEventArgs e)
    {
        if (e.Value)
        {
            _buttonPageViewModel.FullWidthAlignment = LayoutOptions.FillAndExpand;
        }
        else
        {
            _buttonPageViewModel.FullWidthAlignment = LayoutOptions.Start;
        }

    }

    private void isLoading_Toggled(object sender, TMSwitchEventArgs e)
    {
        _buttonPageViewModel.IsLoading = e.Value;
    }
}
