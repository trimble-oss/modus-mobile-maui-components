using System.Collections.ObjectModel;

namespace Trimble.Modus.Components;

public partial class TMFlyoutPage : FlyoutPage
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
        typeof(ObservableCollection<TMFlyoutMenuItem>),
        typeof(TMFlyoutPage),
        propertyChanged: (_, newValue, oldValue) =>
        {
            var temp = (ObservableCollection<TMFlyoutMenuItem>)oldValue;

        });

    public ObservableCollection<TMFlyoutMenuItem> ItemsSource
    {
        get => (ObservableCollection<TMFlyoutMenuItem>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    public TMFlyoutPage()
    {
        InitializeComponent();
        if(flyoutMenu != null && flyoutMenu.collectionView != null)
            flyoutMenu.collectionView.SelectionChanged += OnSelectionChanged;
    }

    /// <summary>
    /// Find Titlebar and send reference to this flyout page to titlebar
    /// </summary>
    /// <param name="child"></param>
    protected override void OnChildAdded(Element child)
    {
        base.OnChildAdded(child);
        if(child is NavigationPage)
        {
            ContentPage contentPage = ((NavigationPage)child).CurrentPage as ContentPage;
            var titleBar = FindTMTitleBar(contentPage.Content);
            if(titleBar != null)
                titleBar.flyoutPageReference = this;
        }
    }

    /// <summary>
    /// Fiind title bar among children
    /// </summary>
    private TMTitleBar FindTMTitleBar(Element element)
    {
        if (element is TMTitleBar titleBar)
        {
            return titleBar;
        }

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                var result = FindTMTitleBar((Element)child);
                if (result != null)
                {
                    return result;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Navigate to page based on selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSelectionChanged(object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as TMFlyoutMenuItem;
        if (item != null)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
            IsPresented = false;
        }
    }
}

