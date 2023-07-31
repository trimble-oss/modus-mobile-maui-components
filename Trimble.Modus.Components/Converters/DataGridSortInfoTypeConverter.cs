using System.ComponentModel;
using System.Globalization;
using Trimble.Modus.Components.Controls.DataGridControl;

namespace Trimble.Modus.Components.Converters;
/// <summary>
/// Converts string to SortingOrder enum.
/// </summary>
public sealed class DataGridSortInfoTypeConverter : TypeConverter
{
    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (int.TryParse(value.ToString(), out var index))
        {
            return (DataGridSortInfo)index;
        }

        return base.ConvertFrom(context, culture, value);
    }
}
