using Trimble.Modus.Components.Enums;
using static System.Math;

namespace Trimble.Modus.Components.Controls.Slider
{
    internal class SliderHelper
    {
        public static double CoerceValue(double value, double stepValue, double minimumValue, double maximumValue)
        {
            if (stepValue > 0 && value < maximumValue)
            {
                double difference = value - minimumValue;
                int stepIndex = (int)Math.Floor(difference / stepValue);
                bool shouldIncrementStep = (DeviceInfo.Platform == DevicePlatform.WinUI) ? stepIndex >= (int)(value) / stepValue : stepIndex > (int)(value) / stepValue;
                if (difference % stepValue >0 && shouldIncrementStep)
                {
                    stepIndex++;
                }
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
        public static Label CreateStepLabel(SliderSize size = SliderSize.Medium)
        {
            var leftPadding = 10;
            if (size == SliderSize.Small)
            {
                leftPadding = 5;
            }
            else if (size == SliderSize.Large)
            {
                leftPadding = 14;
            }
            return new Label
            {
                TextColor = Colors.Black,
                FontSize = 8,
                HorizontalTextAlignment = TextAlignment.Start,
                LineBreakMode = LineBreakMode.NoWrap,
                Margin = new Thickness(leftPadding, 0, 0, 0),
                Padding = new Thickness(0, 4, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };
        }
        public static StackLayout CreateStepLabelContainer()
            => new StackLayout { Orientation = StackOrientation.Vertical, Margin = 0, Padding = 0, HorizontalOptions = LayoutOptions.StartAndExpand };
        public static BoxView CreateStepLine(SliderSize size = SliderSize.Medium)
        {
            var leftPadding = 10;
            if (size == SliderSize.Small)
            {
                leftPadding = 5;
            }
            else if (size == SliderSize.Large)
            {
                leftPadding = 14;
            }
            return new BoxView { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(leftPadding, 0, 0, 0), WidthRequest = 1, HeightRequest = 4, Color = Colors.Black };

        }
    }
}
