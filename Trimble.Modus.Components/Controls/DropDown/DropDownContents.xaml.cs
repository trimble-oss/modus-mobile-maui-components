using System.Collections;
using Trimble.Modus.Components.Popup.Animations;

namespace Trimble.Modus.Components;

public partial class DropDownContents : PopupPage
{
    public ListView dropDownListView;
    public static readonly BindableProperty BorderColorProperty =
    BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(DropDownContents), Colors.Transparent, BindingMode.Default, null, propertyChanged: OnBorderColorPropertyChanged);

    public static readonly BindableProperty SelectedItemProperty =
      BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(DropDownContents), propertyChanged: OnSelectedItemChanged);

    internal Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    internal DropDownContents()
    {

    }

    internal DropDownContents(View anchorView, Enums.ModalPosition position) : base(anchorView, position)
    {
        InitializeComponent();
        dropDownListView = listView;
        this.SetDynamicResource(StyleProperty, "DefaulDropDownContentstStyle");
    }

    public Thickness Margin { get; set; }
    public IEnumerable ItemSource { get; set; }
    public double DesiredHeight { get; set; }
    public EventHandler<SelectedItemChangedEventArgs> SelectedEventHandler { get; set; }
    public int SelectedIndex { get; set; }
    public double YPosition { get; set; }
    public new double Height { get; set; }

    public void Build()
    {
        Animation = new RevealAnimation(DesiredHeight);
        border.Margin = Margin;
        border.HeightRequest = DesiredHeight;
        border.WidthRequest = WidthRequest;
        listView.ItemsSource = ItemSource;
        if (SelectedIndex < 0)
        {
            listView.SelectedItem = null;
        }
        else
        {
            listView.SelectedItem = ItemSource?.Cast<object>()?.ToList()[SelectedIndex];
        }
        UpdateBackgroundColorOfCell(listView);
        listView.ItemSelected += SelectedEventHandler;
        if (Height - YPosition < DesiredHeight)
        {
            this.Position = Enums.ModalPosition.Top;
            border.Margin = new Thickness(0, -16, 0, 0);
#if WINDOWS
            border.Margin = new Thickness(0, 24, 10, 0);
#endif
        }
    }

    private static void UpdateBackgroundColorOfCell(ListView listView)
    {
        if (listView.SelectedItem != null)
        {
            foreach (var item in listView.TemplatedItems)
            {
                if (item is DropDownViewCell textCell)
                {
                    if (listView.SelectedItem == textCell.BindingContext)
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
    }

    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DropDownContents dropDown)
        {
            dropDown.listView.SelectedItem = newValue;
        }
    }

    private static void OnBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DropDownContents dropDown)
        {
            dropDown.border.Stroke = (Color)newValue;
        }
    }
}
