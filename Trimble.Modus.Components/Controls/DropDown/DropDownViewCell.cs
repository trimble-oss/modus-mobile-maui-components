using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public class DropDownViewCell : ViewCell
{
    internal Label label;
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(DropDownViewCell), "");

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public DropDownViewCell()
    {
        label = new Label
        {
            TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9),
            FontAttributes = FontAttributes.None,
            BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Start,
            Padding = new Thickness(8, 12)
        };

        label.SetBinding(Label.TextProperty, new Binding("Text", source: this));

        View = new StackLayout
        {
            Children = { label
},
            BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent),
        };
    }
    internal void UpdateBackgroundColor(Color backgroundColor, bool textAttribute)
    {
        View.BackgroundColor = backgroundColor;
        label.FontAttributes = textAttribute ? FontAttributes.Bold : FontAttributes.None;
    }
}
