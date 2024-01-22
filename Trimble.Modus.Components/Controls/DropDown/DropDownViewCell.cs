using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui.Graphics;
namespace Trimble.Modus.Components;

public class DropDownViewCell : ViewCell
{
    internal Label label;
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(DropDownViewCell), "");

    public static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(DropDownViewCell), propertyChanged: OnBackgroundColorPropertyChanged);

    public static readonly BindableProperty TextAttributeProperty =
        BindableProperty.Create(nameof(TextAttribute), typeof(bool), typeof(DropDownViewCell), true, propertyChanged: OnTextAttributeChanged);

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }
    public Color BackgroundColor
    {
        get { return (Color)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }
    public bool TextAttribute
    {
        get { return (bool)GetValue(TextAttributeProperty); }
        set { SetValue(TextAttributeProperty, value); }
    }
    private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DropDownViewCell cell)
        {
            cell.View.BackgroundColor = (Color)newValue;
        }
    }
    private static void OnTextAttributeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DropDownViewCell cell)
        {
            cell.label.FontAttributes = (bool)newValue ? FontAttributes.Bold : FontAttributes.None;
        }
    }

    public DropDownViewCell()
    {
        label = new Label
        {
            FontAttributes = FontAttributes.None,
            BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Start,
            Padding = new Thickness(8, 12)
        };
        label.SetDynamicResource(Label.TextColorProperty, "DropDownListTextColor");
        label.SetBinding(Label.TextProperty, new Binding("Text", source: this));

        View = new StackLayout
        {
            Children = { label
},
            BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent),
        };
    }
    internal void UpdateBackgroundColor(bool textAttribute)
    {
        View.BackgroundColor = this.BackgroundColor;
        label.FontAttributes = textAttribute ? FontAttributes.Bold : FontAttributes.None;
    }
}
