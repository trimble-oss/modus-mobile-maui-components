namespace Trimble.Modus.Components;

public partial class TextCell : ViewCell
{
    #region Bindable Properties
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TextCell), default(string));

    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(TextCell));

    public static readonly BindableProperty RightIconSourceProperty =
       BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(TextCell));

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(TextCell), default(string));

    public static readonly BindableProperty BackgroundColorProperty =
       BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(TextCell), Colors.Transparent, propertyChanged: OnBackgroundColorChanged);

    #endregion
    #region Public Fields
    public ImageSource LeftIconSource
    {
        get => (ImageSource)GetValue(LeftIconSourceProperty);
        set => SetValue(LeftIconSourceProperty, value);
    }

    public ImageSource RightIconSource
    {
        get => (ImageSource)GetValue(RightIconSourceProperty);
        set => SetValue(RightIconSourceProperty, value);
    }
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    #endregion
    #region Constructor
    public TextCell()
    {
        InitializeComponent();
        if (DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            UpdateBackgroundColor();
        }
    }
    #endregion
    #region Private Methods

    private static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TextCell cell)
        {
            cell.UpdateBackgroundColor();
        }
    }


    private void UpdateBackgroundColor()
    {
        grid.BackgroundColor = BackgroundColor;
    }
    #endregion
}
