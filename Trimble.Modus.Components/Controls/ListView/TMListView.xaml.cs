using System.Collections;
using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public partial class TMListView : ListView
{
    #region Private Fields
    private object previousSelection;
    private IList<object> previousSelections;
    #endregion
    #region Bindable Properties  
    public static new readonly BindableProperty SelectionModeProperty =
             BindableProperty.Create(nameof(SelectionMode), typeof(ListSelectionMode), typeof(TMListView), propertyChanged: OnSelectionModeChanged);

    public static readonly BindableProperty SelectionChangedCommandProperty =
             BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(TMListView));

    public static new readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(TMListView), propertyChanged: OnSelectedItemChanged);

    public static readonly BindableProperty SelectedItemsProperty =
            BindableProperty.Create(nameof(SelectedItems), typeof(IList<object>), typeof(TMListView), propertyChanged: OnSelectedItemsChanged);

    #endregion
    #region Public properties  
    public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    public ICommand SelectionChangedCommand
    {
        get => (ICommand)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }
    public IList<object> SelectedItems
    {
        get => (IList<object>)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);

    }

    public new object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public new ListSelectionMode SelectionMode
    {
        get => (ListSelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);

    }
    //public new IEnumerable ItemsSource
    //{
    //    get => (IEnumerable)GetValue(ItemsSourceProperty);
    //    set => SetValue(ItemsSourceProperty, value);
    //}
    #endregion
    #region Constructor
    public TMListView()
    {
        HasUnevenRows = true;
        ItemTapped += ListViewItemTapped;
        (this as ListView)?.SetValue(ListView.SelectionModeProperty, ListViewSelectionMode.Single);
    }
    #endregion
    #region Private Methods
    private void ListViewItemTapped(object sender, ItemTappedEventArgs e)
    {
        UpdateSelectedItemTappedValue(e.Item);
    }

    private void UpdateSelectedItemTappedValue(object newValue)
    {
        switch (SelectionMode)
        {
            case ListSelectionMode.Multiple:
                var previousSelections = SelectedItems;
                if (SelectedItems.Contains(newValue))
                {
                    SelectedItems.Remove(newValue);
                }
                else
                {
                    SelectedItems.Add(newValue);
                }
                UpdatedItems(previousSelections, SelectedItems);
                break;

            case ListSelectionMode.Single:
                var previousValue = SelectedItem;
                SelectedItem = newValue;
                UpdatedItem(previousValue, SelectedItem);
                break;

            default:
                break;
        }
    }

    private void UpdatedItem(object oldValue, object newValue)
    {
        if (ItemsSource != null && newValue != null)
        {
            int selectedIndex = ((IList)ItemsSource).IndexOf(newValue);
            int oldIndex = ((IList)ItemsSource).IndexOf(oldValue);
            if (oldIndex != selectedIndex)
            {
                previousSelection = oldValue;
            }
            RaiseSelectionChangedEvent(previousSelection, selectedIndex);
            UpdateCellColor();
        }
    }

    private void UpdatedItems(object oldValue, object newValue)
    {
        int selectedIndex = TemplatedItems.GetGlobalIndexOfItem(newValue);
        previousSelections = (IList<object>)oldValue;
        RaiseSelectionChangedEvent(previousSelections, selectedIndex);

        UpdateCellColor();
    }

    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tMListView)
        {
            tMListView.UpdatedItem(oldValue, newValue);
        }
    }

    private static void OnSelectedItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tMListView)
        {
            tMListView.UpdatedItems(oldValue, newValue);
        }
    }

    private static void OnSelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tMListView && newValue is ListSelectionMode selectionMode)
        {
            if (selectionMode == ListSelectionMode.Single)
            {
                tMListView.UpdatedItem(tMListView.previousSelection, tMListView.SelectedItem);
            }
            else if (selectionMode == ListSelectionMode.Multiple)
            {
                tMListView.UpdatedItems(tMListView.previousSelections, tMListView.SelectedItems);
            }
            else
            {
                tMListView.UpdateCellColor();
            }
        }
    }

    private void RaiseSelectionChangedEvent(object previousSelection, int index)
    {
        SelectionChangedEventArgs args = new SelectionChangedEventArgs
        {
            PreviousSelection = previousSelection,
            CurrentSelection = SelectionMode == ListSelectionMode.Multiple ? SelectedItems : SelectedItem,
            SelectedIndex = index
        };

        SelectionChanged?.Invoke(this, args);
        SelectionChangedCommand?.Execute(args);
    }

    private void UpdateCellColor()
    {
        foreach (var item in this.TemplatedItems)
        {
            Updated(item as ViewCell);
        }
    }

    private void Updated(ViewCell textCell)
    {
        if (SelectionMode == ListSelectionMode.Single)
        {
            if (SelectedItem != null && SelectedItem.Equals(textCell.BindingContext))
            {
                textCell.SetDynamicResource(TextCell.BackgrondColorProperty, "CellSelectedBackgroundColor");
            }
            else
            {
                textCell.SetDynamicResource(TextCell.BackgrondColorProperty, "CellDefaultBackgroundColor");
            }
        }
        else if (SelectionMode == ListSelectionMode.Multiple)
        {
            if (SelectedItems != null && SelectedItems.Contains(textCell.BindingContext))
            {
                textCell.SetDynamicResource(TextCell.BackgrondColorProperty, "CellSelectedBackgroundColor");
            }
            else
            {
                textCell.SetDynamicResource(TextCell.BackgrondColorProperty, "CellDefaultBackgroundColor");
            }
        }
        else
        {
            textCell.SetDynamicResource(TextCell.BackgrondColorProperty, "CellDefaultBackgroundColor");
        }
    }
    #endregion
}
