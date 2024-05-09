using System;
namespace Trimble.Modus.Components.Styles
{
    public class ReferenceColor
    {
        public Color Color { get; set; }

        public ReferenceColor()
        {

        }

        public static implicit operator Color(ReferenceColor referenceColor) => referenceColor.Color;
    }
}

