using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;
using CommunityToolkit.Maui.Behaviors;
using System.ComponentModel;

namespace Trimble.Modus.Components;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TMSegmentedItem
{
    #region Bindable propertyies
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(TMSegmentedItem), string.Empty);

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TMSegmentedItem), false, propertyChanged: OnSegmentedItemPropertyChanged);

    public static readonly BindableProperty ShowSeparatorProperty = BindableProperty.Create(nameof(ShowSeparator), typeof(bool), typeof(TMSegmentedItem), true);

    public static readonly BindableProperty ColorThemeProperty = BindableProperty.Create(nameof(ColorTheme), typeof(SegmentColorTheme), typeof(TMSegmentedItem), SegmentColorTheme.Primary, propertyChanged: OnColorThemeChanged);

    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(TMSegmentedItem), null, propertyChanged: OnSegmentedItemPropertyChanged);

    public static readonly BindableProperty CurrentBackgroundColorProperty = BindableProperty.Create(nameof(CurrentBackgroundColor), typeof(Color), typeof(TMSegmentedItem), ResourcesDictionary.GetColor(ColorsConstants.Transparent), propertyChanged: OnCurrentBackgroundColorChanged);

    public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(SegmentedControlSize), typeof(TMSegmentedItem), SegmentedControlSize.Small, propertyChanged: OnSizeChanged);

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TMSegmentedItem), ResourcesDictionary.GetColor(ColorsConstants.Secondary), propertyChanged: OnTextColorChanged);

    public static readonly BindableProperty IconTintColorProperty = BindableProperty.Create(nameof(IconTintColor), typeof(Color), typeof(TMSegmentedItem), Colors.Black, propertyChanged: OnIconTintColorChanged);
    #endregion

    #region public properties

    /// <summary>
    /// IconTintColor for different themes
    /// </summary>
    internal Color IconTintColor
    {
        get { return (Color)GetValue(IconTintColorProperty); }
        set { this.SetValue(IconTintColorProperty, value); }
    }

    /// <summary>
    /// Text color for different themes
    /// </summary>
    internal Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { this.SetValue(TextColorProperty, value); }
    }

    public int ItemIndex { get; internal set; }
    /// <summary>
    /// Size of segment
    /// </summary>
    [
        EditorBrowsable(EditorBrowsableState.Never),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
    ]
    public SegmentedControlSize Size
    {
        get => (SegmentedControlSize)GetValue(SizeProperty);
        internal set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// Icon to be displayed in the segment
    /// </summary>
    [
        EditorBrowsable(EditorBrowsableState.Never),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
    ]
    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Text to be displayed in the segment
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Indicates whether the segment is selected or not
    /// </summary>
    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    /// <summary>
    /// Background color of the segment
    /// </summary>
    [
        EditorBrowsable(EditorBrowsableState.Never),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
    ]
    internal Color CurrentBackgroundColor
    {
        get => (Color)GetValue(CurrentBackgroundColorProperty);
        set => SetValue(CurrentBackgroundColorProperty, value);
    }

    /// <summary>
    /// Toggledthe separator visibility
    /// </summary>
    [
        EditorBrowsable(EditorBrowsableState.Never),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
    ]
    public bool ShowSeparator
    {
        get => (bool)GetValue(ShowSeparatorProperty);
        set => SetValue(ShowSeparatorProperty, value);
    }

    /// <summary>
    /// Indicates the color theme of the segment
    /// </summary>
    public SegmentColorTheme ColorTheme
    {
        get => (SegmentColorTheme)GetValue(ColorThemeProperty);
        set => SetValue(ColorThemeProperty, value);
    }
    #endregion

    #region Property change handlers
    /// <summary>
    /// On current background color property changed
    /// </summary>
    private static void OnIconTintColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as TMSegmentedItem).UpdateIconBehavior();
    }
    /// <summary>
    /// On current background color property changed
    /// </summary>
    private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as TMSegmentedItem).TextLabel.TextColor = (Color)newValue;
    }

    /// <summary>
    /// On current background color property changed
    /// </summary>
    private static void OnCurrentBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as TMSegmentedItem).GridContainer.BackgroundColor = (Color)newValue;
    }

    /// <summary>
    /// Update font size and height of the icon based on the size
    /// </summary>
    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMSegmentedItem segmentedItem)
        {
            if (segmentedItem.Size == SegmentedControlSize.Small)
            {
                segmentedItem.TextLabel.FontSize = 12;
            }
            else if (segmentedItem.Size == SegmentedControlSize.Medium || segmentedItem.Size == SegmentedControlSize.Large)
            {
                segmentedItem.TextLabel.FontSize = 16;
            }
            else if (segmentedItem.Size == SegmentedControlSize.XLarge)
            {
                segmentedItem.TextLabel.FontSize = 20;
            }
        }
    }
    /// <summary>
    /// Update the background color of the segment based on the color theme
    /// </summary>
    private static void OnColorThemeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as TMSegmentedItem).UpdateBackgroundColor();
    }

    /// <summary>
    /// Triggered when the Segment item property is changed
    /// </summary>
    static void OnSegmentedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as TMSegmentedItem)?.UpdateCurrentItemStyle();
    }

    #endregion

    #region Constructor
    public TMSegmentedItem()
    {
        InitializeComponent();
    }
    #endregion

    /// <summary>
    /// Update the current background color and icon color based on the selection
    /// </summary>
    void UpdateCurrentItemStyle()
    {
        UpdateBackgroundColor();
        UpdateIconBehavior();
    }

    /// <summary>
    /// Update the background color of the segment based on the selection
    /// </summary>
    internal void UpdateBackgroundColor()
    {
        string baseStyle = ColorTheme == SegmentColorTheme.Primary ? "Primary" : "Secondary";
        string selectedStyle = IsSelected ? "Selected" : "Normal";
        this.SetDynamicResource(StyleProperty, baseStyle);
        VisualStateManager.GoToState(this, selectedStyle);
    }

    /// <summary>
    /// Update the icon color based on the selection
    /// </summary>
    void UpdateIconBehavior()
    {
        // FIXME: IconTintColorBehavior is not working on windows, so we are removing the behavior for now
        // https://github.com/CommunityToolkit/Maui/issues/1212 - Issue about the same
        // This should be updated in the next release of the toolkit and we can remove this code
        if (DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            SegmentIcon.Behaviors.Clear();
            var behavior = new IconTintColorBehavior
            {
                TintColor = IconTintColor
            };
            SegmentIcon.Behaviors.Add(behavior);
        }
    }
}
