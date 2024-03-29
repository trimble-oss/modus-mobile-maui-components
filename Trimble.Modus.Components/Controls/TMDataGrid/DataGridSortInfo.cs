using System.ComponentModel;
using Trimble.Modus.Components.Converters;

namespace Trimble.Modus.Components.Controls.DataGridControl;
/// <summary>
/// Creates SortData for DataGrid
/// </summary>
[TypeConverter(typeof(DataGridSortInfoTypeConverter))]
public sealed class DataGridSortInfo

{
    public static implicit operator DataGridSortInfo(int index) => new()
    {
        Index = Math.Abs(index),
        Order = index < 0 ? SortingOrder.Descendant : SortingOrder.Ascendant
    };

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is DataGridSortInfo other)
        {
            return other.Index == Index && other.Order == Order;
        }

        return false;
    }

    #region ctor

    public DataGridSortInfo()
    { }

    public DataGridSortInfo(int index, SortingOrder order)
    {
        Index = index;
        Order = order;
    }

    #endregion ctor

    #region Properties

    /// <summary>
    /// Sorting order for the column
    /// </summary>
    public SortingOrder Order { get; set; }

    /// <summary>
    /// Column Index to sort
    /// </summary>
    public int Index { get; set; }

    #endregion Properties

    /// <inheritdoc/>
    public override int GetHashCode() => throw new NotImplementedException();
}
