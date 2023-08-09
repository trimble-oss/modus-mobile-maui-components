using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

[ContentProperty(nameof(AccordionContentView))]
public partial class TMAccordion : ContentView
{
    #region Bindable Property
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(AccordionSize), typeof(TMAccordion), AccordionSize.Default);
    public static readonly BindableProperty ChevronPositionProperty = BindableProperty.Create(nameof(ChevronPosition), typeof(AccordionChevronPosition), typeof(TMAccordion), AccordionChevronPosition.Right);
    public static readonly BindableProperty LeftIconSourceProperty = BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(TMAccordion), null);
    public static readonly BindableProperty RightIconSourceProperty = BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(TMAccordion), null);
    public static readonly BindableProperty ContentViewProperty = BindableProperty.Create(nameof(AccordionContentView), typeof(View), typeof(TMAccordion), default(View));
    public static readonly BindableProperty AccordionSubtitleProperty = BindableProperty.Create(nameof(AccordionSubtitle), typeof(string), typeof(TMAccordion), default(string));
    public static readonly BindableProperty AccordionTitleProperty = BindableProperty.Create(nameof(AccordionTitle), typeof(string), typeof(TMAccordion), string.Empty);
    public static readonly BindableProperty IsOpenBindablePropertyProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(TMAccordion), false, propertyChanged: IsOpenChanged);
    #endregion
    #region Public Property
    /// <summary>
    /// Define the size of the accordion
    /// </summary>
    public AccordionSize Size
    {
        get => (AccordionSize)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
    /// <summary>
    /// Define the position of the chevron
    /// </summary>
    public AccordionChevronPosition ChevronPosition
    {
        get => (AccordionChevronPosition)GetValue(ChevronPositionProperty);
        set => SetValue(ChevronPositionProperty, value);
    }
    /// <summary>
    /// ImageSource for the left icon
    /// </summary>
    public ImageSource LeftIconSource
    {
        get => (ImageSource)GetValue(LeftIconSourceProperty);
        set => SetValue(LeftIconSourceProperty, value);
    }
    /// <summary>
    /// ImageSource for the right icon
    /// </summary>
    public ImageSource RightIconSource
    {
        get => (ImageSource)GetValue(RightIconSourceProperty);
        set => SetValue(RightIconSourceProperty, value);
    }

    /// <summary>
    /// Content of the accordion
    /// </summary>
    public View AccordionContentView
    {
        get => (View)GetValue(ContentViewProperty);
        set => SetValue(ContentViewProperty, value);
    }

    /// <summary>
    /// Title of the accordion
    /// </summary>
    public string AccordionTitle
    {
        get => (string)GetValue(AccordionTitleProperty);
        set => SetValue(AccordionTitleProperty, value);
    }
    /// <summary>
    /// Description of the accordion
    /// </summary>
    public string AccordionSubtitle
    {
        get => (string)GetValue(AccordionSubtitleProperty);
        set => SetValue(AccordionSubtitleProperty, value);
    }

    /// <summary>
    /// Set or Get the state of the accordion
    /// </summary>
    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenBindablePropertyProperty); }
        set { SetValue(IsOpenBindablePropertyProperty, value); }
    }

    public event EventHandler ExpandedStateChanged;
    public uint AnimationDuration { get; set; } = 250;
    #endregion

    #region Constructor
    public TMAccordion()
    {
        InitializeComponent();
        Close();
        IsOpen = false;
    }
    #endregion

    #region Private Method
    /// <summary>
    /// Change the visual state of the accordion to update the UI of the accordion based on its state
    /// </summary>
    private static void IsOpenChanged(BindableObject bindable, object oldValue, object newValue)
    {
        bool isOpen;

        if (bindable != null && newValue != null)
        {
            var control = (TMAccordion)bindable;
            isOpen = (bool)newValue;
            control.ExpandedStateChanged?.Invoke(control, null);
            if (control.IsOpen == false)
            {
                VisualStateManager.GoToState(control, "Open");
                control.Close();
            }
            else
            {
                VisualStateManager.GoToState(control, "Closed");
                control.Open();
            }
        }
    }
    /// <summary>
    /// Closing the accordion
    /// </summary>
    private async void Close()
    {
        await Task.WhenAll(
            _accContent.TranslateTo(0, -10, AnimationDuration),
            _indicatorContainer.RotateTo(0, AnimationDuration),
            _accContent.FadeTo(0, 50)
            );
        _accContent.IsVisible = false;
    }

    /// <summary>
    /// Opening the accordion
    /// </summary>
    private async void Open()
    {
        _accContent.IsVisible = true;
        await Task.WhenAll(
            _accContent.TranslateTo(0, 10, AnimationDuration),
            _indicatorContainer.RotateTo(-180, AnimationDuration),
            _accContent.FadeTo(30, 50, Easing.SinIn)
        );
    }

    /// <summary>
    /// Trigger the IsOpen property when the title is tapped
    /// </summary>
    private void TitleTapped(object sender, EventArgs e)
    {
        if (IsEnabled)
        {
            IsOpen = !IsOpen;
        }
    }
    #endregion
}
