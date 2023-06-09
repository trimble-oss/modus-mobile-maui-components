using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trimble.Modus.Components.Converters
{
    public class FloatingButtonBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isFullWidthButton = (bool)values[0];
            bool isFloatingButton = (bool)values[1];

            if (isFullWidthButton && !isFloatingButton)
            {
                return true;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
