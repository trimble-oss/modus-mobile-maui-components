using Trimble.Modus.Components.Constant;

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
            FontAttributes = FontAttributes.None,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Start,
            Padding = new Thickness(8, 12)
        };

        label.SetDynamicResource(Label.TextColorProperty, ColorsConstants.Secondary);
        label.SetDynamicResource(Label.BackgroundColorProperty, ColorsConstants.Transparent);

        label.SetBinding(Label.TextProperty, new Binding("Text", source: this));

        View = new StackLayout
        {
            Children = { label },
        };
        View.SetDynamicResource(StackLayout.BackgroundColorProperty, ColorsConstants.Transparent);
    }

    internal void UpdateBackgroundColor(string backgroundColorKey, bool textAttribute)
    {
        View.SetDynamicResource(View.BackgroundColorProperty, backgroundColorKey);
        label.FontAttributes = textAttribute ? FontAttributes.Bold : FontAttributes.None;
    }
}
