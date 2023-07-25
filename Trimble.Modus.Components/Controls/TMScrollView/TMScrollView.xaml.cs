namespace Trimble.Modus.Components;

public partial class TMScrollView : ContentView
{
    public static readonly BindableProperty ScrollViewContentProperty =
        BindableProperty.Create(nameof(ScrollViewContent), typeof(View), typeof(TMScrollView),propertyChanged: OnSizeChanged);

    public View ScrollViewContent
    {
        get => (View)GetValue(ScrollViewContentProperty);
        set => SetValue(ScrollViewContentProperty, value);
    }
    public TMScrollView()
    {
        InitializeComponent();
    }

    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
     
    }
}
