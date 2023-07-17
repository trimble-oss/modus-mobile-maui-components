using System;
using System.Globalization;

namespace Trimble.Modus.Components.Converters
{
    public class DescriptionToRowHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasDescription = !(value is null || string.IsNullOrWhiteSpace(value.ToString()));
            return hasDescription ? new GridLength(56) : new GridLength(54);
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
