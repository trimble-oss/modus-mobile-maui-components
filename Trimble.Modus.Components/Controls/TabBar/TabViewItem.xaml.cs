namespace Trimble.Modus.Components;

[ContentProperty(nameof(ContentPage))]
public partial class TabViewItem : ContentView
{
    public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(TabViewItem), string.Empty);

    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty IconProperty =
          BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(TabViewItem), null,
              propertyChanged: OnTabViewItemPropertyChanged);

    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly BindableProperty ContentPageProperty =
            BindableProperty.Create(nameof(ContentPage), typeof(Page), typeof(TabViewItem));

    public Page? ContentPage
    {
        get => (Page?)GetValue(ContentPageProperty);
        set => SetValue(ContentPageProperty, value);
    }

    public View? ContentView
    {
        get => (View?)GetValue(ContentViewProperty);
        set => SetValue(ContentViewProperty, value);
    }

    public static readonly BindableProperty ContentViewProperty =
            BindableProperty.Create(nameof(ContentPage), typeof(View), typeof(TabViewItem));

    public TabViewItem()
    {
        InitializeComponent();
        BindingContext = this;
    }

    static void OnTabViewItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        //(bindable as TabViewItem)?.UpdateCurrent();
    }
}

