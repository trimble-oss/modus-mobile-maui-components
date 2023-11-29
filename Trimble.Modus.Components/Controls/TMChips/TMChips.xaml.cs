namespace Trimble.Modus.Components;

using CommunityToolkit.Maui.Behaviors;
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
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TMChips), propertyChanged: OnTitleChanged);

    public static readonly BindableProperty ChipSizeProperty =
        BindableProperty.Create(nameof(ChipSize), typeof(ChipSize), typeof(TMChips), ChipSize.Default, propertyChanged: OnSizeChanged);

    public static readonly BindableProperty ChipStateProperty =
        BindableProperty.Create(nameof(ChipState), typeof(ChipState), typeof(TMChips), ChipState.Default);

    public static readonly BindableProperty ChipStyleProperty =
        BindableProperty.Create(nameof(ChipStyle), typeof(ChipStyle), typeof(TMChips), ChipStyle.Fill);

    public static readonly BindableProperty ChipTypeProperty =
      BindableProperty.Create(nameof(ChipType), typeof(ChipType), typeof(TMChips), ChipType.Input, propertyChanged: OnChipsTypeSelected);

    public new static readonly BindableProperty IsEnabledProperty =
      BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMChips), true, propertyChanged: OnIsEnabledChanged);

    public static readonly BindableProperty CloseCommandProperty =
      BindableProperty.Create(nameof(CloseCommand), typeof(ICommand), typeof(TMChips));

    public static readonly BindableProperty ClickedCommandProperty =
      BindableProperty.Create(nameof(ClickedCommand), typeof(ICommand), typeof(TMChips));

    public new static readonly BindableProperty BackgroundColorProperty = BindableProperty
        .Create(nameof(BackgroundColor),
        typeof(Color),
        typeof(TMChips),
        Colors.White,
        BindingMode.Default,
        null,
        propertyChanged: OnBackgroundColorPropertyChanged);

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
        typeof(Color),
        typeof(TMChips),
        Colors.Black,
        BindingMode.Default,
        null,
        propertyChanged: OnTextColorPropertyChanged);

    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
        typeof(Color),
        typeof(TMChips),
        Colors.Transparent,
        BindingMode.Default,
        null,
        propertyChanged: OnBorderColorPropertyChanged);

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

    public ChipType ChipType
    {
        get => (ChipType)GetValue(ChipTypeProperty);
        set => SetValue(ChipTypeProperty, value);
    }
    public ChipSize ChipSize
    {
        get => (ChipSize)GetValue(ChipSizeProperty);
        set => SetValue(ChipSizeProperty, value);
    }

    public ChipState ChipState
    {
        get => (ChipState)GetValue(ChipStateProperty);
        set => SetValue(ChipStateProperty, value);
    }
    public ChipStyle ChipStyle
    {
        get => (ChipStyle)GetValue(ChipStyleProperty);
        set => SetValue(ChipStyleProperty, value);
    }

    internal Color BackgroundColor
    {
        get { return (Color)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }

    internal Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { this.SetValue(TextColorProperty, value); }
    }

    internal Color BorderColor
    {
        get { return (Color)GetValue(BorderColorProperty); }
        set { this.SetValue(BorderColorProperty, value); }
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
        if (propertyName == "ChipState" || propertyName == "ChipStyle")
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
        if (IsEnabled && ChipState != ChipState.Error)
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

    private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips tMChips)
        {
            tMChips.OnBackgroundColorChanged();
        }
    }

    private static void OnTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips tMChips)
        {
            tMChips.OnTextColorChanged();
        }
    }

    private static void OnBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips tMChips)
        {
            tMChips.OnBorderColorChanged();
        }
    }
    private void OnRightIconTapped(object sender, EventArgs e)
    {
        CloseCommand?.Execute(this);
    }


    private void OnTapped(object sender, EventArgs e)
    {
        if (sender is TMChips tMChips)
        {
            isSelected = !isSelected;
            if (isSelected)
            {
                if (ChipType == ChipType.Filter)
                {
                    tMChips.lefticon.IsVisible = true;
                    tMChips.lefticon.Source = ImageConstants.Check;
                }
                VisualStateManager.GoToState(this, "Pressed");
                _clicked?.Invoke(this, EventArgs.Empty);
                ClickedCommand?.Execute(this);
            }
            else
            {
                UpdateState();
                if (ChipType == ChipType.Filter)
                {
                    tMChips.lefticon.IsVisible = false;
                }
            }
        }

    }

    private void UpdateChips(TMChips tMChips)
    {
        UpdateCornerRadius(tMChips);
        tMChips.AssignStates(ChipState.Default);
        UpdateLabelOnSize(tMChips);
    }

    private static void UpdateCornerRadius(TMChips tMChips)
    {
        tMChips.frame.StrokeShape = new RoundRectangle
        {
            CornerRadius = new CornerRadius(tMChips.ChipSize == ChipSize.Small ? 16 : 24)
        };
    }

    private void OnBackgroundColorChanged()
    {
        frame.BackgroundColor = this.BackgroundColor;
    }

    private void OnTextColorChanged()
    {
        if (this.label != null)
        {
            this.label.TextColor = this.TextColor;
        }
    }

    private void OnBorderColorChanged()
    {
        frame.Stroke = this.BorderColor;
    }

    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips tMChips)
        {
            UpdateCornerRadius(tMChips);
            UpdateLabelOnSize(tMChips);
        }
    }
    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TMChips tMChips)
        {
            tMChips.label.Text = (string)newValue;
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
            if (newValue != null && tMChips.ChipType == ChipType.Input)
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
            switch (tMChips.ChipType)
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
        switch (ChipState)
        {
            case ChipState.Error:
                AssignStates(ChipState);
                GestureRecognizers.Clear();
                break;
            case ChipState.Default:
            default:
                AssignStates(ChipState);
                if (!GestureRecognizers.Contains(tapGestureRecognizer))
                {
                    GestureRecognizers.Add(tapGestureRecognizer);
                }
                break;
        }
        VisualStateManager.GoToState(this, "Normal");
    }
    private void AssignStates(ChipState state)
    {
        if (ChipStyle == ChipStyle.Fill)
            this.SetDynamicResource(StyleProperty, state.ToString() + "Fill");
        else
            this.SetDynamicResource(StyleProperty, state.ToString() + "Outline");
    }
    private static void UpdateLabelOnSize(TMChips tMChips)
    {
        switch (tMChips.ChipSize)
        {
            case ChipSize.Small:
                tMChips.label.FontSize = 14;
                tMChips.label.FontFamily = "OpenSansSemibold";
                tMChips.chipContent.Padding = new Thickness(12, 4);
                tMChips.chipContent.HeightRequest = 32;
                break;
            case ChipSize.Default:
            default:
                tMChips.label.FontSize = 16;
                tMChips.label.FontFamily = "OpenSansRegular";
                tMChips.chipContent.Padding = new Thickness(12);
                tMChips.chipContent.HeightRequest = 48;
                break;
        }
    }
    #endregion
}
