using System.Collections;
using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public partial class TMListView : ContentView
{
    #region Bindable Properties  
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TMListView), null, propertyChanged: OnItemSourceChanged);

    public static readonly BindableProperty ItemTemplateProperty =
        BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(TMListView));

    public static readonly BindableProperty SelectionModeProperty =
             BindableProperty.Create(nameof(SelectionMode), typeof(ListSelectionMode), typeof(TMListView));

    public static readonly BindableProperty ItemSelectedCommandProperty =
             BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(TMListView));

    #endregion
    #region Public properties  
    public event EventHandler<object> ItemSelected;
    public ICommand ItemSelectedCommand
    {
        get => (ICommand)GetValue(ItemSelectedCommandProperty);
        set => SetValue(ItemSelectedCommandProperty, value);
    }

    public List<object> selectableItems;
    public ListSelectionMode SelectionMode
    {
        get => (ListSelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
    }



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
    #endregion
    #region Constructor
    public TMListView()
    {
        InitializeComponent();
       
    }
    #endregion
    #region Protected Methods
    protected override void OnParentSet()
    {
        base.OnParentSet();
    }
    #endregion
    #region Private Methods
    private static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMListView tmlistview)
        {
            if (newValue is IEnumerable newCollection)
            {
                tmlistview.selectableItems = new List<object>();


                tmlistview.listView.ItemsSource = newCollection;
            }
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
                }
                break;

            case ListSelectionMode.Single:
                selectableItems.Clear();
                selectableItems.Add(e.Item);
                break;

            case ListSelectionMode.None:
            default:
                break;
        }
        ItemSelected?.Invoke(this, e.Item);
        ItemSelectedCommand.Execute(e.Item);
    }
    #endregion
}
