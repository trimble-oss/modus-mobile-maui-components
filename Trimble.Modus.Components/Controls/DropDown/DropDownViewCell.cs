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

        label.SetBinding(Label.TextProperty, new Binding("Text", source: this));

        View = new StackLayout
        {
            Children = { label },
        };
        UpdateDefaultBackgroundColor();
    }

    public void UpdateHighlightBackgroundColor()
    {
        label.FontAttributes = FontAttributes.Bold;
        label.SetDynamicResource(VisualElement.BackgroundColorProperty,ColorsConstants.Transparent);
        View.SetDynamicResource(VisualElement.BackgroundColorProperty, "DropDownContentHighLightColor");
        label.SetDynamicResource(Label.TextColorProperty, "DropDownContentHighLightTextColor");
    }
    public void UpdateDefaultBackgroundColor()
    {
        label.FontAttributes = FontAttributes.None;
        label.SetDynamicResource(VisualElement.BackgroundColorProperty, "DropDownContentDefaultBackgroundColor");
        View.SetDynamicResource(VisualElement.BackgroundColorProperty, "DropDownContentDefaultBackgroundColor");
        label.SetDynamicResource(Label.TextColorProperty, "DropDownContentDefaultTextColor");
    }
}
