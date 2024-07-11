using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Trimble.Modus.Components;

[ContentProperty(nameof(TabItems))]
public partial class TMTabbedPage : ContentPage
{
    #region Private Fields
    private Grid mainContainer;
    private Grid tabStripContainer;
    private CarouselView contentContainer;
    private ContentView contentViewContainer;
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

        tabStripContainer = new Grid
        {
            HeightRequest = 70,
            VerticalOptions = LayoutOptions.Fill
        };

        mainContainer = new Grid
        {
            RowSpacing = 0
        };
        mainContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
        mainContainer.RowDefinitions.Add(new RowDefinition { Height = 70 });

        // TODO: Having issue with caursel view. So fixed the tabbed page to work with content view.
        var isCurrentDevicePlatformIsWindows = DeviceInfo.Current.Platform == DevicePlatform.WinUI;
        if (isCurrentDevicePlatformIsWindows)
        {
            contentViewContainer = new ContentView();
            Grid.SetRow(contentViewContainer, 0);
            mainContainer.Children.Add(contentViewContainer);
        }
        else
        {
            contentContainer = new CarouselView
            {
                ItemsSource = TabItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var contentView = new ContentView();
                    contentView.SetBinding(ContentView.ContentProperty, "ContentView");
                    return contentView;
                }),

                // TODO: Disbaled Swipe and Scroll animation. While scrolling tabs updating wrong content.
                IsSwipeEnabled = false,
                IsScrollAnimated = false,
                Loop = false,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never,

            };
            contentContainer.PropertyChanged += OnContentContainerPropertyChanged;
            // TODO: Disbaled Swipe and Scroll animation. While scrolling tabs updating wrong content.
            //contentContainer.Scrolled += OnContentContainerScrolled;
            Grid.SetRow(contentContainer, 0);
            mainContainer.Children.Add(contentContainer);
        }
        UpdateBackgroundColor(this);
        mainContainer.Children.Add(tabStripContainer);
        Grid.SetRow(tabStripContainer, 1);

        Content = mainContainer;

        TabItems.CollectionChanged += TabItems_CollectionChanged;


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
                tabView.UpdateSelectedIndex((int)oldValue, selectedIndex);
        }
    }
    private static void OnTabColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMTabbedPage tabbedPage)
        {
            UpdateBackgroundColor(tabbedPage);
        }
    }

    private static void UpdateBackgroundColor(TMTabbedPage tabbedPage)
    {
        if (tabbedPage.TabColor == TabColor.Primary)
        {
            tabbedPage.SetDynamicResource(BackgroundColorProperty, "PrimaryTabBackgroundColor");
        }
        else
        {
            tabbedPage.tabStripContainer.SetDynamicResource(BackgroundColorProperty, "SecondaryTabBackgroundColor");
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
                SelectedIndex = selectedIndex;
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

    private void AddTabViewItem(TabViewItem tabViewItem, int index = -1)
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
        tabViewItem.TabColor = TabColor;
      
        tabViewItem.Orientation = Orientation;
        tabStripContainer.Add(tabViewItem, index, 0);
        AddSelectionTapRecognizer(tabViewItem);
        if (SelectedIndex < 0)
            SelectedIndex = 0;
        tabViewItem.UpdateTabColor(tabViewItem);
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
                    SelectedIndex = capturedIndex;
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

    private void UpdateSelectedIndex(int oldValue, int newValue)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (contentTabItems == null || contentTabItems.Count != TabItems.Count)
                contentTabItems = new ObservableCollection<TabViewItem>(TabItems.Where(t => t.Content != null));

            if (TabItems.Count > 0)
            { 
                for (var index = 0; index < TabItems.Count; index++)
                {
                    if (index == newValue)
                    {
                        TabItems[newValue].IsSelected = true;
                    }
                    else
                    {
                        TabItems[index].IsSelected = false;
                    }
                }
            }

            // TODO: Having issue with caursel view. So fixed the tabbed page to work with content view.
            if (contentViewContainer != null)
            {
                contentViewContainer.Content = TabItems[newValue].ContentView;
            }
            if (contentContainer != null)
            {
                if (tabStripContainer.Children.Count > 0)
                    contentContainer.ScrollTo(newValue, 1, ScrollToPosition.MakeVisible, false);

                contentContainer.Position = newValue;
            }

            if (oldValue != SelectedIndex)
            {
                var selectionChangedArgs = new TabSelectionChangedEventArgs()
                {
                    NewPosition = newValue,
                    OldPosition = oldValue
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
