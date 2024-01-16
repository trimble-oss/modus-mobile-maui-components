namespace Trimble.Modus.Components;

public partial class TMTitleBar : ContentView
{
    #region Bindable Properties
    public static readonly BindableProperty SearchBarVisibleProperty = BindableProperty.Create(nameof(SearchBarVisible),
        typeof(Boolean),
        typeof(TMTitleBar),
        defaultValue: false);
    public static readonly BindableProperty RightSideContentProperty = BindableProperty.Create(nameof(RightSideContent),
        typeof(View),
        typeof(TMTitleBar),
        defaultValue: null);
    public static readonly BindableProperty LeftSideContentProperty = BindableProperty.Create(nameof(LeftSideContent),
        typeof(View),
        typeof(TMTitleBar),
        defaultValue: null);
    public static readonly BindableProperty AllowSearchProperty = BindableProperty.Create(nameof(AllowSearch),
        typeof(bool),
        typeof(TMTitleBar),
        propertyChanged: AllowSearchPropertyChanged);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title),
            typeof(string),
            typeof(TMTitleBar));
    #endregion

    #region Public Properties
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
    }

    private static void AllowSearchPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TMTitleBar titleBar = bindable as TMTitleBar;
        if((bool)newValue)
        {
            titleBar.searchIcon.Source = "search_icon.png";
        }
        else
        {
            titleBar.searchIcon.Source = null;
        }
        titleBar.searchIcon.IsVisible = (bool)newValue;
    }

    void SearchIconClicked(System.Object sender, System.EventArgs e)
    {
        searchBarSpace.FadeTo(1, 300);
        mmbar.FadeTo(0, 300);
        mmbar.IsVisible = false;
        searchBarSpace.IsVisible = true;
    }

    void SearchBarCloseIconClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        searchBarSpace.FadeTo(0, 300);
        mmbar.FadeTo(1, 300);
        mmbar.IsVisible = true;
        searchBarSpace.IsVisible = false;
    }
}

