using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;
using CommunityToolkit.Maui.Behaviors;

namespace Trimble.Modus.Components;

[ContentProperty(nameof(ContentView))]
public partial class TabViewItem : ContentView
{
    public const string SelectedVisualState = "Selected";
    public const string UnselectedVisualState = "Unselected";

    internal TabColor TabColor
    {
        get => (TabColor)GetValue(TabColorProperty);
        set => SetValue(TabColorProperty, value);
    }

    public static readonly BindableProperty TabColorProperty =
    BindableProperty.Create(nameof(TabColor), typeof(TabColor), typeof(TabViewItem), TabColor.Primary,
        propertyChanged: OnTabColorChanged);

    private static void OnTabColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if(bindable != null && bindable is TabViewItem item) {
            Console.WriteLine("bindable"+newValue);
            if(newValue is TabColor)
            {
                Console.WriteLine($"TabColor: {newValue}");
                item.UpdateCurrent();
            }
            
        }
    }

    public delegate void TabTappedEventHandler(object? sender, TabTappedEventArgs e);

    public event TabTappedEventHandler? TabTapped;

    public static readonly BindableProperty TapCommandProperty =
       BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(TabViewItem), null);

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    bool isOnScreen;

    public static BindableProperty TabAnimationProperty =
    BindableProperty.Create(nameof(TabAnimation), typeof(ITabViewItemAnimation), typeof(TabViewItem), null);

    internal static readonly BindablePropertyKey CurrentContentPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentContent), typeof(View), typeof(TabViewItem), null);

    public static readonly BindableProperty CurrentContentProperty = CurrentContentPropertyKey.BindableProperty;

    public View? CurrentContent
    {
        get => (View?)GetValue(CurrentContentProperty);
        private set => SetValue(CurrentContentPropertyKey, value);
    }

    internal static readonly BindablePropertyKey CurrentIconPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentIcon), typeof(ImageSource), typeof(TabViewItem), null);

    public static readonly BindableProperty CurrentIconProperty = CurrentIconPropertyKey.BindableProperty;

    public ImageSource? CurrentIcon
    {
        get => (ImageSource?)GetValue(CurrentIconProperty);
        private set => SetValue(CurrentIconPropertyKey, value);
    }

    internal static readonly BindablePropertyKey CurrentTextColorPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentTextColor), typeof(Color), typeof(TabViewItem), Colors.Black);

    public static readonly BindableProperty CurrentTextColorProperty = CurrentTextColorPropertyKey.BindableProperty;

    public Color CurrentTextColor
    {
        get => (Color)GetValue(CurrentTextColorProperty);
        private set => SetValue(CurrentTextColorPropertyKey, value);
    }
    public ITabViewItemAnimation? TabAnimation
    {
        get => (ITabViewItemAnimation?)GetValue(TabAnimationProperty);
        set => SetValue(TabAnimationProperty, value);
    }

    public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(TabViewItem), string.Empty);

    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TabViewItem), Colors.Black,
            propertyChanged: OnTabViewItemPropertyChanged);

    public static readonly BindableProperty OrientationProperty =
         BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(TabViewItem), StackOrientation.Vertical,propertyChanged:OnOrientationPropertyChanged);

    private static void OnOrientationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is TabViewItem item)
        {
            Console.WriteLine("bindable" + newValue);
            if (newValue is StackOrientation)
            {
                Console.WriteLine($"TabColor: {newValue}");
                item.tabItem.Orientation = (StackOrientation)newValue;
                if((StackOrientation)newValue == StackOrientation.Horizontal)
                item.tabItem.Margin = new Thickness(12,4,12,4);
            }

        }
    }

    internal StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty TextColorSelectedProperty =
        BindableProperty.Create(nameof(TextColorSelected), typeof(Color), typeof(TabViewItem), Colors.White);

    public Color TextColorSelected
    {
        get => (Color)GetValue(TextColorSelectedProperty);
        set => SetValue(TextColorSelectedProperty, value);
    }

    public static readonly BindableProperty IconProperty =
          BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(TabViewItem), null,
              propertyChanged: OnTabViewItemPropertyChanged);

    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly BindableProperty IconSelectedProperty =
      BindableProperty.Create(nameof(IconSelected), typeof(ImageSource), typeof(TabViewItem), null,
          propertyChanged: OnTabViewItemPropertyChanged);

    public ImageSource? IconSelected
    {
        get => (ImageSource?)GetValue(IconSelectedProperty);
        set => SetValue(IconSelectedProperty, value);
    }


    public static readonly BindableProperty ContentPageProperty =
            BindableProperty.Create(nameof(ContentPage), typeof(Page), typeof(TabViewItem));

    public Page? ContentPage
    {
        get => (Page?)GetValue(ContentPageProperty);
        set => SetValue(ContentPageProperty, value);
    }

    public View? ContentView
    {
        get => (View?)GetValue(ContentViewProperty);
        set => SetValue(ContentViewProperty, value);
    }

    public static readonly BindableProperty ContentViewProperty =
            BindableProperty.Create(nameof(ContentPage), typeof(View), typeof(TabViewItem));

    public static readonly BindableProperty IsSelectedProperty =
    BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TabViewItem), false,
        propertyChanged: OnIsSelectedChanged);

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    static async void OnIsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TabViewItem tabViewItem)
        {
            tabViewItem.UpdateCurrent();
        }
    }

    public TabViewItem()
    {
        InitializeComponent();
        BindingContext = this;

    }
    void UpdateCurrent()
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
        selectedBorder.BackgroundColor = !IsSelected ? Colors.Transparent : ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
        UpdateCurrentContent();
        ApplyIsSelectedState();
    }
    internal void UpdateCurrentContent(bool isOnScreen = true)
    {
        this.isOnScreen = isOnScreen;
        var newCurrentContent = this.isOnScreen ? Content : null;

        if (newCurrentContent != CurrentContent)
            CurrentContent = newCurrentContent;
    }

    void ApplyIsSelectedState()
    {
        if (IsSelected)
            VisualStateManager.GoToState(this, SelectedVisualState);
        else
            VisualStateManager.GoToState(this, UnselectedVisualState);
    }

    static void OnTabViewItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as TabViewItem)?.UpdateCurrent();
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
}
public interface ITabViewItemAnimation
{
    Task OnSelected(View tabViewItem);

    Task OnDeSelected(View tabViewItem);
}
