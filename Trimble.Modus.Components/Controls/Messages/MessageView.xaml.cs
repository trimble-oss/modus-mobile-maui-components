using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Mvvm.ComponentModel;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class MessageView : ContentView
{
    #region Bindable Properties
    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(MessageView),
        propertyChanged: OnCustomPropertyChanged);

    public static readonly BindableProperty ThemeProperty = BindableProperty.Create(
        nameof(Theme),
        typeof(MessageTheme),
        typeof(MessageView),
        defaultValue: MessageTheme.Primary,
        propertyChanged: OnCustomPropertyChanged);

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IconSource),
        typeof(ImageSource),
        typeof(MessageView),
        propertyChanged: OnCustomPropertyChanged);

    public static readonly BindableProperty MessageSizeProperty = BindableProperty.Create(
        nameof(MessageSize),
        typeof(MessageSize),
        typeof(MessageView),
        defaultValue: MessageSize.Default,
        propertyChanged: OnCustomPropertyChanged);

    public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(
        nameof(BackgroundColor),
        typeof(Color),
        typeof(MessageView),
        Colors.Transparent,
        propertyChanged: (bindable, _, newValue) => (bindable as MessageView).contentLayout.BackgroundColor = (Color)newValue);

    public static readonly BindableProperty TextAndIconColorProperty = BindableProperty.Create(
        nameof(TextAndIconColor),
        typeof(Color),
        typeof(MessageView),
        defaultValue: ResourcesDictionary.ColorsDictionary(ColorsConstants.BlueLight),
        propertyChanged: OnTextAndIconColorChanged);

    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
        nameof(BorderColor),
        typeof(Color),
        typeof(MessageView),
        propertyChanged: (bindable, _, newValue) => (bindable as MessageView).contentBorder.Stroke = (Color)newValue);
    #endregion


    #region Public Properties
    internal Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }
    internal Color TextAndIconColor
    {
        get => (Color)GetValue(TextAndIconColorProperty);
        set => SetValue(TextAndIconColorProperty, value);
    }

    internal new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public MessageTheme Theme
    {
        get => (MessageTheme)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public ImageSource IconSource
    {
        get => (ImageSource)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public MessageSize MessageSize
    {
        get => (MessageSize)GetValue(MessageSizeProperty);
        set => SetValue(MessageSizeProperty, value);
    }

    #endregion

    public MessageView()
    {
        InitializeComponent();
        SetDynamicResource(StyleProperty, "Primary");
    }

    private void Reload()
    {
        switch (Theme)
        {
            case MessageTheme.Primary:
                SetDynamicResource(StyleProperty, "Primary");
                break;
            default:
                SetDynamicResource(StyleProperty, "Secondary");
                break;
        }
        switch (MessageSize)
        {
            case MessageSize.Small:
                titleLabel.FontSize = 12;
                contentLayout.Padding = 8;
                leftIconImage.HeightRequest = 24;
                leftIconImage.WidthRequest = 24;
                break;
            case MessageSize.Large:
                titleLabel.FontSize = 16;
                contentLayout.Padding = 12;
                leftIconImage.HeightRequest = 24;
                leftIconImage.WidthRequest = 24;
                break;
            case MessageSize.XLarge:
                titleLabel.FontSize = 18;
                contentLayout.Padding = 12;
                leftIconImage.HeightRequest = 32;
                leftIconImage.WidthRequest = 32;
                break;
            default:
                titleLabel.FontSize = 14;
                contentLayout.Padding = 10;
                leftIconImage.HeightRequest = 24;
                leftIconImage.WidthRequest = 24;
                break;
        }
        updateIconTint();
    }

    private void updateIconTint()
    {
        leftIconImage.Source = IconSource;
        leftIconImage.IsVisible = IconSource != null;

        leftIconImage.Behaviors.Clear();
        var behavior = new IconTintColorBehavior
        {
            TintColor = TextAndIconColor
        };
        leftIconImage.Behaviors.Add(behavior);

    }

    private static void OnCustomPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MessageView messageView)
            messageView.Reload();
    }

    private static void OnTextAndIconColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var message = bindable as MessageView;
        message.titleLabel.TextColor = (Color)newValue;
        message.updateIconTint();
    }
}

