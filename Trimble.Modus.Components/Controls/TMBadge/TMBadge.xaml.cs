namespace Trimble.Modus.Components;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Runtime.CompilerServices;
using Trimble.Modus.Components.Constant;
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
        BindingContext = this;
        UpdateBadge(this);
    }

    private void UpdateBadge(TMBadge tMBadge)
    {
        UpdateColor(tMBadge);
        UpdateShape(tMBadge);
        UpdateLabelOnSize(tMBadge);
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
                tmBadge.buttonFrame.StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20)
                };
                break;
            case BadgeShape.Rectangle:
            default:
                tmBadge.buttonFrame.StrokeShape = new RoundRectangle
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
                badge.buttonLabel.FontSize = 12;
                badge.buttonFrame.Padding = new Thickness(8,0,8,0);
                badge.buttonLabel.Margin = new Thickness(0,2,0,2);
                break;
            case BadgeSize.Large:
                badge.buttonLabel.FontSize = 14;
                badge.buttonFrame.Padding = new Thickness(12, 4);
                break;
            case BadgeSize.Medium:
            default:
                badge.buttonLabel.FontSize = 12;
                badge.buttonFrame.Padding = new Thickness(12, 2);
                break;
        }
    }

    private static void UpdateColor(TMBadge tmBadge)
    {
        switch(tmBadge.Color)
        {
            case BadgeColor.Secondary:
                tmBadge.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleNeutralGray);
                tmBadge.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
            case BadgeColor.Tertiary:
                tmBadge.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TertiaryButton);
                tmBadge.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Black);
                break;
            case BadgeColor.Success:
                tmBadge.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Green);
                tmBadge.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
            case BadgeColor.Warning:
                tmBadge.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleYellow);
                tmBadge.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Black);
                break;
            case BadgeColor.Error:
                tmBadge.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.DangerRed);
                tmBadge.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
            case BadgeColor.Primary:
            default:
                tmBadge.buttonFrame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                tmBadge.buttonLabel.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                break;
        }
    }
}

public enum BadgeSize
{
    Small,
    Medium,
    Large
}

public enum BadgeColor
{
    Primary,
    Secondary,
    Tertiary,
    Success,
    Warning,
    Error
}
public enum BadgeShape
{
    Rectangle,
    Round
}

