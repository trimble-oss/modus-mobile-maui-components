using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using System.Collections;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMListView : ContentView
{
    private List<SelectableItem<object>> selectedItems;
    private SelectableItem<object> singleSelectedItem;
    public static readonly BindableProperty ItemsSourceProperty =
    BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TMListView), null, propertyChanged: OnItemSourceChanged);

    public static readonly BindableProperty SelectableItemsSourceProperty =
   BindableProperty.Create(nameof(SelectableItemSource), typeof(IEnumerable), typeof(TMListView), null);

    public ListSelectionMode ListSelectionMode { get; set; }
  
    private static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is IEnumerable newCollection)
        {
            var selectableItems = new List<SelectableItem<object>>();

            foreach (var item in newCollection)
            {
                var selectableItem = new SelectableItem<object>(item, false);
                selectableItems.Add(selectableItem);

            }

            if (bindable is TMListView tMListView)
            {
                tMListView.SelectableItemSource = selectableItems;
            }

        }

    }

    public IEnumerable SelectableItemSource
    {
        get => (IEnumerable)GetValue(SelectableItemsSourceProperty);
        set => SetValue(SelectableItemsSourceProperty, value);
    }
    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }


    public static readonly BindableProperty CustomTemplateProperty = BindableProperty.Create(
        nameof(ItemTemplate),
        typeof(DataTemplate),
        typeof(TMListView));
    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(CustomTemplateProperty);
        set => SetValue(CustomTemplateProperty, value);
    }

    public TMListView()
    {
        InitializeComponent();
        selectedItems = new List<SelectableItem<object>>();
      

    }


    private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

        ListSelectionMode = ListSelectionMode.Multiple;

        if (e.SelectedItem is SelectableItem<object> selectedItem)
        {

            if (ListSelectionMode == ListSelectionMode.Multiple)
            {
                    Console.WriteLine("Multiple");
                    selectedItem.IsSelected = !selectedItem.IsSelected;
                    Console.WriteLine(selectedItem.IsSelected);

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
                        Console.WriteLine(selectedItems.Count);
                    }

                
            }
            if (ListSelectionMode == ListSelectionMode.Single)
            {
                Console.WriteLine("Single");
                //  listView.SelectionMode = ListViewSelectionMode.Single;
                Console.WriteLine(selectedItems.Count);
                if (selectedItems.Count > 0)
                {
                    Console.WriteLine("if"+selectedItems.Count);
                    foreach (var item in selectedItems)
                    {
                        item.IsSelected = false;
                       
                    }
                }
                selectedItems.Clear();
                selectedItem.IsSelected = true;
                selectedItems.Add(selectedItem);

            }
            if (ListSelectionMode == ListSelectionMode.ReadOnly)
            {
                Console.WriteLine("Readonly");
                listView.SelectionMode = ListViewSelectionMode.None;
            }
        }
    }
}
