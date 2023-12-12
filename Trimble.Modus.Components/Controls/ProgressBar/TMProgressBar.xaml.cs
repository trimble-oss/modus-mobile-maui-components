using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public partial class TMProgressBar : ContentView
{
    private readonly double _defaultFontSize = 12;
    private readonly double _smallFontSize = 10;

    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
      nameof(Progress), typeof(float), typeof(TMProgressBar), 0.0f);

    public static readonly BindableProperty SizeProperty = BindableProperty.Create(
   nameof(Size), typeof(ProgressBarSize), typeof(TMProgressBar), ProgressBarSize.Default, propertyChanged: OnSizeChangedProperty);

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
      nameof(Text), typeof(string), typeof(TMProgressBar));
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public float Progress
    {
        get => (float)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public ProgressBarSize Size
    {
        get => (ProgressBarSize)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
    public TMProgressBar()
    {
        InitializeComponent();
        this.progressBarIndicatorText.FontSize = _defaultFontSize;
        this.baseProgressBar.SetDynamicResource(BaseProgressBar.ProgressColorProperty, "ProgressBarPrimaryBackgroundColor");
        this.baseProgressBar.SetDynamicResource(BaseProgressBar.BaseColorProperty, "ProgressBarPrimaryBaseColor");
    }

    private static void OnSizeChangedProperty(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMProgressBar tmProgressBar)
        {
            tmProgressBar.progressBarIndicatorText.FontSize = tmProgressBar.Size == ProgressBarSize.Default ? tmProgressBar._defaultFontSize : tmProgressBar._smallFontSize;
        }
    }
}
