
using Microsoft.Maui.Controls;
//using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using Trimble.Modus.Components.Contants;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Animations;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components
{

    public class TMModal : Popup.Pages.PopupPage
    {
        private readonly Border _border;

        private Grid _baseContainer;

        private readonly StackLayout _buttonContainer;

        private readonly Image _titleIcon = new() { WidthRequest = 0, HeightRequest = 26 };

        private ImageButton _closeButton;

        private readonly Label _titleLabel = new() { HorizontalOptions = LayoutOptions.Start, FontFamily = "OpenSans-Semibold.ttf", FontSize = 16, Padding = new Thickness(0, 0, 8, 0), VerticalOptions = LayoutOptions.Center };

        private StackLayout _modalBodyContainer;

        private Label _descriptionLabel = new() { LineBreakMode = LineBreakMode.WordWrap, VerticalOptions = LayoutOptions.StartAndExpand };

        private TMButton _primaryButton;

        private TMButton _secondaryButton;

        private TMButton _tertiaryButton;

        private TMButton _dangerButton;

        private bool _dangerButtonAdded = false;

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
            BindableProperty.Create(nameof(Message), typeof(string), typeof(TMModal), null, BindingMode.TwoWay, propertyChanged: OnMessagePropertyChanged);

        /// <summary>
        /// Gets or sets the image source for the left icon in the title
        /// </summary>
        public static readonly BindableProperty TitleIconSourceProperty =
            BindableProperty.Create(nameof(TitleIcon), typeof(ImageSource), typeof(TMModal), null, BindingMode.TwoWay, propertyChanged: OnTitleIconSourceChanged);

        /// <summary>
        /// Gets or sets the text for the Primary button in the control
        /// </summary>
        public static readonly BindableProperty PrimaryTextProperty =
            BindableProperty.Create(nameof(PrimaryText), typeof(string), typeof(TMModal), null, BindingMode.TwoWay);

        /// <summary>
        /// Gets or sets the text for the secondary button in the control
        /// </summary>
        public static readonly BindableProperty SecondaryTextProperty =
            BindableProperty.Create(nameof(SecondaryText), typeof(string), typeof(TMModal), null, BindingMode.TwoWay);

        /// <summary>
        /// Gets or sets the text for the tertiary button in the control
        /// </summary>
        public static readonly BindableProperty TertiaryTextProperty =
            BindableProperty.Create(nameof(TertiaryText), typeof(string), typeof(TMModal), null, BindingMode.TwoWay);

        /// <summary>
        /// Gets or sets the text for the title label in the control
        /// </summary>
        public static readonly BindableProperty DestructiveButtonTextProperty =
            BindableProperty.Create(nameof(DestructiveButtonText), typeof(string), typeof(TMModal), null, BindingMode.TwoWay);

        /// <summary>
        /// Gets or sets the button alignment option
        /// </summary>
        public static readonly BindableProperty FullWidthButtonProperty =
            BindableProperty.Create(nameof(FullWidthButton), typeof(bool), typeof(TMModal), false, BindingMode.TwoWay, propertyChanged: OnFullWidthButtonChanged);

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets title text
        /// </summary>
        public new string Title
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets title text
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
        /// Gets or sets primary button text
        /// </summary>
        public string PrimaryText
        {
            get { return (string)GetValue(PrimaryTextProperty); }
            set { SetValue(PrimaryTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets secondary button text
        /// </summary>
        public string SecondaryText
        {
            get { return (string)GetValue(SecondaryTextProperty); }
            set { SetValue(SecondaryTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets tertiary button text
        /// </summary>
        public string TertiaryText
        {
            get { return (string)GetValue(TertiaryTextProperty); }
            set { SetValue(TertiaryTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets destructive button text
        /// </summary>
        public string DestructiveButtonText
        {
            get { return (string)GetValue(DestructiveButtonTextProperty); }
            set { SetValue(DestructiveButtonTextProperty, value); }
        }

        public bool FullWidthButton 
        { 
            get { return (bool)GetValue(FullWidthButtonProperty); }
            set { SetValue(FullWidthButtonProperty, value); }
        }

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
        /// Action triggered when Destructive button is clicked
        /// </summary>
        public event Action DestructiveButtonClicked;

        #endregion

        #region Public Methods

        private static void OnFullWidthButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var modal = (TMModal)bindable;
            if ((bool)newValue)
            {
                modal._buttonContainer.Orientation = StackOrientation.Vertical;
                modal._buttonContainer.HorizontalOptions = LayoutOptions.FillAndExpand;

                foreach (TMButton button in modal._buttonContainer.Children)
                {
                    button.HorizontalOptions = LayoutOptions.FillAndExpand;
                    button.frame.HorizontalOptions = LayoutOptions.FillAndExpand;
                    button._titleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
                }
            }
            else
            {
                modal._buttonContainer.Orientation = StackOrientation.Horizontal;
                modal._buttonContainer.HorizontalOptions = LayoutOptions.End;

                foreach (TMButton button in modal._buttonContainer.Children)
                {
                    button.HorizontalOptions = LayoutOptions.Start;
                    button.frame.HorizontalOptions = LayoutOptions.Start;
                    button._titleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
                }
                
            }

            if((bool)oldValue != (bool)newValue)
            {
                modal.ReverseButtonOrder(modal._buttonContainer);
            }

        }

        private void ReverseButtonOrder(StackLayout container)
        {
            int childCount = container.Children.Count;

            // Iterate from the last child to the first child
            for (int i = childCount - 1; i >= 0; i--)
            {
                // Remove the child from the original position
                var child = container.Children[i];
                container.Children.RemoveAt(i);

                // Add the child back at the end of the StackLayout
                container.Children.Add(child);
            }
        }

        private static void OnMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var modal = (TMModal)bindable;
            var text = (string)newValue;
            if (!string.IsNullOrEmpty(text))
            {
                if(modal._modalBodyContainer.Children.Count > 0 && modal._modalBodyContainer.Children[0] is Label)
                {
                    modal._modalBodyContainer.Children.RemoveAt(0);
                }
                modal._descriptionLabel = new Label() { Text = text, LineBreakMode = LineBreakMode.WordWrap, VerticalOptions = LayoutOptions.StartAndExpand };
                modal._modalBodyContainer.Children.Insert(0, modal._descriptionLabel);

            }
            else
            {
                modal._descriptionLabel.HeightRequest = 0;
            }
        }

        public TMModal( string titleText, ImageSource titleIconSource = null, string messageText = null, bool fullWidthButton = false ){
            Title = titleText;
            TitleIcon = titleIconSource;
            ConfigureModal();
            FullWidthButton = fullWidthButton;
            Message = messageText;

            _buttonContainer = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.End, Spacing = 8 };

            _baseContainer.Children.Add(_buttonContainer);
            _baseContainer.SetColumn(_buttonContainer, 0);
            _baseContainer.SetColumnSpan(_buttonContainer, 3);
            _baseContainer.SetRow(_buttonContainer, 2);

            Shadow shadow = new Shadow { Brush = Brush.Black, Radius = 20, Opacity = 0F };

            _border = new Border
            {
                Padding = new Thickness(16),
                Content = _baseContainer,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,
                StrokeShape = new Rectangle
                {
                    RadiusX = 4,
                    RadiusY = 4
                },
                Stroke = (Color)BaseComponent.colorsDictionary()["Black"],
                Shadow = shadow,
                StrokeThickness = 0
            };

            SetBinding();
            Animation = new ScaleAnimation();
            BackgroundColor = Color.FromArgb("#80000000");

            Content = _border;
        }

        public void AddAction(string title, Action clickAction = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("Title cannot be empty");
            }
            if (_buttonContainer.Children.Count >= 3)
            {
                throw new InvalidOperationException("Cannot add more than 3 buttons");
            }

            if (string.IsNullOrEmpty(PrimaryText) && !_dangerButtonAdded)
            {
                ConstructPrimaryButton(title, clickAction);
            }
            else if (string.IsNullOrEmpty(SecondaryText))
            {
                ConstructSecondaryButton(title, clickAction);
            }
            else if (string.IsNullOrEmpty(TertiaryText))
            {
                ConstructTertiaryButton(title, clickAction);
            }
        }

        public void AddDangerButton(string title, Action clickAction = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("Title cannot be empty");
            }
            if (_buttonContainer.Children.Count >= 3)
            {
                throw new InvalidOperationException("Cannot add more than 3 buttons");
            }
            if (_dangerButtonAdded)
            {
                _dangerButton.Title = title;
                DestructiveButtonClicked = clickAction;
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
        public void AddTextInput(Action<TMInput> inputConfigurationHandler = null)
        {
            var inputControl = new TMInput();

            inputConfigurationHandler?.Invoke(inputControl);

            _modalBodyContainer.Add(inputControl);
        }

        public void Show()
        {
            PopupService.Instance.PushAsync(this);
        }

        #endregion

        #region Private methods
        private void SetBinding()
        {
            _titleLabel?.SetBinding(Label.TextProperty, new Binding(nameof(Title), BindingMode.TwoWay, source: this));
            _titleIcon?.SetBinding(Image.SourceProperty, new Binding(nameof(TitleIcon), BindingMode.TwoWay, source: this));

            _primaryButton?.SetBinding(TMButton.TitleProperty, new Binding(nameof(PrimaryText), BindingMode.TwoWay, source: this));
            _secondaryButton?.SetBinding(TMButton.TitleProperty, new Binding(nameof(SecondaryText), BindingMode.TwoWay, source: this));
            _tertiaryButton?.SetBinding(TMButton.TitleProperty, new Binding(nameof(TertiaryText), BindingMode.TwoWay, source: this));
            _dangerButton?.SetBinding(TMButton.TitleProperty, new Binding(nameof(DestructiveButtonText), BindingMode.TwoWay, source: this));
        }

        private void CloseModal(object sender, EventArgs e)
        {
            PopupService.Instance.RemovePageAsync(this, true);
        }

        private static void OnTitleIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TMModal tMModal = bindable as TMModal;
            if(tMModal._titleIcon != null && tMModal._titleLabel != null)
            {
                if ((ImageSource)newValue == null)
                {
                    tMModal._titleIcon.WidthRequest = 0;
                    tMModal._titleLabel.Padding = new Thickness(0, 0, 8, 0);
                }
                else
                {
                    tMModal._titleIcon.WidthRequest = 26;
                    tMModal._titleLabel.Padding = new Thickness(8, 0);
                }
            }
        }

        private void ConfigureModal()
        {

            _baseContainer = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto },
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Auto },
                }
            };

            _baseContainer.Children.Add(_titleLabel);
            _baseContainer.SetColumn(_titleLabel, 1);
            _baseContainer.SetRow(_titleLabel, 0);

            _baseContainer.Children.Add(_titleIcon);
            _baseContainer.SetColumn(_titleIcon, 0);
            _baseContainer.SetRow(_titleIcon, 0);

            _closeButton = new ImageButton { Source = ImageSource.FromResource(ImageConstants.CloseButtonImage), HeightRequest = 16, WidthRequest = 16, HorizontalOptions = LayoutOptions.End };
            _baseContainer.Children.Add(_closeButton);
            _baseContainer.SetColumn(_closeButton, 3);
            _baseContainer.SetRow(_closeButton, 0);


            _modalBodyContainer = new StackLayout { Parent = _baseContainer, Orientation = StackOrientation.Vertical, Spacing = 16};
            _baseContainer.SetColumn(_modalBodyContainer, 0);
            _baseContainer.SetColumnSpan(_modalBodyContainer, 3);
            _baseContainer.SetRow(_modalBodyContainer, 1);

            _baseContainer.Children.Add(_modalBodyContainer);

            _baseContainer.RowSpacing = 16;
            _closeButton.Clicked += CloseModal;
        }

        private void ConstructPrimaryButton(string primaryText = null ,Action primaryButtonClick = null)
        {
            PrimaryText = primaryText;
            PrimaryButtonClicked = primaryButtonClick;
            if (!string.IsNullOrEmpty(PrimaryText))
            {
                _primaryButton = new TMButton
                {
                    Title = PrimaryText,
                    HorizontalOptions = LayoutOptions.End,
                    Size = Enums.Size.Small,
                    ButtonColor = ButtonColor.Primary
                };
                _primaryButton.Clicked += OnPrimaryButtonClicked;
                _primaryButton._titleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;

                if (FullWidthButton)
                {
                    _primaryButton.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _primaryButton.frame.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _buttonContainer.Children.Add(_primaryButton);
                }
                else
                {
                    _primaryButton.HorizontalOptions = LayoutOptions.Start;
                    _primaryButton.frame.HorizontalOptions = LayoutOptions.Start;
                    _buttonContainer.Children.Insert(0, _primaryButton);
                }
            }
        }

        private void OnPrimaryButtonClicked(object sender, EventArgs e)
        {
            CloseModal(sender, e);
            PrimaryButtonClicked?.Invoke();
        }

        private void ConstructSecondaryButton(string secondaryText, Action secondaryButtonClick = null)
        {
            SecondaryText = secondaryText;
            SecondaryButtonClicked = secondaryButtonClick;
            if (!string.IsNullOrEmpty(SecondaryText))
            {
                _secondaryButton = new TMButton
                {
                    Title = SecondaryText,
                    ButtonColor = ButtonColor.Secondary,
                    ButtonStyle = ButtonStyle.Outline,
                    HorizontalOptions = LayoutOptions.End,
                    Size = Enums.Size.Small
                };

                _secondaryButton._titleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;

                _secondaryButton.Clicked += OnSecondaryButtonClicked;
                if (FullWidthButton)
                {
                    _secondaryButton.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _secondaryButton.frame.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _buttonContainer.Children.Add(_secondaryButton);
                }
                else
                {
                    _secondaryButton.HorizontalOptions = LayoutOptions.Start;
                    _secondaryButton.frame.HorizontalOptions = LayoutOptions.Start;
                    _buttonContainer.Children.Insert(0, _secondaryButton);
                }
            }
        }

        private void OnSecondaryButtonClicked(object sender, EventArgs e)
        {
            CloseModal(sender, e);
            SecondaryButtonClicked?.Invoke();
        }

        private void ConstructTertiaryButton(string tertiaryText = null, Action tertiaryButtonClick = null)
        {
            TertiaryText = tertiaryText;
            TertiaryButtonClicked = tertiaryButtonClick;
            if (!string.IsNullOrEmpty(TertiaryText))
            {
                _tertiaryButton = new TMButton
                {
                    Title = TertiaryText,
                    ButtonColor = ButtonColor.Tertiary,
                    ButtonStyle = ButtonStyle.BorderLess,
                    HorizontalOptions = LayoutOptions.End,
                    Size = Enums.Size.Small
                };

                _tertiaryButton.Clicked += OnTertiaryButtonClicked;
                _tertiaryButton._titleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
                if (FullWidthButton)
                {
                    _tertiaryButton.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _tertiaryButton.frame.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _buttonContainer.Children.Add(_tertiaryButton);
                }
                else
                {
                    _tertiaryButton.HorizontalOptions = LayoutOptions.Start;
                    _tertiaryButton.frame.HorizontalOptions = LayoutOptions.Start;
                    _buttonContainer.Children.Insert(0, _tertiaryButton);
                }
            }
        }

        private void OnTertiaryButtonClicked(object sender, EventArgs e)
        {
            CloseModal(sender, e);
            TertiaryButtonClicked?.Invoke();
        }

        private void ConstructDangerButton(string destructiveText = null, Action destructiveButtonClick = null)
        {
            DestructiveButtonText = destructiveText;
            DestructiveButtonClicked = destructiveButtonClick;

            if (!string.IsNullOrEmpty(DestructiveButtonText))
            {
                _dangerButton = new TMButton
                {
                    Title = DestructiveButtonText,
                    ButtonColor = ButtonColor.Danger,
                    HorizontalOptions = LayoutOptions.End,
                    Size = Enums.Size.Small
                };
                _dangerButton._titleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
                _dangerButton.Clicked += OnDestructiveButtonClicked;
                if(_buttonContainer.Children.Count > 0)
                {
                    foreach(TMButton tmButton in _buttonContainer.Children.Cast<TMButton>())
                    {
                        tmButton.ButtonColor = tmButton.ButtonColor == ButtonColor.Primary? ButtonColor.Secondary : ButtonColor.Tertiary;
                        tmButton.ButtonStyle = tmButton.ButtonColor == ButtonColor.Secondary ? ButtonStyle.Outline : ButtonStyle.BorderLess;
                    }
                }
                if (FullWidthButton)
                {
                    _dangerButton.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _dangerButton.frame.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _buttonContainer.Children.Insert(0, _dangerButton);
                }
                else
                {
                    _dangerButton.HorizontalOptions = LayoutOptions.Start;
                    _dangerButton.frame.HorizontalOptions = LayoutOptions.Start;
                    _buttonContainer.Children.Add(_dangerButton);
                }
                _dangerButtonAdded = true;
            }
        }

        private void OnDestructiveButtonClicked(object sender, EventArgs e)
        {
            CloseModal(sender, e);
            DestructiveButtonClicked?.Invoke();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            // Check the width and height to determine the screen orientation
            if (width > height)
            {
                // Landscape orientation
                // Perform actions or update UI for landscape orientation
                _border.WidthRequest = width * 0.5;

            }
            else
            {
                // Portrait orientation
                // Perform actions or update UI for portrait orientation
                _border.WidthRequest = width * 0.75;
            }
        }
        #endregion
    }
}