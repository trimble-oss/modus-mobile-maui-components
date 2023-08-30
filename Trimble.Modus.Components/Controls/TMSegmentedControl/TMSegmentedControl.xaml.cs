using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Model;

namespace Trimble.Modus.Components;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TMSegmentedControl : ContentView
{
    #region Public properties
    [
        EditorBrowsable(EditorBrowsableState.Never),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
    ]
    public ObservableCollection<SegmentItemModel> SegmentedItems { get; set; } =
        new ObservableCollection<SegmentItemModel>();

    /// <summary>
    /// Event raised when the selected index is changed
    /// </summary>
    public event EventHandler<SelectedIndexEventArgs> SelectedIndexChanged;

    /// <summary>
    /// Items to be displayed in the segmented control
    /// </summary>
    public IEnumerable ItemsSource
    {
        get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// Selected index of the segmented control
    /// </summary>
    public int SelectedIndex
    {
        get { return (int)GetValue(SelectedIndexProperty); }
        set { SetValue(SelectedIndexProperty, value); }
    }

    /// <summary>
    /// Indicates whether the segmented control is rounded or not
    /// </summary>
    public bool IsRounded
    {
        get { return (bool)GetValue(IsRoundedProperty); }
        set { SetValue(IsRoundedProperty, value); }
    }

    /// <summary>
    /// Corner radius of the segmented control
    /// </summary>
    public CornerRadius CornerRadius
    {
        get; set;
    } = 8;

    /// <summary>
    /// Color theme of the segmented control
    /// </summary>
    public SegmentColorTheme ColorTheme
    {
        get => (SegmentColorTheme)GetValue(ColorThemeProperty);
        set => SetValue(ColorThemeProperty, value);
    }

    /// <summary>
    /// Enable or disable the segmented control
    /// </summary>
    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set { SetValue(IsEnabledProperty, value); }
    }

