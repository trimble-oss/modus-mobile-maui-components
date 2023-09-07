using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Controls.DataGridControl;
using System.Collections.Generic;

namespace Trimble.Modus.Components;
internal sealed class DataGridRow : Grid
{
    #region Fields

    private Color? _bgColor;
    private readonly Color? _textColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
    private bool _hasSelected;
    private DataGrid _dataGridReference;

    #endregion Fields
    #region Methods

    /// <summary>
    /// Create the container view
    /// </summary>
    private void CreateView()
    {
        if (Children.Count > 0)
            return;
        ColumnDefinitions.Clear();
        Children.Clear();

        SetStyling();

        for (var i = 0; i < _dataGridReference.Columns.Count; i++)
        {
            var col = _dataGridReference.Columns[i];

            ColumnDefinitions.Add(col.ColumnDefinition);

            if (!col.IsVisible)
            {
                continue;
            }

            var cell = CreateCell(col);

            SetColumn((BindableObject)cell, i);
            Children.Add(cell);
        }
        if (DeviceInfo.Idiom == DeviceIdiom.Desktop && _dataGridReference.SelectionMode == SelectionMode.Multiple)
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
        BackgroundColor = _dataGridReference.BorderColor;
        var borderThickness = _dataGridReference.ShowDivider? _dataGridReference.BorderThickness : new Thickness(0, 0, 0, 1);
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
            cell = new ContentView
            {
                BackgroundColor = _bgColor,
                Content = col.CellTemplate.CreateContent() as View
            };
            if(col is TextColumn)
            {
                var grid = (cell as ContentView).Content as Grid;
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
                var image = (cell as ContentView)?.Content as Image;
                image?.SetBinding(Image.SourceProperty, new Binding(imageColumn.PropertyName, source: BindingContext));
            }
            else if(col is BooleanColumn booleanColumn)
            {
                var switchElement = (cell as ContentView)?.Content as TMSwitch;
                switchElement?.SetBinding(TMSwitch.IsToggledProperty, new Binding(booleanColumn.PropertyName, source: BindingContext));
            }
        }
        else
        {
            cell = new Label
            {
                TextColor = _textColor,
                FontSize = 14,
                BackgroundColor = _bgColor,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
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

        _bgColor = _dataGridReference?.SelectionMode != SelectionMode.None && _hasSelected
                ? _dataGridReference.ActiveRowColor
                : Colors.White;
        ChangeColor(_bgColor, _textColor);
    }

    /// <summary>
    /// Change the color of the row
    /// </summary>
    /// <param name="bgColor"></param>
    /// <param name="textColor"></param>
    private void ChangeColor(Color? bgColor, Color? textColor)
    {
        foreach (var v in Children)
        {
            if (v is View view)
            {
                view.BackgroundColor = bgColor;

                if (view is Label label)
                {
                    label.TextColor = textColor;
                }
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnParentSet()
    {
        base.OnParentSet();
        
        Element parent = this;
        int iterations = 0;
        while (parent != null && !(parent is DataGrid) && iterations < 4)
        {
            parent = parent.Parent;
            iterations++;
        }

        if (parent is DataGrid dataGrid)
        {
            _dataGridReference = dataGrid;
        }
        CreateView();
        if (_dataGridReference.SelectionMode != SelectionMode.None)
        {
            if (Parent != null)
            {
                _dataGridReference.ItemSelected += DataGrid_ItemSelected;
            }
            else
            {
                _dataGridReference.ItemSelected -= DataGrid_ItemSelected;
            }
        }
    }
    private int GetRowIndex()
    {
        return _dataGridReference?.InternalItems?.IndexOf(BindingContext) ?? -1;
    }

    /// <summary>
    /// Triggered when an item is selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGrid_ItemSelected(object? sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (_dataGridReference.SelectionMode == SelectionMode.None)
        {
            return;
        }
        var deselectedItems = e.PreviousSelection.Except(e.CurrentSelection).ToList();
        foreach (var deselectedItem in deselectedItems)
        {
            if (deselectedItem == BindingContext)
            {
                _hasSelected = false;
                UpdateBackgroundColor();
                return;
            }
        }
        if (_hasSelected || (e.CurrentSelection.Count > 0))
        {
            foreach(var item in e.CurrentSelection)
            {
                if (item == BindingContext)
                {
                    _hasSelected = true;
                    UpdateBackgroundColor();
                    return;
                }
            }
        }
    }

    #endregion Methods
}
