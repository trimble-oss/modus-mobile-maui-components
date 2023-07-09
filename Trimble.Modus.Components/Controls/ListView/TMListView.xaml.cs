using System.Collections;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMListView : ContentView
{
    public static readonly BindableProperty ItemsSourceProperty =
    BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TMListView), null);

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }


    public static readonly BindableProperty CustomTemplateProperty = BindableProperty.Create(
        nameof(ItemTemplate),
        typeof(DataTemplate),
        typeof(TMListView));
    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(CustomTemplateProperty);
        set => SetValue(CustomTemplateProperty, value);
    }

    public TMListView()
    {
        InitializeComponent();
    }

}


public class TextForm
{
    public string Title { get; set; }
    public string Description { get; set; }
}

