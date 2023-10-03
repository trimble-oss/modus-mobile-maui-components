using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Controls.DataGridControl;

namespace Trimble.Modus.Components;
internal sealed class DataGridRow : Grid
{
    #region Fields

    private Color? _bgColor;
    private readonly Color? _textColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
    private bool _hasSelected;
    private List<int> SelectedIndexes = new List<int>();

    #endregion Fields

    #region Properties

    public DataGrid DataGrid
    {
        get => (DataGrid)GetValue(DataGridProperty);
        set => SetValue(DataGridProperty, value);
    }

    #endregion Properties

    public static readonly BindableProperty DataGridProperty = BindableProperty.Create(nameof(DataGrid), typeof(DataGrid), typeof(DataGridRow), defaultBindingMode: BindingMode.OneTime, propertyChanged: OnDataGridPropertyChanged);

    private static void OnDataGridPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (DataGridRow)bindable;

        if (oldValue is DataGrid oldDataGrid)
        {
            oldDataGrid.ItemSelected -= self.DataGrid_ItemSelected;
        }

        if (newValue is DataGrid newDataGrid && newDataGrid.SelectionMode != SelectionMode.None)
        {
            newDataGrid.ItemSelected += self.DataGrid_ItemSelected;
        }
    }

    #region Methods

    /// <summary>
    /// Create the container view
    /// </summary>
    private void CreateView()
    {
        ColumnDefinitions.Clear();
        Children.Clear();

        SetStyling();

        for (var i = 0; i < DataGrid.Columns.Count; i++)
        {
            var col = DataGrid.Columns[i];

            ColumnDefinitions.Add(col.ColumnDefinition);

            if (!col.IsVisible)
            {
                continue;
            }

            var cell = CreateCell(col);

            SetColumn((BindableObject)cell, i);
            Children.Add(cell);
        }
        if (DeviceInfo.Idiom == DeviceIdiom.Desktop && DataGrid.SelectionMode == SelectionMode.Multiple)
        {
            Padding = new Thickness(-28.5, 0, 0, 0);
        }
    }
    /// <summary>
    /// Set the styling 
    /// </summary>
    private void SetStyling()
    {
        UpdateBackgroundColor();
        BackgroundColor = DataGrid.BorderColor;
        var borderThickness = DataGrid.ShowDivider? DataGrid.BorderThickness : new Thickness(0, 0, 0, 1);
        ColumnSpacing = borderThickness.HorizontalThickness;
    }

    /// <summary>
    /// Create the cells for the row
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    private View CreateCell(DataGridColumn col)
    {
        View cell;

        if (col.CellTemplate != null)
        {
            var contentView = new ContentView
            {
                BackgroundColor = _bgColor,
                Content = col.CellTemplate.CreateContent() as View
            };
            if (col is TextColumn)
            {
                var grid = contentView.Content as Grid;
                if (grid != null)
                {
                    if(!string.IsNullOrWhiteSpace((col as TextColumn).PropertyName))
                        (grid.Children[0] as Label).SetBinding(Label.TextProperty, new Binding((col as TextColumn).PropertyName, source: BindingContext, stringFormat: (col as TextColumn).StringFormat));
                    else
                        (grid.Children[0] as Label).IsVisible = false;
                    if (!string.IsNullOrWhiteSpace((col as TextColumn).DescriptionProperty))
                        (grid.Children[1] as Label).SetBinding(Label.TextProperty, new Binding((col as TextColumn).DescriptionProperty, source: BindingContext));
                    else
                        (grid.Children[1] as Label).IsVisible = false;
                }
            }
            else if (col is ImageColumn imageColumn)
            {
                var image = contentView.Content as Image;
                image?.SetBinding(Image.SourceProperty, new Binding(imageColumn.PropertyName, source: BindingContext));
            }
            else if(col is BooleanColumn booleanColumn)
            {
                var switchElement = contentView.Content as TMSwitch;
                switchElement?.SetBinding(TMSwitch.IsToggledProperty, new Binding(booleanColumn.PropertyName, source: BindingContext));
            }
            cell = contentView;
        }
        else
        {
            cell = new Label
            {
                TextColor = _textColor,
                FontSize = 14,
                BackgroundColor = _bgColor,
                VerticalTextAlignment = col.VerticalContentAlignment.ToTextAlignment(),
                HorizontalTextAlignment = col.HorizontalContentAlignment.ToTextAlignment(),
                LineBreakMode = col.LineBreakMode
            };

            if (!string.IsNullOrWhiteSpace(col.PropertyName))
            {
                cell.SetBinding(Label.TextProperty, new Binding(col.PropertyName, BindingMode.Default));
            }
        }
        return cell;
    }

    /// <summary>
    /// Update background color based on selected state
    /// </summary>
    private void UpdateBackgroundColor()
    {
        var rowIndex = GetRowIndex();

        if (rowIndex < 0)
        {
            return;
        }

        _bgColor = (DataGrid.SelectionMode != SelectionMode.None && _hasSelected && SelectedIndexes.Contains(rowIndex))
                ? DataGrid.ActiveRowColor
                : Colors.White;
        foreach (var v in Children)
        {
            if (v is View view)
            {
                view.BackgroundColor = _bgColor;

                if (view is Label label)
                {
                    label.TextColor = _textColor;
                }
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext != DataGrid.BindingContext)
        {
            CreateView();
        }
    }

    /// <inheritdoc/>
    protected override void OnParentSet()
    {
        base.OnParentSet();
        if (Parent == null)
        {
            DataGrid.ItemSelected -= DataGrid_ItemSelected;
        }
        else
        {
            DataGrid.ItemSelected += DataGrid_ItemSelected;
        }
    }
    private int GetRowIndex()
    {
        return DataGrid.InternalItems?.IndexOf(BindingContext) ?? -1;
    }

    /// <summary>
    /// Triggered when an item is selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGrid_ItemSelected(object? sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (DataGrid.SelectionMode == SelectionMode.None)
        {
            return;
        }
        var deselectedItems = e.PreviousSelection.Except(e.CurrentSelection).ToList();
        foreach (var deselectedItem in deselectedItems)
        {
            if (deselectedItem == BindingContext)
            {
                _hasSelected = false;
                SelectedIndexes.Remove(GetRowIndex());
                UpdateBackgroundColor();
                return;
            }
        }
        if (_hasSelected || (e.CurrentSelection.Count > 0))
        {
            foreach (var item in e.CurrentSelection)
            {
                if (item == BindingContext)
                {
                    _hasSelected = true;
                    SelectedIndexes.Add(GetRowIndex());
                    UpdateBackgroundColor();
                    return;
                }
            }
        }
    }

    #endregion Methods
}
