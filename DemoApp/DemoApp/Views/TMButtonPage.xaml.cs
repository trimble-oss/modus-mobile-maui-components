using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;
using Size = Trimble.Modus.Components.Enums.Size;

namespace DemoApp;

public partial class TMButtonPage : ContentPage
{
    private string _selectedStyle = "Fill";
    private string _selectedSize = "Default";
    private bool _isDiabled;

    public bool IsDisabled
    {
        get => _isDiabled;
        set
        {
            if (_isDiabled != value)
            {
                _isDiabled = value;
                ValidateRadioButtons();
            }
        }
    }


    public TMButtonPage()
    {

        InitializeComponent();
    }

    private void Style_Changed(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _selectedStyle = radioButton.Value.ToString();
            ValidateRadioButtons();
        }
    }

    private void Size_Changed(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _selectedSize = radioButton.Value.ToString();
            ValidateRadioButtons();
        }
    }

    private void ValidateRadioButtons()
    {
        bool isStyleSelected = !string.IsNullOrEmpty(_selectedStyle);
        bool isSizeSelected = !string.IsNullOrEmpty(_selectedSize);

        if (isStyleSelected && isSizeSelected)
        {
            List<TMButton> buttons = new List<TMButton> { button1, button2, button3, button4, button1i, button2i, button3i, button4i };
            List<TMButton> iconbuttons = new List<TMButton> { button1i, button2i, button3i, button4i };


            foreach (TMButton button in buttons)
            {

                button.Size = (Size)Enum.Parse(typeof(Size), _selectedSize);
                button.ButtonStyle = (ButtonStyle)Enum.Parse(typeof(ButtonStyle), _selectedStyle);
                int index = buttons.IndexOf(button);
                if (index >= buttons.Count - 4)
                {
                    if (button.ButtonStyle == ButtonStyle.Outline || button.ButtonStyle == ButtonStyle.BorderLess)
                    {
                        button.IconSource = ImageSource.FromFile("icondark.png");
                    }
                    else
                    {
                        if (!(button.ButtonColor == ButtonColor.Tertiary))
                        {
                            button.IconSource = ImageSource.FromFile("gallery_icon.png");
                        }
                    }
                }

                if (_isDiabled)
                {
                    button.IsDisabled = true;
                }
                else
                {
                    button.IsDisabled = false;
                }
            }
        }
    }

    private void isDisabled_Toggled(object sender, ToggledEventArgs e)
    {
        IsDisabled = e.Value;
    }
}