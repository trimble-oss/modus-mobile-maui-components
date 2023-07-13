using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trimble.Modus.Components.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Console.WriteLine("Color" + value);
            if (value is bool boolValue)
            {
                Console.WriteLine("Convert" + value);
                return boolValue ? Colors.Green : Colors.Red;
            }

            return Colors.Black; // Default color if value is not a boolean
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
