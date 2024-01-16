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
        flyoutMenu.collectionView.SelectionChanged += OnSelectionChanged;
    }

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

