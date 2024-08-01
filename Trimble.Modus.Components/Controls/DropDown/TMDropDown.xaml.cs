using System.Collections;
using System.Windows.Input;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components;
public partial class TMDropDown : ContentView
{
    #region Private fields
    private double desiredHeight;
    //private Label label;
    //private Border innerBorder;
    //private StackLayout indicatorButton;
    private IEnumerable items;

#if WINDOWS
    private Thickness margin = new Thickness(0, 154, 0, 0);

#else
    private Thickness margin = new Thickness(0, 128, 0, 0);
#endif
    private uint AnimationDuration { get; set; } = 250;
    private object previousSelection;

    #endregion
    #region Public Properties
    public event EventHandler<DropDownSelectionChangedEventArgs> SelectionChanged;
    public IEnumerable ItemsSource
    {
        get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    internal new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    internal Color BorderColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    internal Color TextAndIconColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
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
    public ICommand SelectionChangedCommand
    {
        get => (ICommand)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }
    #endregion
    #region Bindable Properties
    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(TMDropDown), 0, propertyChanged: OnSelectedIndexChanged);

    public new static readonly BindableProperty WidthRequestProperty =
        BindableProperty.Create(nameof(WidthRequest), typeof(double), typeof(TMDropDown), 240.0, propertyChanged: OnWidthRequestChanged);

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TMDropDown), null, propertyChanged: OnItemsSourceChanged);

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(TMDropDown));

    public static readonly BindableProperty SelectionChangedCommandProperty =
         BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(TMDropDown));

    public new static readonly BindableProperty BackgroundColorProperty =
     BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(TMDropDown), Colors.Transparent, BindingMode.Default, null, propertyChanged: OnBackgroundColorPropertyChanged);

    public static readonly BindableProperty BorderColorProperty =
    BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(TMDropDown), Colors.Transparent, BindingMode.Default, null, propertyChanged: OnBorderColorPropertyChanged);


    public static readonly BindableProperty TextAndIconColorProperty =
    BindableProperty.Create(nameof(TextAndIconColor), typeof(Color), typeof(TMDropDown), Colors.Transparent, BindingMode.Default, null, propertyChanged: OnTextAndIconColorPropertyChanged);

    #endregion
    #region Constructor
    public TMDropDown()
    {
        SelectedItem = new object { };
        previousSelection = null;
        InitializeComponent();
        this.SetDynamicResource(StyleProperty, "DefaultStyle");
        PopupService.Instance.Dismissed += OnPopupRemoved;
    }
    #endregion

    #region Private Methods

    private static void OnTextAndIconColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMDropDown dropDown)
        {
            dropDown.label.TextColor = (Color)newValue;
        }
    }

    private static void OnBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMDropDown dropDown)
        {
            dropDown.innerBorder.Stroke = (Color)newValue;
        }
    }

    private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMDropDown dropDown)
        {
            dropDown.innerBorder.BackgroundColor = (Color)newValue;
        }
    }

    private void OnSelected(object sender, SelectedItemChangedEventArgs e)
    {
        previousSelection = SelectedItem;

        if (SelectedItem != null)
        {
            SelectedItem = e.SelectedItem;
            SelectedIndex = e.SelectedItemIndex;
            RaiseSelectionChangedEvent(previousSelection, e.SelectedItemIndex);
            UpdateCellColor((ListView)sender);
        }
        label.Text = e.SelectedItem.ToString();
        if (PopupService.Instance.PopupStack.Count > 0)
        {
            PopupService.Instance?.DismissAsync();
            Close();
        }
    }

    private void OnTapped(object sender, EventArgs e)
    {
        if (ItemsSource.Cast<object>().Count() > 0)
        {
            Open();
        }
    }

    private async void Close()
    {
        await Task.WhenAll(

            indicatorButton.RotateTo(0, AnimationDuration)
        );
    }


    DropDownContents dropDownContents;
    private async void Open()
    {
        var locationFetcher = new LocationFetcher();
        var loc = locationFetcher.GetCoordinates(this);
        var height = Application.Current.MainPage.Window.Height;
        dropDownContents = new DropDownContents(innerBorder, Enums.ModalPosition.Bottom)
        {
            ItemSource = this.ItemsSource,
            SelectedIndex = this.SelectedIndex,
            SelectedItem = this.SelectedItem,
            Margin = margin,
            DesiredHeight = desiredHeight,
            WidthRequest = innerBorder.Width,
            SelectedEventHandler = OnSelected,
            YPosition = loc.Y,
            Height = height
        };
        dropDownContents.Build();
        await Task.WhenAll(
            indicatorButton.RotateTo(-180, AnimationDuration)
        );

        await PopupService.Instance?.PresentAsync(dropDownContents, true);
    }

    private void OnPopupRemoved(object sender, EventArgs e)
    {
        Close();
    }
    private void RaiseSelectionChangedEvent(object previousSelection, int index)
    {
        DropDownSelectionChangedEventArgs args = new DropDownSelectionChangedEventArgs
        {
            PreviousSelection = previousSelection,
            CurrentSelection = SelectedItem,
            SelectedIndex = index
        };

        SelectionChanged?.Invoke(this, args);
        SelectionChangedCommand?.Execute(args);
    }
    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMDropDown tmDropDown)
        {
            tmDropDown.items = newValue as IEnumerable;
            tmDropDown.UpdateListViewItemsSource(tmDropDown.items);
            tmDropDown.UpdateListBorderHeight(tmDropDown.items);
        }
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
                margin = new Thickness(0, ((itemCount - 1) * 56) + 4, 10, 0);
#if WINDOWS
                margin = new Thickness(0, ((itemCount - 1) * 56) + 30, 10, 0);
#endif
            }
        }
    }

    private void UpdateCellColor(ListView list)
    {
        foreach (var item in list.TemplatedItems)
        {
            if (item is DropDownViewCell textCell)
            {
                if (SelectedItem == textCell.BindingContext)
                {
                    textCell.UpdateHighlightBackgroundColor();
                }
                else
                {
                    textCell.UpdateDefaultBackgroundColor();
                }
            }
        }
    }
    #endregion

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (ItemsSource != null && ItemsSource.Cast<object>().Any())
        {
            Open();
        }
    }
}
