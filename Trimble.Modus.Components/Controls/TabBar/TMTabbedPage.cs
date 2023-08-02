using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace Trimble.Modus.Components;

[ContentProperty(nameof(TabItems))]
public partial class TMTabbedPage : ContentPage
{
    public ObservableCollection<TabViewItem> TabItems { get; set; } = new();

    public static readonly BindableProperty TabItemsSourceProperty =
            BindableProperty.Create(nameof(TabItemsSource),
                typeof(IList), typeof(TMTabbedPage), null,
                propertyChanged: OnTabItemsSourceChanged);

    public IList? TabItemsSource
    {
        get => (IList?)GetValue(TabItemsSourceProperty);
        set => SetValue(TabItemsSourceProperty, value);
    }

    private Grid mainContainer;
    private Grid tabStripContainer;
    private CarouselView contentContainer;

    public TMTabbedPage()
    {
        InitializeComponent();

        contentContainer = new CarouselView
        {
            BackgroundColor = Colors.Yellow,
            ItemsSource = TabItems.Where(t => t.ContentPage != null),
            ItemTemplate = new DataTemplate(() =>
            {
                var tabViewItemContent = new ContentView();
                tabViewItemContent.SetBinding(ContentProperty, "ContentPage.Content");
                return tabViewItemContent;
            }),
            IsSwipeEnabled = false,
            IsScrollAnimated = true,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            VerticalScrollBarVisibility = ScrollBarVisibility.Never,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand
        };

        tabStripContainer = new Grid
        {
            BackgroundColor = Colors.DeepPink,
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
    }

    static void OnTabItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {

    }

    private void TabItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Console.WriteLine(e.NewItems);
        TabItemsSource = TabItems;
        if (e.OldItems != null)
        {
            foreach (var tabViewItem in e.OldItems.OfType<TabViewItem>())
            {
                //ClearTabViewItem(tabViewItem);
            }
        }

        if (e.NewItems != null)
        {
            foreach (var tabViewItem in e.NewItems.OfType<TabViewItem>())
            {
                AddTabViewItem(tabViewItem, TabItems.IndexOf(tabViewItem));
            }
        }
    }

    void AddTabViewItem(TabViewItem item, int index = -1)
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
            FontAttributes = FontAttributes.Bold,
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

        tabStripContainer.Add(item, index, 0);
    }


    protected Grid _tabBarView = null;

    protected List<View> cells;
    protected List<View> selectedCells;

    protected void createTabBar()
    {
        _tabBarView = new Grid
        {
            HeightRequest = 50,
            RowSpacing = 0
        };

        _tabBarView.RowDefinitions.Add(new RowDefinition
        {
            Height = new GridLength(50, GridUnitType.Absolute)
        });
        _tabBarView.RowDefinitions.Add(new RowDefinition
        {
            Height = new GridLength(1, GridUnitType.Star)
        });

        _tabBarView.Add(new BoxView
        {
            BackgroundColor = Colors.Red
        }, 0, 0);


        cells = new List<View>();

        Grid gridTabs = new Grid()
        {
            ColumnSpacing = 0
        };
        int i = 0;
        /*
        foreach (Page page in Children)
        {
            if (i > 0)
            {
                gridTabs.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(SplitterWidth, GridUnitType.Absolute)
                });
                gridTabs.Add(new BoxView
                {
                    BackgroundColor = SplitterColor
                }, 2 * i - 1, 0);
            }

            gridTabs.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });

            View cell = (View)TabBarCellTemplate.CreateContent();
            cell.BindingContext = page.BindingContext == null ? page : page.BindingContext;
            cell.GestureRecognizers.Add(new TapGestureRecognizer
            {
                CommandParameter = cell,
                Command = SelectCellCommand
            });
            gridTabs.Add(cell, 2 * i, 0);
            cells.Add(cell);

            if (TabBarSelectedCellTemplate != null)
            {
                View selectedCell = (View)TabBarSelectedCellTemplate.CreateContent();
                selectedCell.BindingContext = cell.BindingContext;
                selectedCell.IsVisible = false;
                gridTabs.Add(selectedCell, 2 * i, 0);
                selectedCells.Add(selectedCell);
            }

            i++;
        }
        */

        _tabBarView.Add(gridTabs, 0, 1);
    }

    public Grid TabBarView
    {
        get
        {
            if (_tabBarView == null)
                createTabBar();
            return _tabBarView;
        }
    }
}