using static System.Math;

namespace Trimble.Modus.Components.Controls.Slider
{
    internal class SliderHelper
    {
        public static double CoerceValue(double value, double stepValue, double minimumValue, double maximumValue)
        {
            if (stepValue > 0 && value < maximumValue)
            {
                var stepIndex = (int)((value - minimumValue) / stepValue);
                value = minimumValue + (stepIndex * stepValue);
            }
            return Clamp(value, minimumValue, maximumValue);
        }
        public static void RaiseEvent(object? sender, EventHandler? eventHandler)
            => eventHandler?.Invoke(sender, EventArgs.Empty);
        public static Border CreateBorderElement<TBorder>() where TBorder : Border, new()
        {
            var border = new Border
            {
                Padding = 0
            };

            return border;
        }
        public static Label CreateLabelElement()
            => new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            };
    }
}
