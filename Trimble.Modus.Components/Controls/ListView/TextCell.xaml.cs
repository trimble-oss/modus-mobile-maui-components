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

    public static readonly BindableProperty BackgrondColorProperty =
       BindableProperty.Create(nameof(BackgrondColor), typeof(Color), typeof(TextCell), Colors.White,
           propertyChanged: OnBackgroundColorChanged);   

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
    public Color BackgrondColor
    {
        get => (Color)GetValue(BackgrondColorProperty);
        set => SetValue(BackgrondColorProperty, value);
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
    }
    #endregion
    #region Private Methods
    private static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable!=null && bindable is TextCell cell)
        {
            cell.grid.BackgroundColor = (Color)newValue;
        }
    }

    #endregion
}
