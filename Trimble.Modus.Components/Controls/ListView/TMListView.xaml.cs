using System.Collections;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public partial class TMListView : ContentView
{
   
    private List<SelectableItem<object>> selectedItems;

    private int _selectedItemCount = 0;

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TMListView), null, propertyChanged: OnItemSourceChanged);

    public static readonly BindableProperty SelectableItemsSourceProperty =
        BindableProperty.Create(nameof(SelectableItemSource), typeof(IEnumerable), typeof(TMListView), null, propertyChanged: OnSelectableItemSourceChanged);
    
    public static readonly BindableProperty ItemTemplateProperty =
        BindableProperty.Create(nameof(ItemTemplate),typeof(DataTemplate),typeof(TMListView));

    public event EventHandler<SelectableItemEventArgs> ItemSelected;

    public ListSelectionMode SelectionMode { get; set; }

    public List<SelectableItem<object>> selectableItems;

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }
    internal IEnumerable SelectableItemSource
    {
        get => (IEnumerable)GetValue(SelectableItemsSourceProperty);
        set => SetValue(SelectableItemsSourceProperty, value);
    }

    public TMListView()
    {
        InitializeComponent();
        selectedItems = new List<SelectableItem<object>>();
    }


    private void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

        if (e.SelectedItem is SelectableItem<object> selectedItem)
        {

            switch (SelectionMode)
            {
                case ListSelectionMode.Multiple:
                    selectedItem.IsSelected = !selectedItem.IsSelected;

                    if (selectedItem.IsSelected)
                    {
                        selectedItems.Add(selectedItem);
                    }
                    else
                    {
                        selectedItems.Remove(selectedItem);
                    }
                    foreach (var item in selectedItems)
                    {
                        _selectedItemCount = selectedItems.Count;
                    }
                    break;

                case ListSelectionMode.Single:
                    if (selectedItems.Count > 0)
                    {
                        foreach (var item in selectedItems)
                        {
                            item.IsSelected = false;

                        }
                    }
                    selectedItems.Clear();
                    selectedItem.IsSelected = true;
                    selectedItems.Add(selectedItem);
                    _selectedItemCount = 1;

                    break;

                case ListSelectionMode.ReadOnly:
                    listView.SelectionMode = ListViewSelectionMode.None;
                    _selectedItemCount = 0;
                    break;
                default:
                    break;
            }

            ItemSelected?.Invoke(this, GetValueFromItemsSource(ItemsSource, e.SelectedItemIndex, _selectedItemCount));
        }

    }

    private static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tmlistview)
        {
            if (newValue is IEnumerable newCollection)
            {
                tmlistview.selectableItems = new List<SelectableItem<object>>();

                foreach (var item in newCollection)
                {
                    var selectableItem = new SelectableItem<object>(item, false);
                    tmlistview.selectableItems.Add(selectableItem);
                }
                tmlistview.SelectableItemSource = tmlistview.selectableItems;
            }
        }

    }
    private static void OnSelectableItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tmlistview)
        {
            if (newValue is IEnumerable value)
            {
                tmlistview.listView.ItemsSource = value;
            }
        }
    }

    private SelectableItemEventArgs GetValueFromItemsSource(IEnumerable itemsSource, int selectedItemIndex,int selectedItemsCount)
    {
        if (itemsSource == null || selectedItemIndex < 0)
            return null;

        if (itemsSource is IList list)
        {
            if (selectedItemIndex < list.Count)
            {
                var selectedItem = list[selectedItemIndex];
                return new SelectableItemEventArgs(selectedItem, selectedItemIndex, selectedItemsCount);
            }
        }
        else
        {
            var enumerator = itemsSource.GetEnumerator();
            var currentIndex = 0;

            while (enumerator.MoveNext())
            {
                if (currentIndex == selectedItemIndex)
                {
                    var selectedItem = enumerator.Current;
                    return new SelectableItemEventArgs(selectedItem, selectedItemIndex, selectedItemsCount);
                }

                currentIndex++;
            }
        }

        return null;
    }
}
