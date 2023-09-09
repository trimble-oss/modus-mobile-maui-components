using Microsoft.Maui.Controls.Shapes;
using System.Collections;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Popup.Animations;
using Trimble.Modus.Components.Popup.Services;
using Grid = Microsoft.Maui.Controls.Grid;

namespace Trimble.Modus.Components;
public class TMDropDown : ContentView
{
    private double desiredHeight;
    private Label label;
    private PopupPage listPopup;
    private Border innerBorder, listBorder;
    private ListView listView;
    private ImageButton indicatorButton;
    private bool isVisible;
    private int radius = 15;
    private IEnumerable items;
    private int count = 0;
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
        BindableProperty.Create(nameof(WidthRequest), typeof(double), typeof(TMDropDown), double.NaN, propertyChanged: OnWidthRequestChanged);

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
        indicatorButton = new ImageButton
        {
            Source = ImageConstants.ChevronDownIconWhite,
            HeightRequest = 32,
            WidthRequest = 32,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(0)
        };

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
            Content = grid,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,

            Padding = new Thickness(10)
        };

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
        listView = new ListView
        {
            IsVisible = true,
            RowHeight = 48,
            ItemTemplate = new DataTemplate(() =>
            {
                var customCell = new DropDownViewCell();
                customCell.SetBinding(DropDownViewCell.TextProperty, ".");
                return customCell;
            }),
        };
        listView.ItemSelected += OnSelected;
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnTapped;
        innerBorder.GestureRecognizers.Add(tapGestureRecognizer);
        indicatorButton.GestureRecognizers.Add(tapGestureRecognizer);
        listBorder = new Border()
        {
            Margin = new Thickness(0, 96, 0, 0),
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
            Content = listView,
            IsVisible = true
        };
        listPopup = new PopupPage(innerBorder, Enums.ModalPosition.Bottom);
        listPopup.Content = listBorder;
        listPopup.BackgroundColor = Colors.Transparent;
        listPopup.Animation = null;
        PopupService.Instance.Dismissed += OnPopupRemoved;
        Content = innerBorder;
    }

    private void OnSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (SelectedItems != null)
        {
            SelectedItems.Clear();
            SelectedItems.Add(e.SelectedItem);
            UpdateCellColor();
        }
        label.Text = e.SelectedItem.ToString();
        if (count > 0)
        {
            PopupService.Instance.RemovePageAsync(listPopup, false);
            Close();
        }
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
        var locationFetcher = new LocationFetcher();
        var loc = locationFetcher.GetCoordinates(this);
        var height = Application.Current.MainPage.Window.Height;
        if (height - loc.Y < desiredHeight)
        {
            listPopup._position = Enums.ModalPosition.Top;
            listBorder.Margin = new Thickness(0);
        }
        else
        {
            listPopup._position = Enums.ModalPosition.Bottom;
        }
        await Task.WhenAll(
            indicatorButton.RotateTo(-180, AnimationDuration)
        );
        if (listPopup.Animation == null)
        {
            listPopup.Animation = new RevealAnimation(desiredHeight);
        }
        await PopupService.Instance.PresentAsync(listPopup, true);
        if (count == 0)
        {
            var list = listView.ItemsSource.Cast<object>().ToList();
            var item = list[SelectedIndex];
            Console.WriteLine(item.GetType());
            listView.SelectedItem = (object)item;

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
            dropDown.listBorder.WidthRequest = (double)newValue;
        }
    }

    private void UpdateSelectedItem(int selectedIndex, IEnumerable items)
    {
        if (selectedIndex >= 0 && selectedIndex < listView.ItemsSource.Cast<object>().Count() && listView != null)
        {
            label.Text = listView.ItemsSource.Cast<object>().ElementAt(selectedIndex).ToString();
        }
    }

    private void UpdateListViewItemsSource(IEnumerable items)
    {
        if (items != null)
        {
            listView.ItemsSource = items.Cast<object>().ToList();
            label.Text = (listView.ItemsSource as List<object>)?.FirstOrDefault()?.ToString();
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
                listBorder.HeightRequest = desiredHeight;
                var margin = ((itemCount - 1) * 56)-28;
                listBorder.Margin = new Thickness(0, margin, 0, 0);
            }
            else
            {
                listBorder.MaximumHeightRequest = desiredHeight;
            }
        }
    }
    private void UpdateCellColor()
    {
        foreach (var item in listView.TemplatedItems)
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
