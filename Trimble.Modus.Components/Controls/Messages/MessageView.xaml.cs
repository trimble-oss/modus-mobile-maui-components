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

    #endregion


    #region Public Properties
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
        //Reload();
    }

    private void Reload()
    {
        switch (Theme)
        {
            case MessageTheme.Primary:
                contentLayout.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
                var textColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.BlueLight);
                titleLabel.TextColor = textColor;
                contentBorder.Stroke = textColor;
                break;
            default:
                contentLayout.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                titleLabel.TextColor = Colors.Black;
                contentBorder.Stroke = Colors.Black;
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
            TintColor = titleLabel.TextColor
        };
        leftIconImage.Behaviors.Add(behavior);

    }

    private static void OnCustomPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MessageView messageView)
            messageView.Reload();
    }

}

