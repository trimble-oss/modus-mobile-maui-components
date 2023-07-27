using System.Collections;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMListView : ListView
{
    #region Private Fields
    private List<object> previousSelection, currentSelection;
    #endregion
    #region Bindable Properties  
    public static new readonly BindableProperty SelectionModeProperty =
             BindableProperty.Create(nameof(SelectionMode), typeof(ListSelectionMode), typeof(TMListView),propertyChanged: OnSelectionModeChanged);

    public static readonly BindableProperty SelectionChangedCommandProperty =
             BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(TMListView));

    public static readonly BindableProperty SelectableItemsProperty =
            BindableProperty.Create(nameof(selectableItems), typeof(List<object>), typeof(TMListView),propertyChanged: OnSelectableItemsChanged);

    #endregion
    #region Public properties  
    public new event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    public ICommand SelectionChangedCommand
    {
        get => (ICommand)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }
    public List<object> selectableItems
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
        ItemTapped += listView_ItemTapped;
        (this as ListView)?.SetValue(ListView.SelectionModeProperty, ListViewSelectionMode.None);  
        selectableItems = new List<object> { };
    }
    #endregion
    #region Private Methods
    private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        int selectedIndex = TemplatedItems.GetGlobalIndexOfItem(e.Item);
        previousSelection = new List<object>(selectableItems);
        switch (SelectionMode)
        {
            case ListSelectionMode.Multiple:
                if (selectableItems.Contains(e.Item))
                {
                    selectableItems.Remove(e.Item);
                }
                else
                {
                    selectableItems.Add(e.Item);
                    currentSelection = new List<object>(selectableItems);
                    RaiseSelectionChangedEvent(previousSelection, currentSelection,selectedIndex);
                }
                break;

            case ListSelectionMode.Single:
                selectableItems.Clear();
                selectableItems.Add(e.Item);
                currentSelection = new List<object>(selectableItems);
                RaiseSelectionChangedEvent(previousSelection, currentSelection,selectedIndex);
                break;

            case ListSelectionMode.None:
            default:
                break;
        }
        ChangeCellColor();
    }

    private static void OnSelectableItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tMListView && newValue != null)
        {
            tMListView.ChangeCellColor();
        }
    }
    private static void OnSelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tMListView && newValue != null)
        {
            tMListView.selectableItems.Clear();
        }
    }

    private void RaiseSelectionChangedEvent(List<object> previousSelection, List<object> currentSelection,int index)
    {
        SelectionChangedEventArgs args = new SelectionChangedEventArgs
        {
            PreviousSelection = previousSelection.AsReadOnly(),
            CurrentSelection = currentSelection.AsReadOnly(),
            SelectionIndex = index
        };

        SelectionChanged?.Invoke(this, args);
        SelectionChangedCommand?.Execute(args);
    }
    private void ChangeCellColor()
    {
        foreach (var item in this.TemplatedItems)
        {
            if (item is TextCell textCell)
            {
                if (selectableItems.Contains(textCell.BindingContext))
                {
                    textCell.UpdateBackgroundColor(ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale));
                }
                else
                {
                    textCell.UpdateBackgroundColor(Colors.White);
                }
            }
            else if (item is TemplateCell templateCell)
            {
                if (selectableItems.Contains(templateCell.BindingContext))
                {
                    templateCell.UpdateBackgroundColor(ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale));
                }
                else
                {
                    templateCell.UpdateBackgroundColor(Colors.White);
                }
            }
        }
    }
    #endregion
}
