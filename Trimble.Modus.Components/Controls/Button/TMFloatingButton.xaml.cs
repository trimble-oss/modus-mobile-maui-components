using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMFloatingButton : TMButton
{
    public static readonly BindableProperty FloatingButtonColorProperty =
       BindableProperty.Create(nameof(ButtonColor), typeof(FloatingButtonColor), typeof(TMFloatingButton), Enums.FloatingButtonColor.Primary, propertyChanged: OnButtonColorChanged);
    public new FloatingButtonColor ButtonColor
    {
        get => (FloatingButtonColor)GetValue(FloatingButtonColorProperty);
        set => SetValue(FloatingButtonColorProperty, value);
    }
    private new Enums.ButtonStyle ButtonStyle
    {
        get => (Enums.ButtonStyle)GetValue(ButtonStyleProperty);
        set => SetValue(ButtonStyleProperty, value);
    }
    private static void OnButtonColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMFloatingButton tmFloatingButton)
        {
            UpdateColor(tmFloatingButton);
        }
    }
    private static void UpdateColor(TMFloatingButton tmFloatingButton)
    {
        switch (tmFloatingButton.ButtonColor)
        {
            
            case FloatingButtonColor.Secondary:
                tmFloatingButton._buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                tmFloatingButton._buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                tmFloatingButton._buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
                break;
            case FloatingButtonColor.Primary:
            default:
                tmFloatingButton._buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmFloatingButton._buttonFrame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmFloatingButton._buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
        }
    }

    public TMFloatingButton()
    {
        InitializeComponent();
        SetRadius();
    }
    private void SetRadius()
    {
        if (_buttonFrame != null)
        {
            _buttonFrame.StrokeShape = new Rectangle
            {
                RadiusX = 50,
                RadiusY = 50
            };
            _buttonFrame.Shadow = new Shadow
            {
                Radius = 15,
                Opacity = 1
            };
            _buttonFrame.ZIndex = 1;
        }
    }
    public void RaisePressed()
    {
        if (_buttonFrame.BackgroundColor != null)
        {
            activeColor = _buttonFrame.BackgroundColor;
            _buttonFrame.BackgroundColor = GetOnClickColor(_buttonFrame.BackgroundColor);
        }
    }

    private Color GetOnClickColor(Color backgroundColor)
    {
        switch (ButtonColor)
        {
            case FloatingButtonColor.Secondary:
                return ResourcesDictionary.ColorsDictionary(ColorsConstants.SecondaryFloatingButtonClicked);
            case FloatingButtonColor.Primary:
            default:
                return ResourcesDictionary.ColorsDictionary(ColorsConstants.PrimaryFloatingButtonClicked);
        }
    }
    public void RaiseReleased()
    {
        if (activeColor != null)
        {
            _buttonFrame.BackgroundColor = activeColor;
        }
        Command?.Execute(CommandParameter);
        _clicked?.Invoke(this, EventArgs.Empty);
    }
    public void RaiseCancel()
    {
        if (activeColor != null)
        {
            _buttonFrame.BackgroundColor = activeColor;
        }
    }

}
