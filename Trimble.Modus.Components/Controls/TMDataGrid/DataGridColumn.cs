using System.ComponentModel;
using System.Globalization;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Controls.DataGridControl;

namespace Trimble.Modus.Components;
/// <summary>
/// Specifies each column of the DataGrid.
/// </summary>
public class DataGridColumn : BindableObject, IDefinition
{
    #region Fields

    private bool? _isSortable;
    private ColumnDefinition? _columnDefinition;
    private readonly ColumnDefinition _invisibleColumnDefinition = new(0);
    private readonly WeakEventManager _sizeChangedEventManager = new();

    #endregion Fields
    internal DataGrid? DataGrid { get; set; }

    public DataGridColumn()
    {
        HeaderLabel = new Label { Margin = new Thickness(0,0,0,0), FontFamily = "OpenSansRegular" };
        SortingIcon = new Image {Source = ImageConstants.CaretDownImage};
        SortingIconContainer = new ContentView
        {
            IsVisible = false,
            Content = SortingIcon,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };
    }

    #region Events

    public event EventHandler SizeChanged
    {
        add => _sizeChangedEventManager.AddEventHandler(value);
        remove => _sizeChangedEventManager.RemoveEventHandler(value);
    }

    #endregion Events

    #region Bindable Properties

    public static readonly BindableProperty WidthProperty = BindableProperty.Create(nameof(Width), typeof(GridLength), typeof(DataGridColumn), GridLength.Star, propertyChanged: OnWidthPropertyChanged);
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(DataGridColumn), string.Empty, propertyChanged: OnColumnTitleChanged);
    public static readonly BindableProperty PropertyNameProperty = BindableProperty.Create(nameof(PropertyName), typeof(string), typeof(DataGridColumn));
    public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(DataGridColumn), true, propertyChanged: OnIsVisiblePropertyChanged);
    public static readonly BindableProperty CellTemplateProperty = BindableProperty.Create(nameof(CellTemplate), typeof(DataTemplate), typeof(DataGridColumn));
    public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(DataGridColumn), LineBreakMode.WordWrap);
    public static readonly BindableProperty HorizontalContentAlignmentProperty = BindableProperty.Create(nameof(HorizontalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.Center);
    public static readonly BindableProperty VerticalContentAlignmentProperty = BindableProperty.Create(nameof(VerticalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.Center);
    public static readonly BindableProperty SortingEnabledProperty = BindableProperty.Create(nameof(SortingEnabled), typeof(bool), typeof(DataGridColumn), true);
    #endregion Bindable Properties

    #region Properties


    internal ColumnDefinition? ColumnDefinition
    {
        get
        {
            if (!IsVisible)
            {
                return _invisibleColumnDefinition;
            }

            return _columnDefinition;
        }
        set => _columnDefinition = value;
    }

    internal View? HeaderView { get; set; }

    /// <summary>
    /// Width of the column. Like Grid, you can use <c>Absolute, star, Auto</c> as unit.
    /// </summary>
    [TypeConverter(typeof(GridLengthTypeConverter))]
    public GridLength Width
    {
        get => (GridLength)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    /// <summary>
    /// Column title
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    /// <summary>
    /// Property name to bind in the object
    /// </summary>
    public string PropertyName
    {
        get => (string)GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    /// <summary>
    /// Is this column visible?
    /// </summary>
    public bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    /// <summary>
    /// Cell template. Default value is <c>Label</c> with binding <c>PropertyName</c>
    /// </summary>
    public DataTemplate? CellTemplate
    {
        get => (DataTemplate?)GetValue(CellTemplateProperty);
        set => SetValue(CellTemplateProperty, value);
    }

    /// <summary>
    /// LineBreakModeProperty for the text. WordWrap by default.
    /// </summary>
    public LineBreakMode LineBreakMode
    {
        get => (LineBreakMode)GetValue(LineBreakModeProperty);
        set => SetValue(LineBreakModeProperty, value);
    }

    /// <summary>
    /// Horizontal alignment of the cell content
    /// </summary>
    public LayoutOptions HorizontalContentAlignment
    {
        get => (LayoutOptions)GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    /// <summary>
    /// Vertical alignment of the cell content
    /// </summary>
    public LayoutOptions VerticalContentAlignment
    {
        get => (LayoutOptions)GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    /// <summary>
    /// Defines if the column is sortable. Default is true
    /// Sortable columns must implement <see cref="IComparable"/>
    /// </summary>
    public bool SortingEnabled
    {
        get => (bool)GetValue(SortingEnabledProperty);
        set => SetValue(SortingEnabledProperty, value);
    }

    internal Image SortingIcon { get; }
    internal Label HeaderLabel { get; }
    internal View SortingIconContainer { get; }
    internal SortingOrder SortingOrder { get; set; }

    #endregion Properties

    #region Methods
    private static void OnColumnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var column = (DataGridColumn)bindable;
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        column.HeaderLabel.Text = textInfo.ToTitleCase((string)newValue);
    }
    /// <summary>
    /// On IsVisible property changed
    /// </summary>
    private static void OnIsVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue && bindable is DataGridColumn column)
        {
            try
            {
                column.DataGrid?.Reload();
            }
            catch { }
            finally
            {
                column.OnSizeChanged();
            }
        }
    }
    /// <summary>
    /// Set Column width
    /// </summary>
    private static void OnWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!oldValue.Equals(newValue) && newValue is GridLength width && bindable is DataGridColumn self)
        {
            self.ColumnDefinition = new(width);
            self.OnSizeChanged();
        }
    }

    /// <summary>
    /// Determines via reflection if the column's data type is sortable.
    /// If you want to disable sorting for specific column please use <c>SortingEnabled</c> property
    /// </summary>
    /// <param name="dataGrid"></param>
    public bool IsSortable(DataGrid dataGrid)
    {
        if (_isSortable is not null)
        {
            return _isSortable.Value;
        }

        try
        {
            if (dataGrid.ItemsSource is null)
            {
                _isSortable = false;
            }
            else
            {
                var listItemType = dataGrid.ItemsSource.GetType().GetGenericArguments().Single();
                var columnDataType = listItemType.GetProperty(PropertyName)?.PropertyType;

                if (columnDataType is not null)
                {
                    _isSortable = typeof(IComparable).IsAssignableFrom(columnDataType);
                }
            }
        }
        catch
        {
            _isSortable = false;
        }

        return _isSortable ?? false;
    }

    private void OnSizeChanged() => _sizeChangedEventManager.HandleEvent(this, EventArgs.Empty, nameof(SizeChanged));

    #endregion Methods
}
