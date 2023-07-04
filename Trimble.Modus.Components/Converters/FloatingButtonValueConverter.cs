using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components.Converters
{
    public class FloatingButtonValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Allowed values for the IsFloatingButton property
            var allowedValues = new List<FloatingButtonColor> { FloatingButtonColor.Primary, FloatingButtonColor.Secondary };
        
            // Check if the value is allowed
            if (value is FloatingButtonColor buttonColor && allowedValues.Contains(buttonColor))
                return buttonColor;

            // Return a default value if the value is not allowed
            return false; // Or any other default value you want to set
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not needed for this example
            throw new NotImplementedException();
        }
    }

}
