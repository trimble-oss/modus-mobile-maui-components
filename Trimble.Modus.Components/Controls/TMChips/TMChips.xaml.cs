namespace Trimble.Modus.Components;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Runtime.CompilerServices;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

public partial class TMChips : ContentView
{
    private bool isSelected = false;
    private EventHandler _clicked;

    private TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(TMChips));

    public static readonly BindableProperty ChipsTextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(TMChips), "");

    public static readonly BindableProperty ChipsSizeProperty =
        BindableProperty.Create(nameof(ChipsSize), typeof(ChipsSize), typeof(TMChips), ChipsSize.Default, propertyChanged: OnSizeChanged);

    public static readonly BindableProperty ChipsStateProperty =
            BindableProperty.Create(nameof(State), typeof(ChipsState), typeof(TMChips), ChipsState.Default);

    public static readonly BindableProperty ChipsStyleProperty =
        BindableProperty.Create(nameof(Style), typeof(ChipsStyle), typeof(TMChips), ChipsStyle.Fill);

    public static readonly BindableProperty ChipsTypeProperty =
      BindableProperty.Create(nameof(ChipsType), typeof(ChipsType), typeof(TMChips), ChipsType.Input, propertyChanged: OnChipsTypeSelected);

    public new static readonly BindableProperty IsEnabledProperty =
      BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMChips), true, propertyChanged: OnIsEnabledChanged);

    public ImageSource LeftIconSource
    {
        get => (ImageSource)GetValue(LeftIconSourceProperty);
        set => SetValue(LeftIconSourceProperty, value);
    }

    public event EventHandler Clicked
    {
        add { _clicked += value; }
        remove { _clicked -= value; }
    }

    public string Text
    {
        get => (string)GetValue(ChipsTextProperty);
        set => SetValue(ChipsTextProperty, value);
    }

    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public ChipsType Type
    {
        get => (ChipsType)GetValue(ChipsTypeProperty);
        set => SetValue(ChipsTypeProperty, value);
    }
    public ChipsSize ChipsSize
    {
        get => (ChipsSize)GetValue(ChipsSizeProperty);
        set => SetValue(ChipsSizeProperty, value);
    }

    public ChipsState State
    {
        get => (ChipsState)GetValue(ChipsStateProperty);
        set => SetValue(ChipsStateProperty, value);
    }
    public ChipsStyle Style
    {
        get => (ChipsStyle)GetValue(ChipsStyleProperty);
        set => SetValue(ChipsStyleProperty, value);
    }

    public TMChips()
    {
        InitializeComponent();
        InitialiseChips();
    }
    private void InitialiseChips()
    {
        UpdateChips(this);
        UpdateTapGestureRecogniser();
        tapGestureRecognizer.Tapped += OnTapped;
        lefticon.Source = LeftIconSource != null ? LeftIconSource : ImageConstants.Check;
        lefticon.IsVisible = true;
        righticon.IsVisible = false;
    }

    private void UpdateTapGestureRecogniser()
    {
        if (IsEnabled && State != ChipsState.Error)
        {
            if (!GestureRecognizers.Contains(tapGestureRecognizer))
            {
                GestureRecognizers.Add(tapGestureRecognizer);
            }
        }
        else
        {
            GestureRecognizers.Clear();
        }

    }

    private void OnTapped(object sender, EventArgs e)
    {
        if (sender is TMChips tMChips)
        {
            isSelected = !isSelected;
            if (isSelected)
            {
                label.TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
                if (Style == ChipsStyle.Fill)
                {
                    frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                    frame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                }
                else
                {
                    frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                    frame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlueClicked);
                }
                _clicked?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                UpdateState();
            }
            Console.WriteLine("Clicked " + isSelected);
        }

    }

    private void UpdateChips(TMChips tMChips)
    {
        tMChips.frame.StrokeShape = new RoundRectangle
        {
            CornerRadius = new CornerRadius(20)
        };
        tMChips.AssignStates(ColorsConstants.TertiaryButton, ColorsConstants.TrimbleGray);
        UpdateLabelOnSize(tMChips);
    }

    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips TMChips)
        {
            UpdateLabelOnSize(TMChips);
        }
    }

    private static void OnIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips tmChips)
        {
            if (!(bool)newValue)
            {
                tmChips.frame.Opacity = 0.5;
            }
            else
            {
                tmChips.frame.Opacity = 1;
            }
            tmChips.UpdateTapGestureRecogniser();
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == "State" || propertyName == "Style")
        {
            UpdateState();
        }
    }
    private static void OnChipsTypeSelected(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips tMChips)
        {
            switch (tMChips.Type)
            {
                case ChipsType.Filter:
                    tMChips.righticon.Source = ImageConstants.CancelCircle;
                    tMChips.lefticon.IsVisible = false;
                    tMChips.righticon.IsVisible = true;
                    break;
                case ChipsType.Input:
                default:
                    tMChips.lefticon.Source = tMChips.LeftIconSource != null ? tMChips.LeftIconSource : ImageConstants.Check;
                    tMChips.lefticon.IsVisible = true;
                    tMChips.righticon.IsVisible = false;
                    break;
            }
        }
    }
    private void UpdateState()
    {
        switch (State)
        {
            case ChipsState.Error:
                AssignStates(ColorsConstants.DangerToastColor, ColorsConstants.DangerRedClicked);
                GestureRecognizers.Clear();
                break;
            case ChipsState.Default:
            default:
                AssignStates(ColorsConstants.TertiaryButton, ColorsConstants.TrimbleGray);
                if (!GestureRecognizers.Contains(tapGestureRecognizer))
                {
                    GestureRecognizers.Add(tapGestureRecognizer);
                }
                break;
        }

    }
    private void AssignStates(string backgroundColor, string textColor)
    {
        if (Style == ChipsStyle.Fill)
        {
            frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(backgroundColor);
            frame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
        }
        else
        {
            frame.Stroke = ResourcesDictionary.ColorsDictionary(backgroundColor);
            frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
        }
        label.TextColor = ResourcesDictionary.ColorsDictionary(textColor);
    }
    private static void UpdateLabelOnSize(TMChips tMChips)
    {
        switch (tMChips.ChipsSize)
        {
            case ChipsSize.Small:
                tMChips.label.FontSize = 14;
                tMChips.frame.Padding = new Thickness(12, 4);
                tMChips.label.Margin = new Thickness(4, 0, 4, 0);
                break;
            case ChipsSize.Default:
            default:
                tMChips.label.FontSize = 16;
                tMChips.frame.Padding = new Thickness(12);
                tMChips.label.Margin = new Thickness(4, 0, 4, 0);
                break;
        }
    }
}

public enum ChipsSize
{
    Small,
    Default
}

public enum ChipsState
{
    Default,
    Error
}
public enum ChipsStyle
{
    Fill,
    Outline
}
public enum ChipsType
{
    Input,
    Filter
}
