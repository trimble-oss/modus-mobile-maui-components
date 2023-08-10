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
    public static readonly BindableProperty ChipsTextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(TMChips), "");

    public static readonly BindableProperty ChipsSizeProperty =
        BindableProperty.Create(nameof(ChipsSize), typeof(ChipsSize), typeof(TMChips), ChipsSize.Medium, propertyChanged: OnSizeChanged);

    public static readonly BindableProperty ChipsStateProperty =
            BindableProperty.Create(nameof(State), typeof(ChipsState), typeof(TMChips), ChipsState.Default);

    public static readonly BindableProperty ChipsStyleProperty =
        BindableProperty.Create(nameof(Style), typeof(ChipsStyle), typeof(TMChips), ChipsStyle.Fill);

    public static readonly BindableProperty ChipsTypeProperty =
      BindableProperty.Create(nameof(ChipsType), typeof(ChipsType), typeof(TMChips), ChipsType.Input);

    public new static readonly BindableProperty IsEnabledProperty =
      BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMChips), true, propertyChanged: OnIsEnabledChanged);

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
        UpdateChips(this);
        UpdateTapGestureRecogniser();
        tapGestureRecognizer.Tapped += OnTapped;
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
                _clicked.Invoke(this, e);
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
                tMChips.label.FontSize = 12;
                tMChips.frame.Padding = new Thickness(8, 0, 8, 0);
                tMChips.label.Margin = new Thickness(0, 2, 0, 2);
                break;
            case ChipsSize.Large:
                tMChips.label.FontSize = 14;
                tMChips.frame.Padding = new Thickness(12, 4);
                break;
            case ChipsSize.Medium:
            default:
                tMChips.label.FontSize = 12;
                tMChips.frame.Padding = new Thickness(12, 2);
                break;
        }
    }
}

public enum ChipsSize
{
    Small,
    Medium,
    Large
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
