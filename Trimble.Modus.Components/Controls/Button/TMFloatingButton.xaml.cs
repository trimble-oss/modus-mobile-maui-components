using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMFloatingButton : TMButton
{
    public TMFloatingButton()
    {
        IsFloatingButton = true;
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
            case ButtonColor.Secondary:
                return ResourcesDictionary.ColorsDictionary(ColorsConstants.SecondaryFloatingButtonClicked);
            case ButtonColor.Primary:
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
