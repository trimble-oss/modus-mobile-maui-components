namespace Trimble.Modus.Components;

[ContentProperty(nameof(Content))]
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

    public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(Page), typeof(TabViewItem));

    public Page? Content
    {
        get => (Page?)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

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

