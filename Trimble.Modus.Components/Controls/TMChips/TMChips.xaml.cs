namespace Trimble.Modus.Components;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

public partial class TMChips : ContentView
{
    #region Private Fields
    private bool isSelected = false;
    private EventHandler _clicked;

    private TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
    #endregion
    #region Bindable Properties
    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(TMChips),propertyChanged:OnLeftIconPropertyChanged);

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TMChips), propertyChanged: OnTextChanged);

    public static readonly BindableProperty ChipsSizeProperty =
        BindableProperty.Create(nameof(ChipsSize), typeof(ChipSize), typeof(TMChips), ChipSize.Default, propertyChanged: OnSizeChanged);

    public static readonly BindableProperty ChipsStateProperty =
        BindableProperty.Create(nameof(State), typeof(ChipState), typeof(TMChips), ChipState.Default);

    public static readonly BindableProperty ChipsStyleProperty =
        BindableProperty.Create(nameof(Style), typeof(ChipStyle), typeof(TMChips), ChipStyle.Fill);

    public static readonly BindableProperty ChipsTypeProperty =
      BindableProperty.Create(nameof(Type), typeof(ChipType), typeof(TMChips), ChipType.Input, propertyChanged: OnChipsTypeSelected);

    public new static readonly BindableProperty IsEnabledProperty =
      BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMChips), true, propertyChanged: OnIsEnabledChanged);

    public static readonly BindableProperty CloseCommandProperty =
      BindableProperty.Create(nameof(CloseCommand), typeof(ICommand), typeof(TMChips));

    public static readonly BindableProperty ClickedCommandProperty =
      BindableProperty.Create(nameof(ClickedCommand), typeof(ICommand), typeof(TMChips));
    #endregion
    #region Public Properties
    public ICommand CloseCommand
    {
        get => (ICommand)GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    public ICommand ClickedCommand
    {
        get => (ICommand)GetValue(ClickedCommandProperty);
        set => SetValue(ClickedCommandProperty, value);
    }

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

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public ChipType Type
    {
        get => (ChipType)GetValue(ChipsTypeProperty);
        set => SetValue(ChipsTypeProperty, value);
    }
    public ChipSize ChipsSize
    {
        get => (ChipSize)GetValue(ChipsSizeProperty);
        set => SetValue(ChipsSizeProperty, value);
    }

    public ChipState State
    {
        get => (ChipState)GetValue(ChipsStateProperty);
        set => SetValue(ChipsStateProperty, value);
    }
    public ChipStyle Style
    {
        get => (ChipStyle)GetValue(ChipsStyleProperty);
        set => SetValue(ChipsStyleProperty, value);
    }
    #endregion
    #region Constructor

    public TMChips()
    {
        InitializeComponent();
        InitialiseChips();
    }
    #endregion
    #region Protected Methods
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == "State" || propertyName == "Style")
        {
            UpdateState();
        }
    }
#endregion
    #region Private Methods
    private void InitialiseChips()
    {
        UpdateChips(this);
        UpdateTapGestureRecogniser();
        tapGestureRecognizer.Tapped += OnTapped;
    }

    private void UpdateTapGestureRecogniser()
    {
        if (IsEnabled && State != ChipState.Error)
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
    private void OnRightIconTapped(object sender, EventArgs e)
    {
        if (sender is ImageButton tMChips)
        {
           CloseCommand?.Execute(this);
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
                if (Style == ChipStyle.Fill)
                {
                    frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                    frame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent);
                }
                else
                {
                    frame.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                    frame.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlueClicked);
                }
                if(Type == ChipType.Filter)
                {
                    tMChips.lefticon.IsVisible = true;
                    tMChips.lefticon.Source = ImageConstants.Check;
                }
                _clicked?.Invoke(this, EventArgs.Empty);
                ClickedCommand?.Execute(this);
            }
            else
            {
                UpdateState();
                if (Type == ChipType.Filter)
                {
                    tMChips.lefticon.IsVisible = false;
                }
            }
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
    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips TMChips)
        {
            TMChips.label.Text = (string)newValue;
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
    private static void OnLeftIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips tMChips)
        {
            if (newValue != null && tMChips.Type == ChipType.Input)
            {
                tMChips.lefticon.Source = (ImageSource)newValue;
                tMChips.lefticon.IsVisible = true;

            }
            else
            {
                tMChips.lefticon.IsVisible = false;
            }
        }
    }
    private static void OnChipsTypeSelected(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips tMChips)
        {
            switch (tMChips.Type)
            {
                case ChipType.Filter:
                    tMChips.lefticon.IsVisible = false;
                    tMChips.righticon.IsVisible = false;
                    break;
                case ChipType.Input:
                default:
                    tMChips.lefticon.IsVisible = tMChips.LeftIconSource != null ? true : false;
                    tMChips.righticon.IsVisible = true;
                    break;
            }
        }
    }
    private void UpdateState()
    {
        switch (State)
        {
            case ChipState.Error:
                AssignStates(ColorsConstants.DangerToastColor, ColorsConstants.DangerRedClicked);
                GestureRecognizers.Clear();
                break;
            case ChipState.Default:
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
        if (Style == ChipStyle.Fill)
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
            case ChipSize.Small:
                tMChips.label.FontSize = 14;
                tMChips.frame.Padding = new Thickness(12, 4);
                tMChips.label.Margin = new Thickness(4, 0, 4, 0);
                break;
            case ChipSize.Default:
            default:
                tMChips.label.FontSize = 16;
                tMChips.frame.Padding = new Thickness(12);
                tMChips.label.Margin = new Thickness(4, 0, 4, 0);
                break;
        }
    }
    #endregion
}
