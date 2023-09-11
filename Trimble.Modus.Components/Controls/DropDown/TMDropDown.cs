using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Shapes;
using System.Collections;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Popup.Animations;
using Trimble.Modus.Components.Popup.Services;
using Grid = Microsoft.Maui.Controls.Grid;
using StackLayout = Microsoft.Maui.Controls.StackLayout;

namespace Trimble.Modus.Components;
public class TMDropDown : ContentView
{
    private double desiredHeight;
    private Label label;
    private Border innerBorder;
    private StackLayout indicatorButton;
    private bool isVisible;
    private int radius = 15;
    private IEnumerable items;
    private int count = 0, margin = 96;
    private uint AnimationDuration { get; set; } = 250;
    public IEnumerable ItemsSource
    {
        get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }
    public List<object> SelectedItems
    {
        get => (List<object>)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }
    public new double WidthRequest
    {
        get { return (double)GetValue(WidthRequestProperty); }
        set { SetValue(WidthRequestProperty, value); }
    }
    public int SelectedIndex
    {
        get { return (int)GetValue(SelectedIndexProperty); }
        set { SetValue(SelectedIndexProperty, value); }
    }

    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(TMDropDown), 0, propertyChanged: OnSelectedIndexChanged);

    public new static readonly BindableProperty WidthRequestProperty =
        BindableProperty.Create(nameof(WidthRequest), typeof(double), typeof(TMDropDown), 240.0, propertyChanged: OnWidthRequestChanged);

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TMDropDown), null, propertyChanged: OnItemsSourceChanged);

    public static readonly BindableProperty SelectedItemsProperty =
        BindableProperty.Create(nameof(SelectedItems), typeof(List<object>), typeof(TMDropDown));

    public TMDropDown()
    {
        SelectedItems = new List<object> { };
        label = new Label
        {
            Text = "DropDown",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center,
            FontFamily = "OpenSansSemibold",
            TextColor = Colors.White,
            VerticalOptions = LayoutOptions.Center
        };
        var chevronIcon = new Image
        {
            Source = ImageConstants.ChevronDownIconWhite, 
            HeightRequest = 32,
            WidthRequest = 32,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
        };
        indicatorButton = new StackLayout()
        {
            Padding = new Thickness(0)
        };
        indicatorButton.Children.Add(chevronIcon);
        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
            },
            ColumnSpacing = 2,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center

        };

        grid.Add(label, 0, 0);
        grid.Add(indicatorButton, 1, 0);
        var stack = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,          
            Spacing = 2,
        };
        stack.Children.Add(label);
        stack.Children.Add(indicatorButton);
        innerBorder = new Border
        {
            Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue),
            StrokeThickness = 4,
            BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue),
            HeightRequest = 48,
            WidthRequest = 240,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(4)
            },
            Content = stack,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,

            Padding = new Thickness(0)
        };
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnTapped;

        innerBorder.GestureRecognizers.Add(tapGestureRecognizer);
        indicatorButton.GestureRecognizers.Add(tapGestureRecognizer);
        PopupService.Instance.Dismissed += OnPopupRemoved;
        Content = innerBorder;
    }

    private void OnSelected(object sender, SelectedItemChangedEventArgs e, ListView list)
    {
        if (SelectedItems != null)
        {
            SelectedItems.Clear();
            SelectedItems.Add(e.SelectedItem);
            UpdateCellColor(list);
        }
        label.Text = e.SelectedItem.ToString();
        if (count > 0 && PopupService.Instance.PopupStack.Count > 0)
        {
            
            PopupService.Instance?.DismissAsync();
            Close();
        }
        SelectedIndex = e.SelectedItemIndex;
        count = 1;
    }

    private void OnTapped(object sender, EventArgs e)
    {
        isVisible = !isVisible;
        if (isVisible)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private async void Close()
    {
        await Task.WhenAll(

            indicatorButton.RotateTo(0, AnimationDuration)
        );
        isVisible = false;
    }

    private async void Open()
    {
        var dataTemplate = new DataTemplate(() =>
        {
            var customCell = new DropDownViewCell();
            customCell.SetBinding(DropDownViewCell.TextProperty, ".");
            return customCell;
        });
        var listview = new ListView() { ItemsSource = ItemsSource, RowHeight = 48, ItemTemplate = dataTemplate };
        listview.ItemSelected += (sender, e) => OnSelected(sender, e, listview);
        listview.SelectedItem = listview.ItemsSource.Cast<object>().ToList()[SelectedIndex];
        Point offset = new Point(-1, 1);
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            radius = 3;
            offset = new Point(0, 2);
        }
        else if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            radius = 3;
        }

        var _shadow = new Shadow
        {
            Brush = Colors.Black,
            Radius = radius,
            Opacity = 0.6F,
            Offset = offset
        };
        var border = new Border()
        {
            Margin = new Thickness(0, margin, 0, 0),
            Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.DropDownListBorder),
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            StrokeThickness = 4,
            Shadow = _shadow,
            Padding = new Thickness(8),
            WidthRequest = 240,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(4)
            },
            Content = listview,
            IsVisible = true
        };

        border.HeightRequest = desiredHeight;
        border.WidthRequest = WidthRequest;
        var popup = new PopupPage(innerBorder, Enums.ModalPosition.Bottom)
        {
            Content = border,
            BackgroundColor = Colors.Transparent,
            Animation = new RevealAnimation(desiredHeight),
        };
        var locationFetcher = new LocationFetcher();
        var loc = locationFetcher.GetCoordinates(this);
        var height = Application.Current.MainPage.Window.Height;
        if (height - loc.Y < desiredHeight)
        {
            popup._position = Enums.ModalPosition.Top;
            border.Margin = new Thickness(0,-15,0,0);
        }
        await Task.WhenAll(
            indicatorButton.RotateTo(-180, AnimationDuration)
        );
        await PopupService.Instance?.PresentAsync(popup, true);
        if (count == 0)
        {
            var list = listview.ItemsSource.Cast<object>().ToList();
            var item = list[SelectedIndex];
            listview.SelectedItem = (object)item;
        }
        count = 1;
    }

    private void OnPopupRemoved(object sender, EventArgs e)
    {
        Close();
    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var dropDown = (TMDropDown)bindable;
        dropDown.items = newValue as IEnumerable;
        dropDown.UpdateListViewItemsSource(dropDown.items);
        dropDown.UpdateListBorderHeight(dropDown.items);
    }

    private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMDropDown dropDown && dropDown != null)
        {
            dropDown.UpdateSelectedItem((int)newValue, dropDown.items);
        }
    }
    private static void OnWidthRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMDropDown dropDown && dropDown != null)
        {
            dropDown.innerBorder.WidthRequest = (double)newValue;
        }
    }

    private void UpdateSelectedItem(int selectedIndex, IEnumerable items)
    {
        if (selectedIndex >= 0 && selectedIndex < items.Cast<object>().Count() && items != null)
        {
            label.Text = items.Cast<object>().ElementAt(selectedIndex).ToString();
        }
    }

    private void UpdateListViewItemsSource(IEnumerable items)
    {
        if (items != null)
        {
            UpdateSelectedItem(SelectedIndex, items);
        }
    }
    private void UpdateListBorderHeight(IEnumerable items)
    {
        if (items != null)
        {
            int itemCount = items.Cast<object>().Count();
            desiredHeight = 180.0;

            if (itemCount < 4)
            {
                desiredHeight = itemCount * 56;
                margin = ((itemCount - 1) * 56) - 28;
            }
        }
    }
    private void UpdateCellColor(ListView list)
    {

        foreach (var item in list.TemplatedItems)
        {
            if (item is DropDownViewCell textCell)
            {
                if (SelectedItems.Contains(textCell.BindingContext))
                {
                    textCell.UpdateBackgroundColor(ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale), true);
                }
                else
                {
                    textCell.UpdateBackgroundColor(Colors.White, false);
                }
            }
        }
    }
}
