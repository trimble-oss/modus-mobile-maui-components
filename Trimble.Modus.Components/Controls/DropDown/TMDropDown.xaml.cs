using System.Collections;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
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
        get => (object)GetValue(SelectedItemsProperty);
        private set => SetValue(SelectedItemsProperty, value);
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

    public static readonly BindableProperty SelectedItemsProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(TMDropDown));

    public static readonly BindableProperty SelectionChangedCommandProperty =
         BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(TMDropDown));

    #endregion
    #region Constructor
    public TMDropDown()
    {
        SelectedItem = new object { };
        previousSelection = null;
        InitializeComponent();
        PopupService.Instance.Dismissed += OnPopupRemoved;
    }
    #endregion
    #region Private Methods
    private void OnSelected(object sender, SelectedItemChangedEventArgs e)
    {
        previousSelection = SelectedItem;

        if (SelectedItem != null)
        {
            SelectedItem = e.SelectedItem;
            RaiseSelectionChangedEvent(previousSelection, e.SelectedItemIndex);
            UpdateCellColor((ListView)sender);
        }
        label.Text = e.SelectedItem.ToString();
        if (PopupService.Instance.PopupStack.Count > 0)
        {
            PopupService.Instance?.DismissAsync();
            Close();
        }
        SelectedIndex = e.SelectedItemIndex;
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


    DropDownContents popup;
    private async void Open()
    {
        var locationFetcher = new LocationFetcher();
        var loc = locationFetcher.GetCoordinates(this);
        var height = Application.Current.MainPage.Window.Height;
        popup = new DropDownContents(innerBorder, Enums.ModalPosition.Bottom)
        {
            ItemSource = this.ItemsSource,
            SelectedIndex = this.SelectedIndex,
            Margin = margin,
            DesiredHeight = desiredHeight,
            WidthRequest = innerBorder.Width,
            SelectedEventHandler = OnSelected,
            YPosition = loc.Y,
            Height = height
        };
        popup.Build();
        await Task.WhenAll(
            indicatorButton.RotateTo(-180, AnimationDuration)
        );

        await PopupService.Instance?.PresentAsync(popup, true);
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
                    textCell.UpdateBackgroundColor(ColorsConstants.PrimaryLight, true);
                }
                else
                {
                    textCell.UpdateBackgroundColor(ColorsConstants.AlternateTextColor, false);
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
