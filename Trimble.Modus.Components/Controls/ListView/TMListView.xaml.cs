using System.Collections;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMListView : ListView
{
    #region Private Fields
    private List<object> previousSelection;
    #endregion
    #region Bindable Properties  
    public static new readonly BindableProperty SelectionModeProperty =
             BindableProperty.Create(nameof(SelectionMode), typeof(ListSelectionMode), typeof(TMListView),propertyChanged: OnSelectionModeChanged);

    public static readonly BindableProperty SelectionChangedCommandProperty =
             BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(TMListView));

    public static readonly BindableProperty SelectableItemsProperty =
            BindableProperty.Create(nameof(SelectableItems), typeof(List<object>), typeof(TMListView),propertyChanged: OnSelectableItemsChanged);

    #endregion
    #region Public properties  
    public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    public ICommand SelectionChangedCommand
    {
        get => (ICommand)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }
    public List<object> SelectableItems
    {
        get => (List<object>)GetValue(SelectableItemsProperty);
        set => SetValue(SelectableItemsProperty, value);
        
    }
    public new ListSelectionMode SelectionMode
    {
        get => (ListSelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
        
    }
    public new IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion
    #region Constructor
    public TMListView()
    {
        HasUnevenRows = true;
        ItemTapped += ListViewItemTapped;
        (this as ListView)?.SetValue(ListView.SelectionModeProperty, ListViewSelectionMode.Single);  
        SelectableItems = new List<object> { };
    }
    #endregion
    #region Private Methods
    private void ListViewItemTapped(object sender, ItemTappedEventArgs e)
    {
        int selectedIndex = TemplatedItems.GetGlobalIndexOfItem(e.Item);
        previousSelection = new List<object>(SelectableItems);
        switch (SelectionMode)
        {
            case ListSelectionMode.Multiple:
                if (SelectableItems.Contains(e.Item))
                {
                    SelectableItems.Remove(e.Item);
                }
                else
                {
                    SelectableItems.Add(e.Item);
                }
                RaiseSelectionChangedEvent(previousSelection, selectedIndex);
                break;

            case ListSelectionMode.Single:
                SelectableItems.Clear();
                SelectableItems.Add(e.Item);
                RaiseSelectionChangedEvent(previousSelection, selectedIndex);
                break;

            case ListSelectionMode.None:
            default:
                break;
        }
        UpdateCellColor();
    }

    private static void OnSelectableItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tMListView && newValue != null)
        {
            tMListView.UpdateCellColor();
        }
    }
    private static void OnSelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tMListView && newValue != null)
        {
            tMListView.SelectableItems.Clear();
            tMListView.UpdateCellColor();
        }
    }

    private void RaiseSelectionChangedEvent(List<object> previousSelection, int index)
    {
        SelectionChangedEventArgs args = new SelectionChangedEventArgs
        {
            PreviousSelection = previousSelection.AsReadOnly(),
            CurrentSelection = SelectableItems.AsReadOnly(),
            SelectedIndex = index
        };

        SelectionChanged?.Invoke(this, args);
        SelectionChangedCommand?.Execute(args);
    }

    private void UpdateCellColor()
    {
        foreach (var item in this.TemplatedItems)
        {
            if (item is TextCell textCell)
            {
                if (SelectableItems.Contains(textCell.BindingContext))
                {
                    textCell.SetDynamicResource(TextCell.BackgrondColorProperty, "CellSelectedBackgroundColor");
                }
                else
                {
                    textCell.SetDynamicResource(TextCell.BackgrondColorProperty, "CellDefaultBackgroundColor");
                }
            }
            else if (item is TemplateCell templateCell)
            {
                if (SelectableItems.Contains(templateCell.BindingContext))
                {
                    templateCell.SetDynamicResource(TemplateCell.BackgrondColorProperty, "CellSelectedBackgroundColor");
                }
                else
                {
                    templateCell.SetDynamicResource(TemplateCell.BackgrondColorProperty, "CellDefaultBackgroundColor");
                }
            }
        }
    }
    #endregion
}