    /// <summary>
    /// Size of the segmented control
    /// </summary>
    public SegmentedControlSize Size
    {
        get => (SegmentedControlSize)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Command to be executed when the selected index is changed
    /// </summary>
    public ICommand SelectedIndexChangedCommand
    {
        get { return (ICommand)GetValue(SelectedIndexChangedCommandProperty); }
        set { SetValue(SelectedIndexChangedCommandProperty, value); }
    }
    #endregion
    #region Bindable properties
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TMSegmentedControl), null, propertyChanged: OnItemSourceChanged, defaultBindingMode: BindingMode.TwoWay);
    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(TMSegmentedControl), 0, BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);
    public static readonly BindableProperty IsRoundedProperty = BindableProperty.Create(nameof(IsRounded), typeof(bool), typeof(TMSegmentedControl), false, propertyChanged: OnRoundedPropertyChanged);
    public static readonly BindableProperty ColorThemeProperty = BindableProperty.Create(nameof(ColorTheme), typeof(SegmentColorTheme), typeof(TMSegmentedControl), SegmentColorTheme.Primary, BindingMode.TwoWay, propertyChanged: OnColorThemeChanged);
    public static new readonly BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMSegmentedControl), true, propertyChanged: OnEnabledStateChanged);
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(SegmentedControlSize), typeof(TMSegmentedControl), defaultValue: SegmentedControlSize.Small, BindingMode.TwoWay, propertyChanged: OnSizeChanged);
    public static readonly BindableProperty SelectedIndexChangedCommandProperty = BindableProperty.Create(nameof(SelectedIndexChangedCommand), typeof(ICommand), typeof(TMSegmentedControl));
    #endregion

    #region Property change handlers
    /// <summary>
    /// Update radius of the segmented control when the size/ IsRounded property is changed
    /// </summary>
    private static void OnRoundedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var segmentedControl = bindable as TMSegmentedControl;
        if (segmentedControl.IsRounded)
        {
            segmentedControl.CornerRadius = segmentedControl.FrameView.HeightRequest / 2;
            segmentedControl.FrameView.StrokeShape = new RoundRectangle()
            {
                CornerRadius = segmentedControl.FrameView.HeightRequest / 2
            };
        }
        else
        {
            segmentedControl.FrameView.StrokeShape = new RoundRectangle()
            {
                CornerRadius = 8
            };
        }
    }

    /// <summary>
    /// Method that is called when the size of the segmented control is changed.
    /// </summary>
    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSegmentedControl segmentedControl)
        {
            switch ((SegmentedControlSize)newValue)
            {
                case SegmentedControlSize.Small:
                    segmentedControl.FrameView.HeightRequest = 32;
                    break;
                case SegmentedControlSize.Medium:
                    segmentedControl.FrameView.HeightRequest = 40;
                    break;
                case SegmentedControlSize.Large:
                    segmentedControl.FrameView.HeightRequest = 48;
                    break;
                case SegmentedControlSize.XLarge:
                    segmentedControl.FrameView.HeightRequest = 56;
                    break;
            }
            foreach (var item in segmentedControl.SegmentedItems)
            {
                item.Size = (SegmentedControlSize)newValue;
            }
            OnRoundedPropertyChanged(bindable, oldValue, newValue);
        }
    }

    /// <summary>
    /// Invoked when the enabled state of the segmented control changes.
    /// </summary>
    /// <param name="bindable">The segmented control whose enabled state changed.</param>
    /// <param name="oldValue">The old value of the enabled state.</param>
    /// <param name="newValue">The new value of the enabled state.</param>
    private static void OnEnabledStateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSegmentedControl segmentedControl)
        {
            segmentedControl.Opacity = segmentedControl.IsEnabled ? 1 : 0.5;
            foreach (TMSegmentedItem item in segmentedControl.TabButtonHolder.Children)
            {
                if (!(bool)newValue)
                {
                    item.GestureRecognizers.Clear();
                }
                else
                {
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += segmentedControl.OnSegmentedItemTapped;

                    if (!item.GestureRecognizers.Contains(tapGestureRecognizer))
                        item.GestureRecognizers.Add(tapGestureRecognizer);
                }
            }
        }
    }

    /// <summary>
    /// Invoked when the color theme of the segmented control changes.
    /// </summary>
    private static void OnColorThemeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSegmentedControl segmentedControl)
        {
            foreach (var item in segmentedControl.SegmentedItems)
            {
                item.ColorTheme = segmentedControl.ColorTheme;
            }
        }
    }
    private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as TMSegmentedControl)?.UpdateSelectedIndex();
    }

    /// <summary>
    /// Invoked when the items source of the segmented control changes.
    /// </summary>
    private static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var segmentedControl = bindable as TMSegmentedControl;
        segmentedControl.ClearSegmentedItems();
        if (newValue == null)
        {
            return;
        }
        if (oldValue is INotifyCollectionChanged oldCollection)
        {
            oldCollection.CollectionChanged -= segmentedControl.OnItemsSourceCollectionChanged;
        }
        if (newValue is INotifyCollectionChanged newCollection)
        {
            newCollection.CollectionChanged += segmentedControl.OnItemsSourceCollectionChanged;
        }
        segmentedControl.PopulateItems();
        segmentedControl.UpdateIsEnabled();
        segmentedControl.UpdateSelectedIndex();
    }
    #endregion

    #region Constructor
    public TMSegmentedControl()
    {
        InitializeComponent();
        UpdateIsEnabled();
    }

    #endregion
    #region Private methods
    /// <summary>
    /// Populate Segment items in items source in the group
    /// </summary>
    private void PopulateItems()
    {
        if (ItemsSource == null)
            return;
        foreach (var item in ItemsSource)
        {
            SegmentItemModel newTab =
                new()
                {
                    ItemIndex = SegmentedItems.Count,
                    ColorTheme = ColorTheme,
                    IsSelected = SegmentedItems.Count == SelectedIndex,
                    ShowSeparator = SegmentedItems.Count != 0,
                    Size = Size
                };
            if (item is ImageSource)
            {
                newTab.Icon = (ImageSource)item;
            }
            else if (item is string)
            {
                newTab.Text = (string)item;
            }
            else if (item is SegmentedItem segmentedItem)
            {

                newTab.Text = segmentedItem.Text;
                newTab.Icon = segmentedItem.IconSource;
            }
            else
            {
                newTab.Text = item.ToString();
            }
            UpdateSegmentedItem(newTab);
            SegmentedItems.Add(newTab);
        }
    }

    /// <summary>
    /// Called when ItemsSource is changed
    /// </summary>
    private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            HandleAdd(e);
        else if (e.Action == NotifyCollectionChangedAction.Remove)
            HandleRemove(e);
        else if (e.Action == NotifyCollectionChangedAction.Reset)
            ClearSegmentedItems();
    }

    /// <summary>
    /// Update the segmented control when a new item is added to the items source.
    /// </summary>
    private void UpdateIsEnabled()
    {
        if (TabButtonHolder == null)
            return;

        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnSegmentedItemTapped;

        foreach (var children in TabButtonHolder.Children)
        {
            if (children is TMSegmentedItem segmentedItem)
            {
                if (IsEnabled)
                {
                    if (!segmentedItem.GestureRecognizers.Contains(tapGestureRecognizer))
                        segmentedItem.GestureRecognizers.Add(tapGestureRecognizer);
                }
                else
                {
                    segmentedItem.GestureRecognizers.Clear();
                }
            }
        }
    }

    /// <summary>
    /// Update the UI and state of the segment items when selected index is changed.
    /// </summary>
    private void UpdateSelectedIndex()
    {
        var selectedIndexEventArgs = new SelectedIndexEventArgs(SelectedIndex);
        SelectedIndexChanged?.Invoke(this, selectedIndexEventArgs);
        if (SelectedIndexChangedCommand != null && SelectedIndexChangedCommand.CanExecute(SelectedIndex))
        {
            SelectedIndexChangedCommand.Execute(SelectedIndex);
        }

        for (int i = 0; i < SegmentedItems.Count; i++)
        {
            SegmentedItems[i].IsSelected = SelectedIndex == i;
            SegmentedItems[i].ShowSeparator = (i != 0) && (SelectedIndex != i);
        }
        if (SelectedIndex < SegmentedItems.Count - 1)
        {
            SegmentedItems[SelectedIndex + 1].ShowSeparator = false;
        }
    }

    /// <summary>
    /// Handle changes to SegmentedItems collection
    /// </summary>
    private void OnSegmentedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (TMSegmentedItem oldSegmentedItem in e.OldItems)
            {
                ClearSegmentedItem(oldSegmentedItem);
            }
        }

        if (e.NewItems != null)
        {
            foreach (SegmentItemModel newSegmentedItem in e.NewItems)
            {
                UpdateSegmentedItem(newSegmentedItem);
            }
        }

        UpdateIsEnabled();
        UpdateSelectedIndex();
    }

    /// <summary>
    /// Triggered when a segmented item is tapped.
    /// </summary>
    private void OnSegmentedItemTapped(object sender, EventArgs e)
    {
        if (sender is TMSegmentedItem segmentedItem)
        {
            SelectedIndex = TabButtonHolder.Children.IndexOf(segmentedItem);

            int index = 0;

            foreach (TMSegmentedItem child in TabButtonHolder.Children.OfType<TMSegmentedItem>())
            {
                child.IsSelected = SelectedIndex == index;
                index++;
            }
        }
    }

    /// <summary>
    /// Clear the segmented items in the group
    /// </summary>
    private void ClearSegmentedItems()
    {
        if (TabButtonHolder == null)
            return;

        SegmentedItems.Clear();
    }

    /// <summary>
    /// Remove the segmented item from the group
    /// </summary>
    /// <param name="segmentedItem"></param>
    private void ClearSegmentedItem(TMSegmentedItem segmentedItem)
    {
        if (TabButtonHolder == null)
            return;
        TabButtonHolder.Children.Remove(segmentedItem);
    }

    /// <summary>
    /// Update the segmented item in the group
    /// </summary>
    /// <param name="segmentedItem"></param>
    private void UpdateSegmentedItem(SegmentItemModel segmentedItem)
    {
        segmentedItem.ColorTheme = ColorTheme;
    }

    /// <summary>
    /// Handle the add event when items are added to the collection
    /// </summary>
    /// <param name="e">The event arguments</param>
    private void HandleAdd(NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in e.NewItems)
        {
            SegmentItemModel newTab = new();

            newTab.ItemIndex = SegmentedItems.Count;
            newTab.ColorTheme = ColorTheme;
            newTab.IsSelected = SegmentedItems.Count == SelectedIndex;
            newTab.ShowSeparator = SegmentedItems.Count != 0;
            if (item is ImageSource itemIconSource)
            {
                newTab.Icon = itemIconSource;
            }
            else if (item is string itemText)
            {
                newTab.Text = itemText;
            }
            else
            {
                newTab.Text = item.ToString();
            }
            UpdateSegmentedItem(newTab);
            SegmentedItems.Add(newTab);
        }
    }

    /// <summary>
    /// Handle the remove event when items are removed from the collection
    /// </summary>
    /// <param name="e">The event arguments</param>
    private void HandleRemove(NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in e.OldItems)
        {
            int index = -1;
            if (item is ImageSource itemIconSource)
            {
                index = SegmentedItems.IndexOf(
                    SegmentedItems.FirstOrDefault(x => x.Icon == itemIconSource)
                );
            }
            else if (item is string itemText)
            {
                index = SegmentedItems.IndexOf(
                    SegmentedItems.FirstOrDefault(x => x.Text == itemText)
                );
            }
            else
            {
                index = SegmentedItems.IndexOf(
                    SegmentedItems.FirstOrDefault(x => x.Text == item.ToString())
                );
            }
            if (index != -1)
            {
                SegmentedItems.RemoveAt(index);
            }
        }
    }
    #endregion
}
