using System.Windows.Input;
using CommunityToolkit.Maui.Behaviors;

namespace Trimble.Modus.Components;

public partial class TMAlert : ContentView
{
    #region Private fields
    Layout _parent;
    #endregion

    #region Bindable Properties
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
        typeof(string),
        typeof(TMAlert));

    public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(nameof(ButtonText),
        typeof(string),
        typeof(TMAlert),
        defaultValue: null);

    public static readonly BindableProperty HideLeftIconProperty = BindableProperty.Create(nameof(HideLeftIcon),
        typeof(Boolean),
        typeof(TMAlert),
        defaultValue: false);

    public static readonly BindableProperty DismissableProperty = BindableProperty.Create(nameof(Dismissable),
        typeof(Boolean),
        typeof(TMAlert),
        defaultValue: true);

    public static readonly BindableProperty TypeProperty = BindableProperty.Create(nameof(TMAlert.Type),
        typeof(AlertType),
        typeof(TMAlert),
        defaultValue: AlertType.Primary,
        propertyChanged: OnAlertTypeChanged);

    public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor),
        typeof(Color),
        typeof(TMAlert),
        Colors.White,
        BindingMode.Default,
        null,
        (bindable, _, newValue) => (bindable as TMAlert).alertLayout.BackgroundColor = (Color)newValue);

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
        typeof(Color),
        typeof(TMAlert),
        Colors.Black,
        BindingMode.Default,
        null,
        (bindable, _, newValue) => (bindable as TMAlert).titleLabel.TextColor = (Color)newValue);

    public static readonly BindableProperty IconTintColorProperty = BindableProperty.Create(nameof(IconTintColor),
        typeof(Color),
        typeof(TMAlert),
        Colors.Black,
        BindingMode.Default,
        null,
        OnIconTintColorPropertyChanged);

    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
        typeof(Color),
        typeof(TMAlert),
        Colors.Black,
        BindingMode.Default,
        null,
        (bindable, _, newValue) =>
        {
            (bindable as TMAlert).VerticalSeparator.Color = (Color)newValue;
            (bindable as TMAlert).contentBorder.Stroke = (Color)newValue;
        });

    public static readonly BindableProperty ButtonColorProperty = BindableProperty.Create(nameof(ButtonColor),
        typeof(Color),
        typeof(TMAlert),
        Colors.Black,
        BindingMode.Default,
        null,
        (bindable, _, newValue) =>
        {
            var control = (bindable as TMAlert);
            control.actionButton.TextColor = (Color)newValue;

            if (DeviceInfo.Platform == DevicePlatform.WinUI) return;

            control.rightIconImage.Behaviors.Clear();
            if (newValue != null)
            {
                var behavior = new IconTintColorBehavior
                {
                    TintColor = newValue as Color
                };
                control.rightIconImage.Behaviors.Add(behavior);
            }
        });

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(nameof(IconSource),
        typeof(ImageSource),
        typeof(TMAlert),
        propertyChanged: (bindable, _, newValue) =>
        {
            (bindable as TMAlert).leftIconImage.Source = (ImageSource)newValue;
        });

    public static readonly BindableProperty ButtonClickedCommandProperty = BindableProperty.Create(nameof(ButtonClickedCommand),
        typeof(ICommand),
        typeof(TMAlert),
        null);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter),
        typeof(object),
        typeof(TMAlert));
    #endregion

    #region Public Properties

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public Boolean HideLeftIcon
    {
        get => (Boolean)GetValue(HideLeftIconProperty);
        set => SetValue(HideLeftIconProperty, value);
    }

    public Boolean Dismissable
    {
        get => (Boolean)GetValue(DismissableProperty);
        set => SetValue(DismissableProperty, value);
    }

    public AlertType Type
    {
        get => (AlertType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public ICommand ButtonClickedCommand
    {
        get { return (ICommand)GetValue(ButtonClickedCommandProperty); }
        set { SetValue(ButtonClickedCommandProperty, value); }
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    internal new Color BackgroundColor
    {
        get { return (Color)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }

    internal Color ButtonColor
    {
        get { return (Color)GetValue(ButtonColorProperty); }
        set { SetValue(ButtonColorProperty, value); }
    }

    internal Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { this.SetValue(TextColorProperty, value); }
    }

    internal Color IconTintColor
    {
        get { return (Color)GetValue(IconTintColorProperty); }
        set { this.SetValue(IconTintColorProperty, value); }
    }
    internal Color BorderColor
    {
        get { return (Color)GetValue(BorderColorProperty); }
        set { this.SetValue(BorderColorProperty, value); }
    }

    internal ImageSource IconSource
    {
        get { return (ImageSource)GetValue(IconSourceProperty); }
        set { this.SetValue(IconSourceProperty, value); }
    }

    #endregion

    #region Constructor
    public TMAlert(string text)
    {
        InitializeComponent();
        Text = text;
        UpdateAlertStyle();
    }
    #endregion

    #region Protected Methods
    /// <summary>
    /// Show animation when parent is set
    /// </summary>
    /// <param name="args"></param>
    protected override void OnParentChanging(ParentChangingEventArgs args)
    {
        base.OnParentChanging(args);
        if (_parent == null) return ;
        this.FadeTo(1, 500, Easing.SpringOut);
        this.TranslateTo(0, 0, 500, Easing.SpringOut);
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Update Icon tint color based on theme
    /// </summary>
    void UpdateIconTint()
    {
        // FIXME: IconTintColorBehavior doesn't work properly on Windows
        // Remove this once the issue is fixed.
        if (DeviceInfo.Platform == DevicePlatform.WinUI) return;

        leftIconImage.Behaviors.Clear();
        if (IconTintColor != null)
        {
            var behavior = new IconTintColorBehavior
            {
                TintColor = IconTintColor
            };
            leftIconImage.Behaviors.Add(behavior);
        }
    }

    /// <summary>
    /// Update Tint color based on theme
    /// </summary>
    static void OnIconTintColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMAlert tmAlert)
        {
            tmAlert.UpdateIconTint();
        }
    }

    /// <summary>
    /// Update alert type
    /// </summary>
    static void OnAlertTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMAlert alert)
        {
            alert.UpdateAlertStyle();
        }
    }

    /// <summary>
    /// Update dynamic resource when alert type is updated
    /// </summary>
    void UpdateAlertStyle()
    {
        switch (Type)
        {
            case AlertType.Success:
                SetDynamicResource(StyleProperty, "Success");
                break;
            case AlertType.Error:
                SetDynamicResource(StyleProperty, "Error");
                break;
            case AlertType.Warning:
                SetDynamicResource(StyleProperty, "Warning");
                break;
            case AlertType.Primary:
                SetDynamicResource(StyleProperty, "Primary");
                break;
            case AlertType.Secondary:
                SetDynamicResource(StyleProperty, "Secondary");
                break;
            case AlertType.Dark:
                SetDynamicResource(StyleProperty, "Dark");
                break;
            default:
                SetDynamicResource(StyleProperty, "Primary");
                break;
        }

        if (DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            UpdateIconTint();
        }
    }

    /// <summary>
    /// Event handler for close button click
    /// </summary>
    void CloseButtonClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        DismissAlert();
    }

    /// <summary>
    /// Execute command when button is clicked
    /// </summary>
    void ActionButtonClicked(System.Object sender, System.EventArgs e)
    {
        ButtonClickedCommand?.Execute(CommandParameter);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Dismiss alert with animation
    /// </summary>
    public void DismissAlert()
    {
        if (_parent != null && _parent.Children.Contains(this))
        {
            this.TranslateTo(0, 50, 500, Easing.SpringIn);
            this.FadeTo(0, 500, Easing.SpringIn).ContinueWith((t) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var tempParent = _parent;
                    _parent = null;

                    tempParent.Children.Remove(this);
                });
            });
        }
    }

    /// <summary>
    /// Insert Alert into parent layout with animation
    /// </summary>
    /// <param name="parent"></param>
    public void ShowAlert(Layout parent)
    {
        _parent = parent;
        this.TranslationY = 50;
        _parent.Children.Insert(0, this);
    }
    #endregion
}
