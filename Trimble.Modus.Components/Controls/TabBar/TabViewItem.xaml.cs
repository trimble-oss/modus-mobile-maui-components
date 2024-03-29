﻿using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;
using CommunityToolkit.Maui.Behaviors;

namespace Trimble.Modus.Components;

[ContentProperty(nameof(ContentView))]
public partial class TabViewItem : ContentView
{
    #region Private fields
    private bool isOnScreen;
    #endregion
    #region Bindable Properties
    public static readonly BindableProperty TabColorProperty =
        BindableProperty.Create(nameof(TabColor), typeof(TabColor), typeof(TabViewItem), TabColor.Primary, propertyChanged: OnTabColorChanged);

    public static readonly BindableProperty TapCommandProperty =
        BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(TabViewItem), null);

    public static readonly BindableProperty ContentViewProperty =
        BindableProperty.Create(nameof(ContentPage), typeof(View), typeof(TabViewItem));

    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TabViewItem), false, propertyChanged: OnIsSelectedChanged);
    // See if we can support Content Page in the future
    /*
    public static readonly BindableProperty ContentPageProperty =
        BindableProperty.Create(nameof(ContentPage), typeof(Page), typeof(TabViewItem));
    */
    public static readonly BindableProperty IconProperty =
          BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(TabViewItem), null, propertyChanged: OnTabViewItemPropertyChanged);

    public static readonly BindableProperty TextProperty =
          BindableProperty.Create(nameof(Text), typeof(string), typeof(TabViewItem), string.Empty);

    public static readonly BindableProperty OrientationProperty =
         BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(TabViewItem), StackOrientation.Vertical, propertyChanged: OnOrientationPropertyChanged);

    internal static readonly BindablePropertyKey CurrentContentPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentContent), typeof(View), typeof(TabViewItem), null);

    public static readonly BindableProperty CurrentContentProperty = CurrentContentPropertyKey.BindableProperty;

    
    internal static readonly BindablePropertyKey CurrentTextColorPropertyKey =
        BindableProperty.CreateReadOnly(nameof(CurrentTextColor), typeof(Color), typeof(TabViewItem), ResourcesDictionary.ColorsDictionary(ColorsConstants.Gray9));

    public static readonly BindableProperty CurrentTextColorProperty = CurrentTextColorPropertyKey.BindableProperty;

    #endregion
    #region Public Delegate
    public delegate void TabTappedEventHandler(object? sender, TabTappedEventArgs e);
    #endregion
    #region Event
    public event TabTappedEventHandler? TabTapped;
    #endregion
    #region Internal Fields
    internal TabColor TabColor
    {
        get => (TabColor)GetValue(TabColorProperty);
        set => SetValue(TabColorProperty, value);
    }
    internal StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
    #endregion
    #region Public Fields
    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public View? CurrentContent
    {
        get => (View?)GetValue(CurrentContentProperty);
        private set => SetValue(CurrentContentPropertyKey, value);
    }

    internal Color CurrentTextColor
    {
        get => (Color)GetValue(CurrentTextColorProperty);
        private set => SetValue(CurrentTextColorPropertyKey, value);
    }
    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    // See if we can support Content Page in the future
    /*
    public Page? ContentPage
    {
        get => (Page?)GetValue(ContentPageProperty);
        set => SetValue(ContentPageProperty, value);
    }*/

    public View? ContentView
    {
        get => (View?)GetValue(ContentViewProperty);
        set => SetValue(ContentViewProperty, value);
    }

    internal bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
    #endregion
    #region Constructor
    public TabViewItem()
    {
        InitializeComponent();
        BindingContext = this;

    }
    #endregion
    #region Private Methods
    private void UpdateCurrent()
    {
        icon.Behaviors.Clear();
        var iconColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
        if (TabColor == TabColor.Primary)
        {
            iconColor = IsSelected ? ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue) : ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
            CurrentTextColor = IsSelected ? ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue) : ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
        }
        else
        {
            iconColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
            CurrentTextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);

        }
        var behavior = new IconTintColorBehavior
        {
            TintColor = iconColor
        };
        tabItem.Orientation = Orientation;
        icon.Behaviors.Add(behavior);
        text.TextColor = CurrentTextColor;
        selectedBorder.BackgroundColor = !IsSelected ? ResourcesDictionary.ColorsDictionary(ColorsConstants.Transparent) : ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
        UpdateCurrentContent();
    }
    private static void OnTabViewItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as TabViewItem)?.UpdateCurrent();
    }
    private static void OnTabColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TabViewItem item)
        {
            if (newValue is TabColor)
            {
                item.UpdateCurrent();
            }

        }
    }
    private static void OnOrientationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TabViewItem item)
        {
            if (newValue is StackOrientation)
            {
                item.tabItem.Orientation = (StackOrientation)newValue;
                if ((StackOrientation)newValue == StackOrientation.Horizontal)
                {
                    item.tabItem.Margin = new Thickness(12, 4, 12, 4);
                    item.tabItem.Spacing = 4;
                }
            }

        }
    }

    private static void OnIsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TabViewItem tabViewItem)
        {
            tabViewItem.UpdateCurrent();
        }
    }

    #endregion
    #region Internal Methods
    internal void UpdateCurrentContent(bool isOnScreen = true)
    {
        this.isOnScreen = isOnScreen;
        var newCurrentContent = this.isOnScreen ? Content : null;

        if (newCurrentContent != CurrentContent)
            CurrentContent = newCurrentContent;
    }
    internal virtual void OnTabTapped(TabTappedEventArgs e)
    {
        if (IsEnabled)
        {
            var handler = TabTapped;
            handler?.Invoke(this, e);

            if (TapCommand != null)
                TapCommand.Execute(null);
        }
    }
    #endregion
}
