using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

[ContentProperty(nameof(TabItems))]
public partial class TMTabbedPage : ContentPage
{
    #region Private Fields
    private Grid mainContainer;
    private Grid tabStripContainer;
    private CarouselView contentContainer;
    private ObservableCollection<TabViewItem>? contentTabItems;
    #endregion
    #region Public Fields
    public ObservableCollection<TabViewItem> TabItems { get; set; } = new();
    public delegate void TabSelectionChangedEventHandler(object? sender, TabSelectionChangedEventArgs e);
    public event TabSelectionChangedEventHandler? SelectionChanged;
    #endregion
    #region Bindable Properties
    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(TMTabbedPage), -1, BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);

    public static readonly BindableProperty OrientationProperty =
     BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(TMTabbedPage), StackOrientation.Vertical);

    public static readonly BindableProperty TabItemsSourceProperty =
            BindableProperty.Create(nameof(TabItemsSource), typeof(IList), typeof(TMTabbedPage), null);

    public static readonly BindableProperty TabColorProperty =
       BindableProperty.Create(nameof(TabColor), typeof(TabColor), typeof(TabViewItem), defaultValue: TabColor.Primary, propertyChanged: OnTabColorPropertyChanged);

    #endregion
    #region Public Fields
    public StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public TabColor TabColor
    {
        get => (TabColor)GetValue(TabColorProperty);
        set => SetValue(TabColorProperty, value);
    }
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }
    public IList? TabItemsSource
    {
        get => (IList?)GetValue(TabItemsSourceProperty);
        set => SetValue(TabItemsSourceProperty, value);
    }
    #endregion

    #region Constructor
    public TMTabbedPage()
    {
        InitializeComponent();

        contentContainer = new CarouselView
        {
            BackgroundColor = Colors.LightGray,
            ItemsSource = TabItems,
            ItemTemplate = new DataTemplate(() =>
            {
                var contentView = new ContentView();
                contentView.SetBinding(ContentView.ContentProperty, "ContentView");
                return contentView;
            }),
            IsSwipeEnabled = true,
            IsScrollAnimated = true,
            Loop = false,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            VerticalScrollBarVisibility = ScrollBarVisibility.Never,

        };

        tabStripContainer = new Grid
        {
            BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue),
            HeightRequest = 70,
            VerticalOptions = LayoutOptions.Fill
        };

        mainContainer = new Grid
        {
            BackgroundColor = Colors.Red,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            Children = { contentContainer, tabStripContainer },
            RowSpacing = 0
        };

        mainContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
        mainContainer.RowDefinitions.Add(new RowDefinition { Height = 70 });

        Grid.SetRow(contentContainer, 0);
        Grid.SetRow(tabStripContainer, 1);

        Content = mainContainer;

        TabItems.CollectionChanged += TabItems_CollectionChanged;
        contentContainer.PropertyChanged += OnContentContainerPropertyChanged;
        contentContainer.Scrolled += OnContentContainerScrolled;

    }
    #endregion
    #region Private Methods
    private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMTabbedPage tabView && tabView.TabItems != null)
        {
            var selectedIndex = (int)newValue;

            if (selectedIndex < 0)
            {
                return;
            }
            if ((int)oldValue != selectedIndex)
                tabView.UpdateSelectedIndex(selectedIndex);
        }
    }
    private static void OnTabColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMTabbedPage tabbedPage)
        {
            if ((TabColor)newValue == TabColor.Primary)
            {
                tabbedPage.tabStripContainer.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
            }
            else
            {
                tabbedPage.tabStripContainer.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
            }
        }
    }
    private void OnContentContainerScrolled(object? sender, ItemsViewScrolledEventArgs args)
    {
        for (var i = 0; i < TabItems.Count; i++)
            TabItems[i].UpdateCurrentContent();
    }

    private void OnContentContainerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CarouselView.ItemsSource)
            || e.PropertyName == nameof(CarouselView.VisibleViews))
        {
            var items = contentContainer.ItemsSource;

            UpdateItemsSource(items);
        }
        else if (e.PropertyName == nameof(CarouselView.Position))
        {
            var selectedIndex = contentContainer.Position;
            if (SelectedIndex != selectedIndex)
            {
                UpdateSelectedIndex(selectedIndex, true);
            }
        }
    }
    private void UpdateItemsSource(IEnumerable items)
    {

        if (contentContainer.VisibleViews.Count == 0)
            return;

        var contentWidth = contentContainer.VisibleViews.FirstOrDefault().Width;
        var tabItemsCount = items.Cast<object>().Count();

    }
    private void TabItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        TabItemsSource = TabItems;

        if (e.NewItems != null)
        {
            foreach (var tabViewItem in e.NewItems.OfType<TabViewItem>())
            {
                AddTabViewItem(tabViewItem, TabItems.IndexOf(tabViewItem));
            }
        }
    }

   private void AddTabViewItem(TabViewItem item, int index = -1)
    {
        var tabItem = new Grid()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand
        };
        tabItem.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
        tabItem.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var text = new Label
        {
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 6)
        };
        tabItem.Add(text);
        tabItem.SetRow(text, 1);

        tabStripContainer.ColumnDefinitions.Add(new ColumnDefinition()
        {
            Width = GridLength.Star
        });
        item.TabColor = TabColor;
        item.Orientation = Orientation;
        tabStripContainer.Add(item, index, 0);
        AddSelectionTapRecognizer(item);
        if (SelectedIndex != 0)
            UpdateSelectedIndex(0);
    }
    private void AddSelectionTapRecognizer(View view)
    {
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (sender, args) =>
        {
            if (sender is not View view)
                return;

            var capturedIndex = tabStripContainer.Children.IndexOf(view);

            if (view is TabViewItem tabViewItem)
            {
                var tabTappedEventArgs = new TabTappedEventArgs(capturedIndex);
                tabViewItem.OnTabTapped(tabTappedEventArgs);
            }

            if (CanUpdateSelectedIndex(capturedIndex))
            {
                if (SelectedIndex != capturedIndex)
                    UpdateSelectedIndex(capturedIndex);
            }
       
        };

        view.GestureRecognizers.Add(tapGestureRecognizer);
    }
    private bool CanUpdateSelectedIndex(int selectedIndex)
    {
        if (TabItems == null || TabItems.Count == 0)
            return true;

        var tabItem = TabItems[selectedIndex];

        if (tabItem != null && tabItem.Content == null)
        {
            var itemsCount = TabItems.Count;
            var contentItemsCount = TabItems.Count(t => t.Content == null);

            return itemsCount == contentItemsCount;
        }

        return true;
    }

    private void UpdateSelectedIndex(int position, bool hasCurrentItem = false)
    {
        if (position < 0)
            return;
        var oldposition = SelectedIndex;

        var newPosition = position;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (contentTabItems == null || contentTabItems.Count != TabItems.Count)
                contentTabItems = new ObservableCollection<TabViewItem>(TabItems.Where(t => t.Content != null));

            var contentIndex = position;
            var tabStripIndex = position;

            if (TabItems.Count > 0)
            {
                TabViewItem? currentItem = null;

                if (hasCurrentItem)
                    currentItem = (TabViewItem)contentContainer.CurrentItem;

                var tabViewItem = TabItems[position];

                contentIndex = contentTabItems.IndexOf(tabViewItem);
                tabStripIndex = TabItems.IndexOf(tabViewItem);

                position = tabStripIndex;

                for (var index = 0; index < TabItems.Count; index++)
                {
                    if (index == position)
                    {
                        TabItems[position].IsSelected = true;
                    }
                    else
                    {
                        TabItems[index].IsSelected = false;
                    }
                }
            }
            
            if (contentIndex >= 0)
                contentContainer.Position = contentIndex;

            if (tabStripContainer.Children.Count > 0)
                contentContainer.ScrollTo(tabStripIndex, 1, ScrollToPosition.MakeVisible, false);

            SelectedIndex = position;
            if (oldposition != SelectedIndex)
            {
                var selectionChangedArgs = new TabSelectionChangedEventArgs()
                {
                    NewPosition = newPosition,
                    OldPosition = oldposition
                };

                OnTabSelectionChanged(selectionChangedArgs);
            }
        });
    }
    #endregion
    #region Internal Methods
    internal virtual void OnTabSelectionChanged(TabSelectionChangedEventArgs e)
    {
        var handler = SelectionChanged;
        handler?.Invoke(this, e);
    }
    #endregion
}
