using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class ListViewTemplateCell : ViewCell
{
    public static readonly BindableProperty ContentProperty
    = BindableProperty.Create(nameof(Content), typeof(View), typeof(ListViewTemplateCell), propertyChanged: OnContentPropertyChanged);

    private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is View view)
        {
            var test = view.BindingContext;
        }
    }

    public View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public ListViewTemplateCell()
    {
        InitializeComponent();
    }
}

