using Microsoft.Maui.Controls.Shapes;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Contant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Popup.Animations;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components
{

    public class TMModal : Popup.Pages.PopupPage
    {
        private readonly Border _border;

        private Grid _baseContainer;

        private readonly StackLayout _buttonContainer;

        private readonly Image _titleIcon = new() { WidthRequest = 0, HeightRequest = 26, VerticalOptions = LayoutOptions.Center };

        private Image _closeButton;

        private readonly Label _titleLabel = new() { HorizontalOptions = LayoutOptions.Start, FontFamily = "OpenSans-Semibold.ttf", FontSize = 16, Padding = new Thickness(0, 0, 8, 0), VerticalOptions = LayoutOptions.Center };

        private StackLayout _modalBodyContainer;

        private Label _descriptionLabel = new() { LineBreakMode = LineBreakMode.WordWrap, VerticalOptions = LayoutOptions.StartAndExpand };

        private TMButton _primaryButton;

        private TMButton _secondaryButton;

        private TMButton _tertiaryButton;

        private TMButton _dangerButton;

        private bool _dangerButtonAdded = false;

        private readonly TapGestureRecognizer _closeButtonTapRecognizer = new();

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
        /// Gets or sets the button width option
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

        #region Property Change handler

        /// <summary>
        /// Triggered when FullWidthButton option is changed
        /// </summary>
        /// <param name="bindable">Object</param>
        /// <param name="oldValue">Old bool value</param>
        /// <param name="newValue">New bool value</param>
        private static void OnFullWidthButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var modal = (TMModal)bindable;

            if (modal._buttonContainer != null)
            {
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

                if ((bool)oldValue != (bool)newValue)
                {
                    modal.ReverseButtonOrder(modal._buttonContainer);
                }
            }

        }

        /// <summary>
        /// Handles when message text is changed to update the size of the modal
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var modal = (TMModal)bindable;
            var text = (string)newValue;
            if (!string.IsNullOrEmpty(text))
            {
                if (modal._modalBodyContainer.Children.Count > 0 && modal._modalBodyContainer.Children[0] is Label)
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

        /// <summary>
        /// Handle icon source change
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnTitleIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TMModal tMModal = bindable as TMModal;
            if (tMModal._titleIcon != null && tMModal._titleLabel != null)
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

        #endregion

        #region Public Methods
        public TMModal(string titleText, string messageText = null, ImageSource titleIconSource = null, bool fullWidthButton = false)
        {
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
                Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstant.Black),
                Shadow = shadow,
                StrokeThickness = 0,
                BackgroundColor = Colors.White
            };

            SetBinding();
            Animation = new ScaleAnimation();
            BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstant.ModalGraySemiTransparent);

            Content = _border;
        }

        /// <summary>
        /// Add Primary button
        /// </summary>
        /// <param name="title">Button titke</param>
        /// <param name="clickAction">Button click action</param>
        /// <exception cref="ArgumentNullException">Thrown when title is empty</exception>
        /// <exception cref="InvalidOperationException">Thrown while trying to add more that 3 button</exception>
        public void AddPrimaryAction(string title, Action clickAction = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(Constants.ButtonEmptyTitleError);
            }
            if (_buttonContainer.Children.Count >= 3)
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
        public void AddSecondaryAction(string title, Action clickAction = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(Constants.ButtonEmptyTitleError);
            }
            if (_buttonContainer.Children.Count >= 3)
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
        public void AddTertiaryAction(string title, Action clickAction = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(Constants.ButtonEmptyTitleError);
            }
            if (_buttonContainer.Children.Count >= 3)
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
        public void AddDangerAction(string title, Action clickAction = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(Constants.ButtonEmptyTitleError);
            }
            if (_buttonContainer.Children.Count >= 3)
            {
                throw new InvalidOperationException(Constants.ButtonLimitError);
            }
            if (_dangerButtonAdded)
            {
                _dangerButton.Title = title;
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
        public void AddTextInput(Action<TMInput> inputConfigurationHandler = null)
        {
            var inputControl = new TMInput();

            inputConfigurationHandler?.Invoke(inputControl);

            _modalBodyContainer.Add(inputControl);
        }

        /// <summary>
        /// Display the modal
        /// </summary>
        public void Show()
        {
            PopupService.Instance.PushAsync(this);
        }

        /// <summary>
        /// Close modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseModal(object sender, EventArgs e)
        {
            PopupService.Instance.RemovePageAsync(this, true);
        }

        #endregion

        #region Private methods
        private void SetBinding()
        {
            _titleLabel?.SetBinding(Label.TextProperty, new Binding(nameof(Title), BindingMode.TwoWay, source: this));
            _titleIcon?.SetBinding(Image.SourceProperty, new Binding(nameof(TitleIcon), BindingMode.TwoWay, source: this));
        }

        /// <summary>
        /// Set up the initial UI for modal
        /// </summary>
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

            _closeButton = new Image { Source = ImageSource.FromFile(ImageConstants.CloseButtonImage), HeightRequest = 16, WidthRequest = 16, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(2) };
            _baseContainer.Children.Add(_closeButton);
            _baseContainer.SetColumn(_closeButton, 2);
            _baseContainer.SetRow(_closeButton, 0);


            _modalBodyContainer = new StackLayout { Parent = _baseContainer, Orientation = StackOrientation.Vertical, Spacing = 16 };
            _baseContainer.SetColumn(_modalBodyContainer, 0);
            _baseContainer.SetColumnSpan(_modalBodyContainer, 3);
            _baseContainer.SetRow(_modalBodyContainer, 1);

            _baseContainer.Children.Add(_modalBodyContainer);

            _baseContainer.RowSpacing = 16;

            _closeButtonTapRecognizer.Tapped += CloseModal;
            _closeButton.GestureRecognizers.Add(_closeButtonTapRecognizer);
        }

        /// <summary>
        /// Constructs promary button
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
                    Title = primaryText,
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
                    Title = secondaryText,
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
                    Title = tertiaryText,
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
                    Title = destructiveText,
                    ButtonColor = ButtonColor.Danger,
                    HorizontalOptions = LayoutOptions.End,
                    Size = Enums.Size.Small
                };
                _dangerButton._titleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
                _dangerButton.Clicked += OnDestructiveButtonClicked;

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
                _border.WidthRequest = width * 0.5;

            }
            else
            {
                _border.WidthRequest = width * 0.75;
            }
        }
        #endregion
    }
}
