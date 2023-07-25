using System.Collections;
using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public partial class TMListView : ListView
{
    #region Bindable Properties  
    public static new readonly BindableProperty SelectionModeProperty =
             BindableProperty.Create(nameof(SelectionMode), typeof(ListSelectionMode), typeof(TMListView),propertyChanged:OnSelectionModeChanged);

    public static readonly BindableProperty ItemSelectedCommandProperty =
             BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(TMListView));
    #endregion
    #region Public properties  
    public new event EventHandler<object> ItemSelected;
    public ICommand ItemSelectedCommand
    {
        get => (ICommand)GetValue(ItemSelectedCommandProperty);
        set => SetValue(ItemSelectedCommandProperty, value);
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
    #region Protected Methods
    protected override void OnParentSet()
    {
        base.OnParentSet();
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
                if (selectableItems.Contains(e.Item))
                {
                    selectableItems.Remove(e.Item);
                }
                else
                {
                    selectableItems.Add(e.Item);
                    ItemSelected?.Invoke(this, e.Item);
                    ItemSelectedCommand.Execute(e.Item);
                }
                break;

            case ListSelectionMode.Single:
                selectableItems.Clear();
                selectableItems.Add(e.Item);
                ItemSelected?.Invoke(this, e.Item);
                ItemSelectedCommand.Execute(e.Item);
                break;

            case ListSelectionMode.None:
            default:
                break;
        }
    }
    #endregion
}
