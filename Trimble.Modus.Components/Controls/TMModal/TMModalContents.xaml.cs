using CommunityToolkit.Maui.Behaviors;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Modal;

public partial class TMModalContents
{
    #region Private fields
    private bool _dangerButtonAdded = false;

    private TMButton _primaryButton;

    private TMButton _secondaryButton;

    private TMButton _tertiaryButton;

    private TMButton _dangerButton;

    #endregion
    #region Bindable Properties

    /// <summary>
    /// Gets or sets the text for the title label in the control
    /// </summary>
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TMModal), null, BindingMode.TwoWay);

    /// <summary>
    /// Gets or sets the text for the modal description in the control
    /// </summary>
    public static readonly BindableProperty MessageProperty =
        BindableProperty.Create(nameof(Message), typeof(string), typeof(TMModal), null, BindingMode.TwoWay);

    /// <summary>
    /// Gets or sets the image source for the left icon in the title
    /// </summary>
    public static readonly BindableProperty TitleIconSourceProperty =
        BindableProperty.Create(nameof(TitleIcon), typeof(ImageSource), typeof(TMModal), null, BindingMode.TwoWay);

    /// <summary>
    /// Gets or sets the button width option
    /// </summary>
    public static readonly BindableProperty FullWidthButtonProperty =
        BindableProperty.Create(nameof(FullWidthButton), typeof(bool), typeof(TMModal), false, BindingMode.TwoWay ,propertyChanged: OnFullWidthButtonChanged);

    /// <summary>
    /// Bindable property for text and image color for theme
    /// </summary>
    public static readonly BindableProperty TextAndImageColorProperty =
        BindableProperty.Create(nameof(TextAndImageColor), typeof(Color), typeof(TMModal), null, BindingMode.Default, propertyChanged: OnTextAndImageColorChanged);

    /// <summary>
    /// Bindable property for modal background color based on theme
    /// </summary>
    public static readonly BindableProperty ModalBackgroundColorProperty =
        BindableProperty.Create(nameof(ModalBackgroundColor), typeof(Color), typeof(TMModal), Colors.Black, BindingMode.Default, propertyChanged: OnModalBackgroundColor);

    #endregion

    #region Public properties

    /// <summary>
    /// Background Color for specific themes
    /// </summary>
    internal Color ModalBackgroundColor
    {
        get { return (Color)GetValue(ModalBackgroundColorProperty); }
        set { this.SetValue(ModalBackgroundColorProperty, value); }
    }

    /// <summary>
    /// Text Color for specific themes
    /// </summary>
    internal Color TextAndImageColor
    {
        get { return (Color)GetValue(TextAndImageColorProperty); }
        set { this.SetValue(TextAndImageColorProperty, value); }
    }

    /// <summary>
    /// Gets or sets title text
    /// </summary>
    public new string Title
    {
        get { return (string)GetValue(TitleTextProperty); }
        set { SetValue(TitleTextProperty, value); }
    }

    /// <summary>
    /// Gets or sets modal body text
    /// </summary>
    public string Message
    {
        get { return (string)GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    /// <summary>
    /// Gets or sets title icon source
    /// </summary>
    public ImageSource TitleIcon
    {
        get { return (ImageSource)GetValue(TitleIconSourceProperty); }
        set { SetValue(TitleIconSourceProperty, value); }
    }

    /// <summary>
    /// Gets or sets the full width button option
    /// </summary>
    public bool FullWidthButton
    {
        get { return (bool)GetValue(FullWidthButtonProperty); }
        set { SetValue(FullWidthButtonProperty, value); }
    }

    /// <summary>
    /// Gets or sets the CloseAction
    /// </summary>
    public Action OnModalClosing { get; set; }

    /// <summary>
    /// Action triggered when primary button is clicked
    /// </summary>
    public event Action PrimaryButtonClicked;

    /// <summary>
    /// Action triggered when secondary button is clicked
    /// </summary>
    public event Action SecondaryButtonClicked;

    /// <summary>
    /// Action triggered when Tertiary button is clicked
    /// </summary>
    public event Action TertiaryButtonClicked;

    /// <summary>
    /// Action triggered when Danger button is clicked
    /// </summary>
    public event Action DangerButtonClicked;

    #endregion

    #region Constructor
	internal TMModalContents(string titleText, string messageText = null, ImageSource titleIconSource = null, bool fullWidthButton = false)
	{
		InitializeComponent();
        Title = titleText;
        TitleIcon = titleIconSource;
        FullWidthButton = fullWidthButton;
        Message = messageText;
        BindingContext = this;

        // Bind color to dynamic resource
        this.SetDynamicResource(StyleProperty, "TMModalStyle");
    }
    #endregion

    #region internal methods
    /// <summary>
    /// Add Primary button
    /// </summary>
    /// <param name="title">Button titke</param>
    /// <param name="clickAction">Button click action</param>
    /// <exception cref="ArgumentNullException">Thrown when title is empty</exception>
    /// <exception cref="InvalidOperationException">Thrown while trying to add more that 3 button</exception>
    internal void AddPrimaryAction(string title, Action clickAction = null)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(Constants.ButtonEmptyTitleError);
        }
        if (ButtonContainer.Children.Count >= 3)
        {
            throw new InvalidOperationException(Constants.ButtonLimitError);
        }
        ConstructPrimaryButton(title, clickAction);
    }

    /// <summary>
    /// Add Secondary button
    /// </summary>
    /// <param name="title">Button titke</param>
    /// <param name="clickAction">Button click action</param>
    /// <exception cref="ArgumentNullException">Thrown when title is empty</exception>
    /// <exception cref="InvalidOperationException">Thrown while trying to add more that 3 button</exception>
    internal void AddSecondaryAction(string title, Action clickAction = null)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(Constants.ButtonEmptyTitleError);
        }
        if (ButtonContainer.Children.Count >= 3)
        {
            throw new InvalidOperationException(Constants.ButtonLimitError);
        }
        ConstructSecondaryButton(title, clickAction);
    }

    /// <summary>
    /// Add Tertiary button
    /// </summary>
    /// <param name="title">Button titke</param>
    /// <param name="clickAction">Button click action</param>
    /// <exception cref="ArgumentNullException">Thrown when title is empty</exception>
    /// <exception cref="InvalidOperationException">Thrown while trying to add more that 3 button</exception>
    internal void AddTertiaryAction(string title, Action clickAction = null)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(Constants.ButtonEmptyTitleError);
        }
        if (ButtonContainer.Children.Count >= 3)
        {
            throw new InvalidOperationException(Constants.ButtonLimitError);
        }
        ConstructTertiaryButton(title, clickAction);
    }

    /// <summary>
    /// Add Danger button
    /// </summary>
    /// <param name="title">Button titke</param>
    /// <param name="clickAction">Button click action</param>
    /// <exception cref="ArgumentNullException">Thrown when title is empty</exception>
    /// <exception cref="InvalidOperationException">Thrown while trying to add more that 3 button</exception>
    internal void AddDangerAction(string title, Action clickAction = null)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(Constants.ButtonEmptyTitleError);
        }
        if (ButtonContainer.Children.Count >= 3)
        {
            throw new InvalidOperationException(Constants.ButtonLimitError);
        }
        if (_dangerButtonAdded)
        {
            _dangerButton.Text = title;
            DangerButtonClicked = clickAction;
        }
        else
        {
            ConstructDangerButton(title, clickAction);
        }
    }

    /// <summary>
    /// Adds TMInput in the body of the modal
    /// </summary>
    /// <param name="inputConfigurationHandler">Handler to configure the TMInput</param>
    internal void AddTextInput(Action<TMInput> inputConfigurationHandler = null)
    {
        var inputControl = new TMInput();

        inputConfigurationHandler?.Invoke(inputControl);

        ModalBodyContainer.Add(inputControl);
    }

    /// <summary>
    /// Display the modal
    /// </summary>
    internal void Show(bool closeWhenBackgroundClicked)
    {
        CloseWhenBackgroundIsClicked = closeWhenBackgroundClicked;
        PopupService.Instance.PresentAsync(this);
    }

    /// <summary>
    /// Close modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal void CloseModal(object sender, EventArgs e)
    {
        OnModalClosing?.Invoke();
        PopupService.Instance.RemovePageAsync(this, true);
    }
    #endregion

    #region Private methods
    /// <summary>
    /// Update modal background color
    /// </summary>
    private static void OnModalBackgroundColor(BindableObject bindable, object _, object newValue)
    {
        (bindable as TMModalContents).ParentContainer.BackgroundColor = (Color)newValue;
    }

    /// <summary>
    /// Update text and image color
    /// </summary>
    private static void OnTextAndImageColorChanged(BindableObject bindable, object _, object newValue)
    {
        TMModalContents modal = (TMModalContents)bindable;
        modal.TitleLabel.TextColor = (Color)newValue;
        modal.MessageLabel.TextColor = (Color)newValue;
        modal.UpdateIconColor();
    }

    /// <summary>
    /// Update Icon color when theme changes
    /// </summary>
    private void UpdateIconColor()
    {
        // FIXME: IconTintColorBehavior doesn't work properly on Windows, hence the DeviceInfo.Platform != DevicePlatform.WinUI check. 
        // Remove this check once the issue is fixed.
        if (DeviceInfo.Platform != DevicePlatform.WinUI)
        {
            CloseButton.Behaviors.Clear();
            IconImage.Behaviors.Clear();
            if (TextAndImageColor != null)
            {
                var behavior = new IconTintColorBehavior
                {
                    TintColor = TextAndImageColor
                };
                CloseButton.Behaviors.Add(behavior);
                IconImage.Behaviors.Add(behavior);
            }
        }
    }

    /// <summary>
    /// Triggered when FullWidthButton option is changed
    /// </summary>
    /// <param name="bindable">Object</param>
    /// <param name="oldValue">Old bool value</param>
    /// <param name="newValue">New bool value</param>
    private static void OnFullWidthButtonChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var modal = (TMModalContents)bindable;

        if (modal.ButtonContainer != null)
        {
            if ((bool)newValue)
            {
                modal.ButtonContainer.Orientation = StackOrientation.Vertical;
                modal.ButtonContainer.HorizontalOptions = LayoutOptions.FillAndExpand;

                foreach (TMButton button in modal.ButtonContainer.Children)
                {
                    button.HorizontalOptions = LayoutOptions.FillAndExpand;
                }
            }
            else
            {
                modal.ButtonContainer.Orientation = StackOrientation.Horizontal;
                modal.ButtonContainer.HorizontalOptions = LayoutOptions.End;

                foreach (TMButton button in modal.ButtonContainer.Children)
                {
                    button.HorizontalOptions = LayoutOptions.Start;
                }
            }

            if ((bool)oldValue != (bool)newValue)
            {
                modal.ReverseButtonOrder(modal.ButtonContainer);
            }
        }
    }

    /// <summary>
    /// Constructs primary button
    /// </summary>
    /// <param name="primaryText"></param>
    /// <param name="primaryButtonClick"></param>
    private void ConstructPrimaryButton(string primaryText = null, Action primaryButtonClick = null)
    {
        PrimaryButtonClicked = primaryButtonClick;
        if (!string.IsNullOrEmpty(primaryText))
        {
            _primaryButton = new TMButton
            {
                Text = primaryText,
                Size = Enums.Size.Small,
                ButtonColor = ButtonColor.Primary,
                HorizontalOptions = FullWidthButton ? LayoutOptions.FillAndExpand : LayoutOptions.Start
            };
            _primaryButton.Clicked += OnPrimaryButtonClicked;
            ButtonContainer.Children.Insert(FullWidthButton ? ButtonContainer.Children.Count : 0, _primaryButton);
        }
    }

    /// <summary>
    /// Invoke on click action and closes the modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnPrimaryButtonClicked(object sender, EventArgs e)
    {
        CloseModal(sender, e);
        PrimaryButtonClicked?.Invoke();
    }

    /// <summary>
    /// Construct secondary button
    /// </summary>
    /// <param name="secondaryText"></param>
    /// <param name="secondaryButtonClick"></param>
    private void ConstructSecondaryButton(string secondaryText, Action secondaryButtonClick = null)
    {
        SecondaryButtonClicked = secondaryButtonClick;
        if (!string.IsNullOrEmpty(secondaryText))
        {
            _secondaryButton = new TMButton
            {
                Text = secondaryText,
                ButtonColor = ButtonColor.Secondary,
                ButtonStyle = ButtonStyle.Outline,
                Size = Enums.Size.Small,
                HorizontalOptions = FullWidthButton ? LayoutOptions.FillAndExpand : LayoutOptions.Start
            };

            _secondaryButton.Clicked += OnSecondaryButtonClicked;
            ButtonContainer.Children.Insert(FullWidthButton ? ButtonContainer.Children.Count : 0, _secondaryButton);
        }
    }

    /// <summary>
    /// Invoke on click action and closes the modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSecondaryButtonClicked(object sender, EventArgs e)
    {
        CloseModal(sender, e);
        SecondaryButtonClicked?.Invoke();
    }

    /// <summary>
    /// Construct tertiary button
    /// </summary>
    /// <param name="tertiaryText"></param>
    /// <param name="tertiaryButtonClick"></param>
    private void ConstructTertiaryButton(string tertiaryText = null, Action tertiaryButtonClick = null)
    {
        TertiaryButtonClicked = tertiaryButtonClick;
        if (!string.IsNullOrEmpty(tertiaryText))
        {
            _tertiaryButton = new TMButton
            {
                Text = tertiaryText,
                ButtonColor = ButtonColor.Tertiary,
                ButtonStyle = ButtonStyle.BorderLess,
                Size = Enums.Size.Small,
                HorizontalOptions = FullWidthButton ? LayoutOptions.FillAndExpand : LayoutOptions.Start
            };

            _tertiaryButton.Clicked += OnTertiaryButtonClicked;
            ButtonContainer.Children.Insert(FullWidthButton ? ButtonContainer.Children.Count : 0, _tertiaryButton);
        }
    }

    /// <summary>
    /// Invoke on click action and closes the modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnTertiaryButtonClicked(object sender, EventArgs e)
    {
        CloseModal(sender, e);
        TertiaryButtonClicked?.Invoke();
    }

    /// <summary>
    /// Construct danger button
    /// </summary>
    /// <param name="destructiveText"></param>
    /// <param name="destructiveButtonClick"></param>
    private void ConstructDangerButton(string destructiveText = null, Action destructiveButtonClick = null)
    {
        DangerButtonClicked = destructiveButtonClick;

        if (!string.IsNullOrEmpty(destructiveText))
        {
            _dangerButton = new TMButton
            {
                Text = destructiveText,
                ButtonColor = ButtonColor.Danger,
                Size = Enums.Size.Small,
                HorizontalOptions = FullWidthButton ? LayoutOptions.FillAndExpand : LayoutOptions.Start
            };
            _dangerButton.Clicked += OnDestructiveButtonClicked;
            ButtonContainer.Children.Insert(FullWidthButton ? 0 : ButtonContainer.Children.Count, _dangerButton);
            _dangerButtonAdded = true;
        }
    }

    /// <summary>
    /// Invoke on click action and closes the modal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDestructiveButtonClicked(object sender, EventArgs e)
    {
        CloseModal(sender, e);
        DangerButtonClicked?.Invoke();
    }

    /// <summary>
    /// Reverse button order, triggered when FullWidth property is changed
    /// </summary>
    /// <param name="container">Button container</param>
    private void ReverseButtonOrder(StackLayout container)
    {
        int childCount = container.Children.Count;

        for (int i = childCount - 1; i >= 0; i--)
        {
            var child = container.Children[i];
            container.Children.RemoveAt(i);

            container.Children.Add(child);
        }
    }

    /// <summary>
    /// Update modal size based on the screen dimension
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > height)
        {
            ModalContainer.WidthRequest = width * 0.5;
        }
        else
        {
            ModalContainer.WidthRequest = width * 0.75;
        }
    }
    #endregion
}
