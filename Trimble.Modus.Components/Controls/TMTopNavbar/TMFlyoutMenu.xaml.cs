using System.Collections.ObjectModel;

namespace Trimble.Modus.Components;

public partial class TMFlyoutMenu : ContentPage
{
    public static readonly BindableProperty MenuItemsSourceProperty = BindableProperty.Create(nameof(MenuItemsSource),
        typeof(ObservableCollection<TMFlyoutMenuItem>),
        typeof(TMFlyoutMenu),
        defaultValue: null);

    public ObservableCollection<TMFlyoutMenuItem> MenuItemsSource
    {
        get => (ObservableCollection<TMFlyoutMenuItem>)GetValue(MenuItemsSourceProperty);
        set => SetValue(MenuItemsSourceProperty, value);
    }
    public TMFlyoutMenu()
    {
        InitializeComponent();
    }
}

