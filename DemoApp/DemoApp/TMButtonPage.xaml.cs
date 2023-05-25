using Trimble.Modus.Components.Enums;
using Size = Trimble.Modus.Components.Enums.Size;

namespace DemoApp;

public partial class TMButtonPage : ContentPage
{
    private string _selectedColor ="Primary";
    private string _selectedSize ="Default";

    public TMButtonPage()
	{
        
		InitializeComponent();
    }

    private void button_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("Clicked");
     
    }
    private void Color_Changed(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _selectedColor = radioButton.Value.ToString();
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
        bool isColorSelected = !string.IsNullOrEmpty(_selectedColor);
        bool isSizeSelected = !string.IsNullOrEmpty(_selectedSize);

        if (isColorSelected && isSizeSelected)
        {
            filled.Size = (Size)Enum.Parse(typeof(Size), _selectedSize);
            filled.ButtonColor = (ButtonColor)Enum.Parse(typeof(ButtonColor), _selectedColor);
            filledicon.Size = (Size)Enum.Parse(typeof(Size), _selectedSize);
            filledicon.ButtonColor = (ButtonColor)Enum.Parse(typeof(ButtonColor), _selectedColor);
            if(filledicon.ButtonColor == ButtonColor.Tertiary)
            {
                filledicon.IconSource = ImageSource.FromFile("icondark.png");
            }
            else
            {
                filledicon.IconSource = ImageSource.FromFile("icon.png");
            }
          
        }
    }



}