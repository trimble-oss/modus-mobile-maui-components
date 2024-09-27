namespace Trimble.Modus.Components;

public partial class ControlLabel : ContentView
{


    internal const double disabledOpacity = 0.4;
    /// <summary>
    /// Gets or sets the text for the title label in the control
    /// </summary>
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText), typeof(string), typeof(ControlLabel), null);

    /// <summary>
    /// Gets or sets the required boolean for the label in the control
    /// </summary>
    public static readonly BindableProperty IsRequiredProperty =
        BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(ControlLabel), false);

    /// <summary>
    /// Gets or sets the header text color
    /// </summary>
    public static readonly BindableProperty HeaderTextColorProperty =
        BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(ControlLabel), Colors.Transparent,
            propertyChanged: OnHeaderTextColorPropertyChanged);

    /// <summary>
    /// Gets or sets value that indicates whether the input control is enabled or not.
    /// </summary>
    public static new readonly BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(ControlLabel), true, propertyChanged: OnEnabledPropertyChanged);

    /// <summary>
    /// Gets or sets value that indicates whether the input control is readonly or not.
    /// </summary>
    public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(ControlLabel), false, propertyChanged: OnReadOnlyPropertyChanged);


    /// <summary>
    /// Gets or sets the title text
    /// </summary>
    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the required property
    /// </summary>
    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }

    /// <summary>
    /// Gets or sets the header text color
    /// </summary>
    internal Color HeaderTextColor
    {
        get => (Color)GetValue(HeaderTextColorProperty);
        set => SetValue(HeaderTextColorProperty, value);
    }


    /// <summary>
    /// Gets or sets the text color
    /// </summary>
    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the readonly property of the control
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public ControlLabel()
    {
        InitializeComponent();
    }

    private static void OnHeaderTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ControlLabel controlLabel)
        {
            controlLabel.inputLabel.TextColor = controlLabel.HeaderTextColor;
        }
    }

    private static void OnEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ControlLabel controlLabel)
        {
            controlLabel.UpdateControlLabelColors(controlLabel);
        }
    }

    /// <summary>
    /// Triggered when the IsReadOnly property is changed
    /// </summary>
    /// <param name="bindable">Object</param>
    /// <param name="oldValue">Old value</param>
    /// <param name="newValue">New value</param>
    private static void OnReadOnlyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ControlLabel controlLabel)
        {
            controlLabel.UpdateControlLabelColors(controlLabel);
        }
    }

    private void UpdateControlLabelColors(ControlLabel controlLabel)
    {
        if (controlLabel.IsReadOnly)
        {
            controlLabel.controlLabelView.Opacity = 1;
        }
        else
        {
            if (controlLabel.IsEnabled)
            {
                controlLabel.controlLabelView.Opacity = 1;
            }
            else
            {
                controlLabel.controlLabelView.Opacity = disabledOpacity;
            }
        }
    }
}
