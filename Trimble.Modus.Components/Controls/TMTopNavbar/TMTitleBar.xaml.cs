using CommunityToolkit.Maui.Behaviors;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public partial class TMTitleBar : ContentView
{
    #region Bindable Properties
    public static readonly BindableProperty SearchBarVisibleProperty = BindableProperty.Create(nameof(SearchBarVisible), typeof(Boolean), typeof(TMTitleBar), defaultValue: false);
    public static readonly BindableProperty RightSideContentProperty = BindableProperty.Create(nameof(RightSideContent), typeof(View),typeof(TMTitleBar), defaultValue: null);
    public static readonly BindableProperty LeftSideContentProperty = BindableProperty.Create(nameof(LeftSideContent), typeof(View), typeof(TMTitleBar), defaultValue: null);
    public static readonly BindableProperty AllowSearchProperty = BindableProperty.Create(nameof(AllowSearch), typeof(bool), typeof(TMTitleBar), propertyChanged: AllowSearchPropertyChanged);
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TMTitleBar));
    public static readonly BindableProperty TitleBarBackgroundColorProperty = BindableProperty.Create(nameof(TitleBarBackgroundColor), typeof(Color), typeof(TMTitleBar), Colors.Black, propertyChanged: OnTitleBarBackgroundColorChanged);
    public static readonly BindableProperty TopNavBarThemeProperty = BindableProperty.Create(nameof(TopNavBarTheme), typeof(TopNavBarThemes), typeof(TMTitleBar), propertyChanged: OnTitleBarColorThemeCHanged, defaultBindingMode: BindingMode.TwoWay);
    public static readonly BindableProperty IconTintColorProperty = BindableProperty.Create(nameof(IconTintColor), typeof(Color), typeof(TMTitleBar), null, propertyChanged: OnIconTintColorChanged, defaultBindingMode: BindingMode.TwoWay);
    public static readonly BindableProperty SearchBarColorProperty = BindableProperty.Create(nameof(SearchBarColor), typeof(Color), typeof(TMTitleBar), null);
    public static readonly BindableProperty SearchBarIconColorProperty = BindableProperty.Create(nameof(SearchBarIconColor), typeof(Color), typeof(TMTitleBar), null, propertyChanged: OnSearchBarIconColorChanged, defaultBindingMode: BindingMode.TwoWay);
    #endregion

    #region Public Properties
    public TMFlyoutPage flyoutPageReference;
    public TopNavBarThemes TopNavBarTheme
    {
        get => (TopNavBarThemes)GetValue(TopNavBarThemeProperty);
        set => SetValue(TopNavBarThemeProperty, value);
    }
    public Color SearchBarIconColor
    {
        get => (Color)GetValue(SearchBarIconColorProperty);
        set => SetValue(SearchBarIconColorProperty, value);
    }
    public Color IconTintColor
    {
        get => (Color)GetValue(IconTintColorProperty);
        set => SetValue(IconTintColorProperty, value);
    }
    public Color SearchBarColor
    {
        get => (Color)GetValue(SearchBarColorProperty);
        set => SetValue(SearchBarColorProperty, value);
    }
    public Color TitleBarBackgroundColor
    {
        get => (Color)GetValue(TitleBarBackgroundColorProperty);
        set => SetValue(TitleBarBackgroundColorProperty, value);
    }
    public Boolean SearchBarVisible
    {
        get => (Boolean)GetValue(SearchBarVisibleProperty);
        set => SetValue(SearchBarVisibleProperty, value);
    }
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public View RightSideContent
    {
        get => (View)GetValue(RightSideContentProperty);
        set => SetValue(RightSideContentProperty, value);
    }
    public View LeftSideContent
    {
        get => (View)GetValue(LeftSideContentProperty);
        set => SetValue(LeftSideContentProperty, value);
    }
    public bool AllowSearch
    {
        get => (bool)GetValue(AllowSearchProperty);
        set => SetValue(AllowSearchProperty, value);
    }
    #endregion

    public TMTitleBar()
    {
        InitializeComponent();
        this.SetDynamicResource(StyleProperty, "TitleBarPrimaryStyle");
    }

    /// <summary>
    /// Animation to display search bar
    /// </summary>
    void SearchIconClicked(System.Object sender, System.EventArgs e)
    {
        searchBarSpace.FadeTo(1, 300);
        mmbar.FadeTo(0, 300);
        mmbar.IsVisible = false;
        mmbar.HeightRequest = 0;
        searchBarSpace.IsVisible = true;
        searchBarSpace.HeightRequest = 64;
    }

    /// <summary>
    /// Search bar close animation
    /// </summary>
    void SearchBarCloseIconClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        searchBarSpace.FadeTo(0, 300);
        mmbar.FadeTo(1, 300);
        mmbar.IsVisible = true;
        mmbar.HeightRequest = 64;
        searchBarSpace.IsVisible = false;
        searchBarSpace.HeightRequest = 0;
    }

    /// <summary>
    /// Expose SearchBar to user
    /// </summary>
    public TMInput GetSearchBar()
    {
        return searchBar;
    }

    /// <summary>
    /// Display flyout menu
    /// </summary>
    void ShowFlyoutMenu(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        flyoutPageReference.IsPresented = true;
    }

    /// <summary>
    /// Update icon color for search bar icon
    /// </summary>
    private static void OnSearchBarIconColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var titleBar = (bindable as TMTitleBar);
        var behavior = new IconTintColorBehavior
        {
            TintColor = (Color)newValue
        };

        titleBar.searchBarIcon.Behaviors.Clear();
        titleBar.searchBarIcon.Behaviors.Add(behavior);
    }

    /// <summary>
    /// Set primary and secondary color schemes
    /// </summary>
    private static void OnTitleBarColorThemeCHanged(BindableObject bindable, object oldValue, object newValue)
    {
        var titleBar = (bindable as TMTitleBar);
        if ((TopNavBarThemes)newValue == TopNavBarThemes.Primary)
        {
            titleBar.SetDynamicResource(StyleProperty, "TitleBarPrimaryStyle");
        }
        else
        {
            titleBar.SetDynamicResource(StyleProperty, "TitleBarSecondaryStyle");
        }
    }

    /// <summary>
    /// Set title bar background color
    /// </summary>
    private static void OnTitleBarBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var titleBar = (bindable as TMTitleBar);
        titleBar.BackgroundColor = (Color)newValue;
    }

    /// <summary>
    /// update icon tint color
    /// </summary>
    private static void OnIconTintColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var titleBar = (bindable as TMTitleBar);
        var behavior = new IconTintColorBehavior
        {
            TintColor = (Color)newValue
        };

        titleBar.closeIcon.Behaviors.Clear();
        titleBar.closeIcon.Behaviors.Add(behavior);

        titleBar.searchIcon.Behaviors.Clear();
        titleBar.searchIcon.Behaviors.Add(behavior);

        titleBar.hamburgerIcon.Behaviors.Clear();
        titleBar.hamburgerIcon.Behaviors.Add(behavior);
    }

    /// <summary>
    /// Show search icon to access search bar
    /// </summary>
    private static void AllowSearchPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TMTitleBar titleBar = bindable as TMTitleBar;
        if ((bool)newValue)
        {
            titleBar.searchIcon.Source = "search_icon.png";
            titleBar.searchIcon.Behaviors.Clear();
            var behavior = new IconTintColorBehavior
            {
                TintColor = titleBar.IconTintColor
            };
            titleBar.searchIcon.Behaviors.Add(behavior);
        }
        else
        {
            titleBar.searchIcon.Source = null;
        }
        titleBar.searchIcon.IsVisible = (bool)newValue;
    }
}

