using System.Collections;
using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public partial class TMListView : ListView
{
    #region Bindable Properties  
    public static new readonly BindableProperty SelectionModeProperty =
             BindableProperty.Create(nameof(SelectionMode), typeof(ListSelectionMode), typeof(TMListView),propertyChanged:OnSelectionModeChanged);

    public static readonly BindableProperty SelectionChangedCommandProperty =
             BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(TMListView));
    #endregion
    #region Public properties  
    public new event EventHandler<object> ItemSelected;
    public ICommand SelectionChangedCommand
    {
        get => (ICommand)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }
    public List<object> selectableItems;
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
    private static void OnSelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tmlistview && newValue != null)
        {
            tmlistview.selectableItems.Clear();
        }
    }
    private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        switch (SelectionMode)
        {
            case ListSelectionMode.Multiple:
                var old = selectableItems;
                if (selectableItems.Contains(e.Item))
                {
                    selectableItems.Remove(e.Item);
                }
                else
                {
                    selectableItems.Add(e.Item);
                    ItemSelected?.Invoke(this, e.Item);
                    SelectionChangedCommand.Execute(e.Item);
                }
                break;

            case ListSelectionMode.Single:
                selectableItems.Clear();
                selectableItems.Add(e.Item);
                ItemSelected?.Invoke(this, e.Item);
                SelectionChangedCommand.Execute(e.Item);
                break;

            case ListSelectionMode.None:
            default:
                break;
        }
    }
    #endregion
}
