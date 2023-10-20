namespace Trimble.Modus.Components;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

public partial class TMBadge : ContentView
{
    public static readonly BindableProperty BadgeTextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(TMBadge), "");

    public static readonly BindableProperty BadgeSizeProperty =
        BindableProperty.Create(nameof(BadgeSize), typeof(BadgeSize), typeof(TMBadge), BadgeSize.Medium, propertyChanged: OnSizeChanged);

    public static readonly BindableProperty BadgeColorProperty =
            BindableProperty.Create(nameof(Color), typeof(BadgeColor), typeof(TMBadge), BadgeColor.Primary, propertyChanged: OnBadgeColorChanged);

    public static readonly BindableProperty BadgeShapeProperty =
        BindableProperty.Create(nameof(Shape), typeof(BadgeShape), typeof(TMBadge), BadgeShape.Rectangle, propertyChanged: OnBadgeShapePropertyChanged);

    public static readonly BindableProperty BadgeContentProperty =
        BindableProperty.Create(nameof(BadgeContent),typeof(View),typeof(TMBadge),null,propertyChanged: OnContentPropertyChanged);

    public View BadgeContent
    {
        get { return (View)GetValue(BadgeContentProperty); }
        set { SetValue(BadgeContentProperty, value); }
    }

    public string Text
    {
        get => (string)GetValue(BadgeTextProperty);
        set => SetValue(BadgeTextProperty, value);
    }

    public BadgeSize BadgeSize
    {
        get => (BadgeSize)GetValue(BadgeSizeProperty);
        set => SetValue(BadgeSizeProperty, value);
    }

    public BadgeColor Color
    {
        get => (BadgeColor)GetValue(BadgeColorProperty);
        set => SetValue(BadgeColorProperty, value);
    }
    public BadgeShape Shape
    {
        get => (BadgeShape)GetValue(BadgeShapeProperty);
        set => SetValue(BadgeShapeProperty, value);
    }

    public TMBadge()
    {
        InitializeComponent();
        UpdateBadge(this);
    }

    private void UpdateBadge(TMBadge tMBadge)
    {
        UpdateColor(tMBadge);
        UpdateShape(tMBadge);
        UpdateLabelOnSize(tMBadge);
    }
    private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMBadge contentView && newValue != null)
        {
            contentView.content.Content = (View)newValue;
            contentView.content.IsVisible = true;
            contentView.frame.Margin = new Thickness(0, 0, -10, -15);
            contentView.frame.HorizontalOptions = LayoutOptions.End;
            contentView.frame.VerticalOptions = LayoutOptions.Start;
        }
    }

    private static void OnBadgeColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMBadge tmBadge)
        {
            UpdateColor(tmBadge);
        }
    }

    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMBadge tmBadge)
        {
            UpdateLabelOnSize(tmBadge);
        }
    }
    private static void OnBadgeShapePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMBadge tmBadge)
        {
            UpdateShape(tmBadge);
        }
    }

    private static void UpdateShape(TMBadge tmBadge)
    {
        switch (tmBadge.Shape)
        {
            case BadgeShape.Round:
                tmBadge.frame.StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20)
                };
                break;
            case BadgeShape.Rectangle:
            default:
                tmBadge.frame.StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(4)
                };
                break;
        }
    }

    private static void UpdateLabelOnSize(TMBadge badge)
    {
        switch (badge.BadgeSize)
        {
            case BadgeSize.Small:
                badge.label.FontSize = 12;
                badge.label.Padding = new Thickness(12, 0);
                badge.label.HeightRequest = 20;
                break;
            case BadgeSize.Large:
                badge.label.FontSize = 14;
                badge.label.Padding = new Thickness(12, 4);
                badge.label.HeightRequest = 32;
                break;
            case BadgeSize.Medium:
            default:
                badge.label.FontSize = 12;
                badge.label.Padding = new Thickness(12, 2);
                badge.label.HeightRequest = 24;
                break;
        }
    }

    private static void UpdateColor(TMBadge tmBadge)
    {
        switch(tmBadge.Color)
        {
            case BadgeColor.Secondary:
                tmBadge.frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray7);
                tmBadge.label.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
            case BadgeColor.Tertiary:
                tmBadge.frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray1);
                tmBadge.label.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                break;
            case BadgeColor.Success:
                tmBadge.frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Green);
                tmBadge.label.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
            case BadgeColor.Warning:
                tmBadge.frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Yellow);
                tmBadge.label.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9);
                break;
            case BadgeColor.Error:
                tmBadge.frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Red);
                tmBadge.label.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
            case BadgeColor.Primary:
            default:
                tmBadge.frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmBadge.label.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
        }
    }
}

