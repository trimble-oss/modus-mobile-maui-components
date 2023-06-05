using DemoApp.ViewModels;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;
using Size = Trimble.Modus.Components.Enums.Size;

namespace DemoApp;

public partial class TMButtonPage : ContentPage
{
    private string _selectedStyle = "Fill";
    private string _selectedSize = "Default";
    private bool _isDiabled;
    private readonly ButtonPageViewModel _buttonPageViewModel;

    public TMButtonPage()
    {
        _buttonPageViewModel = new ButtonPageViewModel();

        InitializeComponent();
        BindingContext = _buttonPageViewModel;
    }

    private void Style_Changed(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _selectedStyle = radioButton.Value.ToString();
            _buttonPageViewModel.SelectedButtonStyle = Trimble.Modus.Components.Helpers.AppDataHelper.ParseEnum<ButtonStyle>(_selectedStyle);
        }
    }

    private void Size_Changed(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _selectedSize = radioButton.Value.ToString();
            _buttonPageViewModel.SelectedFontSize = Trimble.Modus.Components.Helpers.AppDataHelper.ParseEnum<Size>(_selectedSize);
        }
    }

    private void isDisabled_Toggled(object sender, ToggledEventArgs e)
    {
        _buttonPageViewModel.IsDisabled = e.Value;
    }
}
