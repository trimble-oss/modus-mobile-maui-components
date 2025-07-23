using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Controls.DataGridControl;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;
/// <summary>
/// DataGrid component for Maui
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DataGrid
{
    #region Fields

    private static readonly ColumnDefinitionCollection HeaderColumnDefinitions = new()
                {
                    new() { Width = new(1, GridUnitType.Star) },
                    new() { Width = new(1, GridUnitType.Auto) }
                };

    private readonly WeakEventManager _itemSelectedEventManager = new();

    private readonly Style _defaultHeaderStyle;

    /// <summary>
    /// Border color
    /// Default Value is Black
    /// </summary>
    internal Color BorderColor = ResourcesDictionary.GetColor(ColorsConstants.TertiaryDark);
    /// <summary>
    /// Border thickness for header &amp; each cell
    /// </summary>
    internal Thickness BorderThickness = new(0, 0, 0, 1);
    #endregion Fields

    #region ctor

    public DataGrid()
    {
        InitializeComponent();
        _defaultHeaderStyle = (Style)Resources["DefaultHeaderStyle"];
        this.SetDynamicResource(StyleProperty, "DataGridStyle");
    }

    #endregion ctor

    #region Events

    public event EventHandler<Microsoft.Maui.Controls.SelectionChangedEventArgs> ItemSelected
    {
        add => _itemSelectedEventManager.AddEventHandler(value);
        remove => _itemSelectedEventManager.RemoveEventHandler(value);
    }

    #endregion Events

    #region Sorting methods

    /// <summary>
    /// Check if the column is sortable and display a debug message if not 
    /// </summary>
    private bool CanSort(DataGridSortInfo? sortData = null)
    {
        sortData ??= ColumnSortingInfo;

        if (sortData is null)
        {
            Console.WriteLine("There is no sort data");
            return false;
        }

        if (InternalItems is null)
        {
            Console.WriteLine("There are no items to sort");
            return false;
        }

        if (!IsSortable)
        {
            Console.WriteLine("DataGrid is not sortable");
            return false;
        }

        if (Columns.Count < 1)
        {
            Console.WriteLine("There are no columns on this DataGrid.");
            return false;
        }

        if (sortData is null)
        {
            Console.WriteLine("Sort index is null");
            return false;
        }

        if (sortData.Index >= Columns.Count)
        {
            Console.WriteLine("Sort index is out of range");
            return false;
        }

        var columnToSort = Columns[sortData.Index];

        if (columnToSort.PropertyName == null)
        {
            Console.WriteLine($"Please set the {nameof(columnToSort.PropertyName)} of the column");
            return false;
        }

        if (!columnToSort.SortingEnabled)
        {
            Console.WriteLine($"{columnToSort.PropertyName} column does not have sorting enabled");
            return false;
        }

        if (!columnToSort.IsSortable(this))
        {
            Console.WriteLine($"{columnToSort.PropertyName} column is not sortable");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Sort the items
    /// </summary>
    /// <param name="unsortedItems">Initial unsortedItems</param>
    /// <param name="sortData">Contains the column index and sorting order</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private IList<object> GetSortedItems(IList<object> unsortedItems, DataGridSortInfo sortData)
    {
        var columnToSort = Columns[sortData.Index];

        foreach (var column in Columns)
        {
            if (column == columnToSort)
            {
                column.SortingOrder = sortData.Order;
                column.SortingIconContainer.IsVisible = sortData.Order != SortingOrder.None;
            }
            else
            {
                column.SortingOrder = SortingOrder.None;
                column.SortingIconContainer.IsVisible = false;
            }
        }
        IEnumerable<object> items;

        switch (sortData.Order)
        {
            case SortingOrder.Ascendant:
                items = unsortedItems.OrderBy(x => ReflectionUtils.GetValueByPath(x, columnToSort.PropertyName)).ToList();
                _ = columnToSort.SortingIcon.RotateTo(0, 300, easing: Easing.Linear);
                break;
            case SortingOrder.Descendant:
                items = unsortedItems.OrderByDescending(x => ReflectionUtils.GetValueByPath(x, columnToSort.PropertyName)).ToList();
                _ = columnToSort.SortingIcon.RotateTo(180, 300, easing: Easing.Linear);
                break;
            case SortingOrder.None:
                return unsortedItems;
            default:
                throw new NotImplementedException();
        }

        return items.ToList();
    }

    private void SortColumn(DataGridSortInfo? sortData = null)
    {
        if (ItemsSource is null)
        {
            return;
        }

        sortData ??= ColumnSortingInfo;

        var originalItems = ItemsSource.Cast<object>().ToList();

        IList<object> sortedItems;

        if (sortData != null && CanSort(sortData))
        {
            sortedItems = GetSortedItems(originalItems, sortData);
        }
        else
        {
            sortedItems = originalItems;
        }


        InternalItems = sortedItems;
    }
    #endregion Sorting methods

    #region Methods

    /// <summary>
    /// Scrolls to the row
    /// </summary>
    /// <param name="item">Item to scroll</param>
    /// <param name="position">Position of the row in screen</param>
    /// <param name="animated">animated</param>
    public void ScrollTo(object item, ScrollToPosition position, bool animated = true) => _collectionView.ScrollTo(item, position: position, animate: animated);

    #endregion Methods

    #region Bindable properties
    public static readonly BindableProperty ColumnsProperty = BindableProperty.Create(nameof(Columns), typeof(ObservableCollection<DataGridColumn>), typeof(DataGrid), propertyChanged: OnColumnsChanged, defaultValueCreator: _ => new ObservableCollection<DataGridColumn>());
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(DataGrid), null, propertyChanged: OnItemsSourceChanged);
    public static readonly BindableProperty IsSortableProperty = BindableProperty.Create(nameof(IsSortable), typeof(bool), typeof(DataGrid), false);
    public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(DataGrid), Microsoft.Maui.Controls.SelectionMode.None, BindingMode.TwoWay, propertyChanged: OnSelectionModeChanged);
    public static readonly BindableProperty EmptyProperty = BindableProperty.Create(nameof(EmptyView), typeof(View), typeof(DataGrid), propertyChanged: OnNoDataViewChanged);
    public static readonly BindableProperty ShowDividerProperty = BindableProperty.Create(nameof(ShowDivider), typeof(bool), typeof(DataGrid), true, propertyChanged: OnShowDividerPropertyChanged);
    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList<object>), typeof(DataGrid), null, BindingMode.TwoWay);
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(DataGrid), null, BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged, coerceValue: ValidateSelectionItemWithMode);
    public static readonly BindableProperty ColumnSortingInfoProperty = BindableProperty.Create(nameof(ColumnSortingInfo), typeof(DataGridSortInfo), typeof(DataGrid), null, BindingMode.TwoWay, ColumnSortingInfoValidator, OnColumnSortingInfoChanged);
    public static readonly BindableProperty ActiveRowColorProperty = BindableProperty.Create(nameof(ActiveRowColor), typeof(Color), typeof(DataGrid), null, BindingMode.TwoWay, propertyChanged: (bindable, _, _) => (bindable as DataGrid).Reload());
    public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(DataGrid), null, propertyChanged: (bindable, _, _) => (bindable as DataGrid).Reload());
    public static readonly BindableProperty DefaultRowColorProperty = BindableProperty.Create(nameof(DefaultRowColor), typeof(Color), typeof(DataGrid), null, propertyChanged: (bindable, _, _) => (bindable as DataGrid).Reload());
    #endregion Bindable properties

    #region Property changed handlers
    private static bool ColumnSortingInfoValidator(BindableObject bindable, object value)
    {
        var self = (DataGrid)bindable;
        var sortData = (DataGridSortInfo)value;
        if (!self.IsLoaded)
        {
            return true;
        }
        return (self.CanSort(sortData));
    }
    private static void OnColumnSortingInfoChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue && bindable is DataGrid self && newValue is DataGridSortInfo sortData)
        {
            self.SortColumn(sortData);
        }
    }
    private static object ValidateSelectionItemWithMode(BindableObject bindable, object value)
    {
        var self = (DataGrid)bindable;
        if (self.SelectionMode == SelectionMode.None)
        {
            return null;
        }

        return value;
    }
    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (DataGrid)bindable;
        if (self._collectionView.SelectedItem != newValue)
        {
            self._collectionView.SelectedItem = newValue;
        }
    }
    /// <summary>
    /// Triggered when column is changed
    /// </summary>
    private static void OnColumnsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue || bindable is not DataGrid self)
        {
            return;
        }

        if (oldValue is ObservableCollection<DataGridColumn> oldColumns)
        {
            oldColumns.CollectionChanged -= self.OnColumnsChanged;

            foreach (var oldColumn in oldColumns)
            {
                oldColumn.SizeChanged -= self.OnColumnSizeChanged;
            }
        }

        if (newValue is ObservableCollection<DataGridColumn> newColumns)
        {
            newColumns.CollectionChanged += self.OnColumnsChanged;

            foreach (var newColumn in newColumns)
            {
                newColumn.SizeChanged += self.OnColumnSizeChanged;
            }
        }

        self.Reload();
    }

    /// <summary>
    /// Hide or show divider 
    /// </summary>
    private static void OnShowDividerPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var dataGrid = (DataGrid)bindable;
        dataGrid.BorderThickness = (bool)newValue ? new(0.5) : new(0, 0, 0, 1);
        dataGrid._headerView.BackgroundColor = (bool)newValue ? dataGrid.BorderColor : dataGrid.HeaderBackgroundColor;
        dataGrid._collectionView.BackgroundColor = (bool)newValue ? dataGrid.BorderColor : dataGrid.DefaultRowColor;
        dataGrid.Reload();
    }
    /// <summary>
    /// Display no data view
    /// </summary>
    private static void OnNoDataViewChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue && bindable is DataGrid self)
        {
            self._collectionView.EmptyView = newValue as View;
        }
    }
    /// <summary>
    /// Clear and reload selection of table and add event handler when selection mode is changed
    /// </summary>
    private static void OnSelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (DataGrid)bindable;
        self._collectionView.SelectionChanged -= self.OnSelectionChanged;
        self.SelectedItem = null;
        self.SelectedItems.Clear();
        self._collectionView.SelectedItems.Clear();
        if (newValue is SelectionMode mode && mode != Microsoft.Maui.Controls.SelectionMode.None)
        {
            self._collectionView.SelectionChanged += self.OnSelectionChanged;
        }
        self.Reload();
    }

    /// <summary>
    /// Update event handlers and Selected items property when itemsource is changed
    /// </summary>
    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {

        if (newValue == oldValue || bindable is not DataGrid self)
        {
            return;
        }

        if (oldValue is INotifyCollectionChanged oldCollection)
        {
            oldCollection.CollectionChanged -= self.HandleItemsSourceCollectionChanged;
        }

        if (newValue == null)
        {
            self.InternalItems = null;
        }
        else
        {
            if (newValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += self.HandleItemsSourceCollectionChanged;
            }

            var itemsSource = ((IEnumerable)newValue).Cast<object>().ToList();


            self.SortColumn();
        }

        if (self.SelectedItem != null && self.InternalItems?.Contains(self.SelectedItem) != true || self._collectionView.SelectedItems.Count > 0)
        {
            self.SelectedItem = null;
            self.SelectedItems.Clear();
            self._collectionView.SelectedItems.Clear();
        }

    }

    private void HandleItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        SortColumn();

        if (SelectedItem != null && InternalItems?.Contains(SelectedItem) != true)
        {
            SelectedItem = null;
            SelectedItems.Clear();
        }
    }

    #endregion

    #region Properties
    /// <summary>
    /// Active row color based on theme
    /// </summary>
    internal Color DefaultRowColor
    {
        get => (Color)GetValue(DefaultRowColorProperty);
        set => SetValue(DefaultRowColorProperty, value);
    }

    /// <summary>
    /// Header Background color based on theme
    /// </summary>
    internal Color HeaderBackgroundColor
    {
        get => (Color)GetValue(HeaderBackgroundColorProperty);
        set => SetValue(HeaderBackgroundColorProperty, value);
    }
    /// <summary>
    /// Active row color based on theme
    /// </summary>
    internal Color ActiveRowColor
    {
        get => (Color)GetValue(ActiveRowColorProperty);
        set => SetValue(ActiveRowColorProperty, value);
    }

    /// <summary>
    /// Set if divider should be shown
    /// </summary>
    public bool ShowDivider
    {
        get => (bool)GetValue(ShowDividerProperty);
        set => SetValue(ShowDividerProperty, value);
    }

    /// <summary>
    /// ItemsSource of the DataGrid
    /// </summary>
    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    private IList<object>? _internalItems;

    internal IList<object>? InternalItems
    {
        get => _internalItems;
        set
        {
            if (_internalItems != value)
            {
                _internalItems = value;
                _collectionView.ItemsSource = _internalItems;
            }
        }
    }

    /// <summary>
    /// Columns
    /// </summary>
    public ObservableCollection<DataGridColumn> Columns
    {
        get => (ObservableCollection<DataGridColumn>)GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    /// <summary>
    /// Gets or sets if the grid is sortable. Default value is true.
    /// Sortable columns must implement <see cref="IComparable"/>
    /// If you want to enable or disable sorting for specific column please use <c>SortingEnabled</c> property
    /// </summary>
    public bool IsSortable
    {
        get => (bool)GetValue(IsSortableProperty);
        set => SetValue(IsSortableProperty, value);
    }

    /// <summary>
    /// Enables selection in dataGrid. Default value is True
    /// </summary>
    public SelectionMode SelectionMode
    {
        get => (Microsoft.Maui.Controls.SelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
    }

    /// <summary>
    /// Selected item
    /// </summary>
    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    /// <summary>
    /// Selected item
    /// </summary>
    public IList<object> SelectedItems
    {
        get => (IList<object>)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }
    /// <summary>
    /// Column index and sorting order for the DataGrid
    /// </summary>
    public DataGridSortInfo ColumnSortingInfo
    {
        get => (DataGridSortInfo)GetValue(ColumnSortingInfoProperty);
        set => SetValue(ColumnSortingInfoProperty, value);
    }

    /// <summary>
    /// View to show when there is no data to display
    /// </summary>
    public View EmptyView
    {
        get => (View)GetValue(EmptyProperty);
        set => SetValue(EmptyProperty, value);
    }

    #endregion Properties

    #region UI Methods

    /// <inheritdoc/>
    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (SelectionMode != SelectionMode.None)
        {
            _collectionView.SelectionChanged -= OnSelectionChanged;
            _collectionView.SelectionChanged += OnSelectionChanged;
        }

        if (Parent != null)
        {
            Columns.CollectionChanged += OnColumnsChanged;

            foreach (var column in Columns)
            {
                column.SizeChanged += OnColumnSizeChanged;
            }
        }
        else
        {
            Columns.CollectionChanged -= OnColumnsChanged;

            foreach (var column in Columns)
            {
                column.SizeChanged -= OnColumnSizeChanged;
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        InitHeaderView();
    }

    private void OnColumnsChanged(object? sender, NotifyCollectionChangedEventArgs e) => Reload();

    private void OnColumnSizeChanged(object? sender, EventArgs e) => Reload();

    private void OnSelectionChanged(object? sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        SelectedItem = SelectionMode != SelectionMode.None? _collectionView.SelectedItem : null;
        SelectedItems =  _collectionView.SelectedItems;
        _itemSelectedEventManager.HandleEvent(this, e, nameof(ItemSelected));

    }

    /// <summary>
    /// Reload the table
    /// </summary>
    internal void Reload()
    {
        InitHeaderView();
        _collectionView.BackgroundColor = BorderColor;

        if (_internalItems is not null)
        {
            InternalItems = new List<object>(_internalItems);
        }
    }

    #endregion UI Methods

    #region Header Creation Methods

    /// <summary>
    /// Gets the header view for column.
    /// </summary>
    /// <returns>The header view for column.</returns>
    /// <param name="column">Column.</param>
    /// <param name="index">Index.</param>
    private View GetHeaderViewForColumn(DataGridColumn column, int index)
    {
        column.HeaderLabel.Style = _defaultHeaderStyle;
        Grid.SetColumnSpan(column.HeaderLabel, 2);
        if (!IsSortable || !column.SortingEnabled || !column.IsSortable(this))
        {
            return new ContentView
            {
                Content = column.HeaderLabel
            };
        }
        column.SortingIconContainer.HeightRequest = 32;
        column.SortingIconContainer.WidthRequest = 32;

        var grid = new Grid
        {
            ColumnSpacing = 0,
            Padding = new(0, 0, 0, 0),
            ColumnDefinitions = HeaderColumnDefinitions,
            Children = { column.HeaderLabel, column.SortingIconContainer },
            GestureRecognizers =
            {
                new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        var order = SortingOrder.None;
                        switch (column.SortingOrder)
                        {
                            case SortingOrder.None:
                                order = SortingOrder.Ascendant;
                                break;
                            case SortingOrder.Ascendant:
                                order = SortingOrder.Descendant;
                                break;
                            case SortingOrder.Descendant:
                                order = SortingOrder.None;
                                break;
                        }

                        ColumnSortingInfo = new(index, order);
                        column.SortingOrder = order;
                    }, () => column.SortingEnabled)
                }
            }
        };

        Grid.SetColumn(column.SortingIconContainer, 1);
        return grid;
    }

    /// <summary>
    /// Initializes the header view.
    /// </summary>
    private void InitHeaderView()
    {
        SetColumnsBindingContext();

        _headerView.Children.Clear();
        _headerView.ColumnDefinitions.Clear();
        ResetSortingOrders();

        if (Columns == null)
        {
            return;
        }

        for (var i = 0; i < Columns.Count; i++)
        {
            var col = Columns[i];

            col.ColumnDefinition ??= new(col.Width);

            col.DataGrid ??= this;

            _headerView.ColumnDefinitions.Add(col.ColumnDefinition);

            if (!col.IsVisible)
            {
                continue;
            }

            col.HeaderView ??= GetHeaderViewForColumn(col, i);


            Grid.SetColumn(col.HeaderView, i);
            _headerView.Children.Add(col.HeaderView);
        }
        foreach (var child in _headerView.Children.OfType<View>())
        {
            child.BackgroundColor = HeaderBackgroundColor;
        }
    }

    /// <summary>
    /// Reset sorting order for all columns
    /// </summary>
    private void ResetSortingOrders()
    {
        foreach (var column in Columns)
        {
            column.SortingOrder = SortingOrder.None;
        }
    }

    /// <summary>
    /// Set the binding context for all columnsselecte
    /// </summary>
    private void SetColumnsBindingContext()
    {
        if (Columns != null)
        {
            foreach (var c in Columns)
            {
                c.BindingContext = BindingContext;
            }
        }
    }

    #endregion Header Creation Methods
}
