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
    #region Private Methods
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

    #endregion

    #region Public Methods


    #endregion

}
