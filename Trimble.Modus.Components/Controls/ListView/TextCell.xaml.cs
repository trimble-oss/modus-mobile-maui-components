using System.Runtime.CompilerServices;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TextCell : ViewCell
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TextCell), default(string),propertyChanged:TitleChanged);

    private static void TitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Console.WriteLine("Title"+newValue);
    }

    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(TextCell));

    public static readonly BindableProperty RightIconSourceProperty =
       BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(TextCell));

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

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(TextCell), default(string));

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    public Color setterColor;
    public TextCell()
    {
        InitializeComponent();
        BindingContext = this;
    }
}
