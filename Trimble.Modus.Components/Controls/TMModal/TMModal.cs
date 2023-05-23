
using Microsoft.Maui.Controls;
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
        private Border _border;

        private Grid _baseContainer;

        private StackLayout _buttonContainer;

        private Image _titleIcon = new Image { WidthRequest = 0, HeightRequest = 26 };

        private ImageButton _closeButton;

        private Label _titleLabel = new Label { HorizontalOptions = LayoutOptions.Start, FontFamily = "OpenSans-Semibold.ttf", FontSize = 16, Padding = new Thickness(0, 0, 8, 0) };

        private Label _descriptionLabel;

        private TMButton _primaryButton;

        private TMButton _secondaryButton;

        private TMButton _tertiaryButton;

        private TMButton _destructiveButton;

        #region Bindable Properties

        /// <summary>
        /// Gets or sets the text for the title label in the control
        /// </summary>
        public static readonly BindableProperty TitleTextProperty =
            BindableProperty.Create(nameof(TitleText), typeof(string), typeof(TMModal), null, BindingMode.TwoWay);

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

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets title text
        /// </summary>
        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
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

        /// <summary>
        /// Action triggered when primary button is clicked
        /// </summary>
        public event EventHandler PrimaryButtonClicked;

        /// <summary>
        /// Action triggered when secondary button is clicked
        /// </summary>
        public event EventHandler SecondaryButtonClicked;

        /// <summary>
        /// Action triggered when Tertiary button is clicked
        /// </summary>
        public event EventHandler TertiaryButtonClicked;

        /// <summary>
        /// Action triggered when Destructive button is clicked
        /// </summary>
        public event EventHandler DestructiveButtonClicked;

        public TMModalBuilder TMModalBuilder;

        #endregion
        public TMModal( string titleText, 
                        ImageSource titleIconSource = null, 
                        string primaryText = null, 
                        string secondaryText = null, 
                        string tertiaryText = null,
                        string destructiveText = null,
                        EventHandler primaryButtonClick = null,
                        EventHandler secondaryButtonClick = null,
                        EventHandler tertiaryButtonClick = null,
                        EventHandler destructiveButtonClick = null
            ){
            TitleText = titleText;
            TitleIcon = titleIconSource;
            PrimaryText = primaryText;
            SecondaryText = secondaryText;
            TertiaryText = tertiaryText;
            DestructiveButtonText = destructiveText;
            PrimaryButtonClicked = primaryButtonClick;
            SecondaryButtonClicked = secondaryButtonClick;
            TertiaryButtonClicked = tertiaryButtonClick;
            DestructiveButtonClicked = destructiveButtonClick;

            ConfigureModal();

            ConstructDestructiveButton();
            ConstructPrimaryButton();
            ConstructSecondaryButton();
            ConstructTertiaryButton();

            _buttonContainer = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.End, Spacing = 8 };
            _buttonContainer.Children.Add(_tertiaryButton);
            _buttonContainer.Children.Add(_secondaryButton);
            _buttonContainer.Children.Add(_primaryButton);
            _buttonContainer.Children.Add(_destructiveButton);

            _baseContainer.Children.Add(_buttonContainer);
            _baseContainer.SetColumn(_buttonContainer, 0);
            _baseContainer.SetColumnSpan(_buttonContainer, 3);
            _baseContainer.SetRow(_buttonContainer, 2);



            double screenWidth = DeviceDisplay.MainDisplayInfo.Width;
            double desiredWidth = screenWidth * 0.3;

            Shadow shadow = new Shadow { Brush = Brush.Black, Radius = 20, Opacity = 0.6F };
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
                Shadow = shadow
            };
            //_border.WidthRequest = desiredWidth;

            SetBinding();
            Animation = new ScaleAnimation();
            Content = _border;
        }

        #region Private methods
        private void SetBinding()
        {
            _titleLabel?.SetBinding(Label.TextProperty, new Binding(nameof(TitleText), BindingMode.TwoWay, source: this));
            _titleIcon?.SetBinding(Image.SourceProperty, new Binding(nameof(TitleIcon), BindingMode.TwoWay, source: this));

            _primaryButton?.SetBinding(TMButton.TitleProperty, new Binding(nameof(PrimaryText), BindingMode.TwoWay, source: this));
            _secondaryButton?.SetBinding(TMButton.TitleProperty, new Binding(nameof(SecondaryText), BindingMode.TwoWay, source: this));
            _tertiaryButton?.SetBinding(TMButton.TitleProperty, new Binding(nameof(TertiaryText), BindingMode.TwoWay, source: this));
            _destructiveButton?.SetBinding(TMButton.TitleProperty, new Binding(nameof(DestructiveButtonText), BindingMode.TwoWay, source: this));
        }

        private void CloseModal(object sender, EventArgs e)
        {
            PopupService.Instance.RemovePageAsync(this);
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

            _baseContainer = new Grid();
            _baseContainer.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto },
            };
            _baseContainer.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto },
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

            _descriptionLabel = new Label
            {
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean mattis, " +
                        "mauris at dictum luctus, sem sapien malesuada nisi",
                LineBreakMode = LineBreakMode.WordWrap
            };
            _baseContainer.Children.Add(_descriptionLabel);
            _baseContainer.SetColumn(_descriptionLabel, 0);
            _baseContainer.SetColumnSpan(_descriptionLabel, 3);
            _baseContainer.SetRow(_descriptionLabel, 1);

            _baseContainer.RowSpacing = 16;

            BackgroundColor = Color.FromArgb("#80000000");
            _closeButton.Clicked += CloseModal;
        }

        private void ConstructPrimaryButton()
        {
            if (!string.IsNullOrEmpty(PrimaryText))
            {
                _primaryButton = new TMButton();
                _primaryButton.HorizontalOptions = LayoutOptions.End;
                _primaryButton.Size = Enums.Size.Small;
                _primaryButton.Clicked += OnPrimaryButtonClicked;
            }
        }

        private void OnPrimaryButtonClicked(object sender, EventArgs e)
        {
            PrimaryButtonClicked?.Invoke(this, e);
            CloseModal(sender, e);
        }

        private void ConstructSecondaryButton()
        {
            if (!string.IsNullOrEmpty(SecondaryText))
            {
                _secondaryButton = new TMButton();
                _secondaryButton.BackgroundColor = Colors.White;
                _secondaryButton.TextColor = Colors.Black;
                _secondaryButton.HorizontalOptions = LayoutOptions.End;
                _secondaryButton.Size = Enums.Size.Small;
                _secondaryButton.Clicked += OnSecondaryButtonClicked;
            }
        }

        private void OnSecondaryButtonClicked(object sender, EventArgs e)
        {
            SecondaryButtonClicked?.Invoke(this, e);
            CloseModal(sender, e);
        }

        private void ConstructTertiaryButton()
        {
            if (!string.IsNullOrEmpty(TertiaryText))
            {
                _tertiaryButton = new TMButton();
                _tertiaryButton.BackgroundColor = Colors.White;
                _tertiaryButton.TextColor = (Color)BaseComponent.colorsDictionary()["TrimbleBlue"];
                _tertiaryButton.BorderColor = Colors.Transparent;
                _tertiaryButton.HorizontalOptions = LayoutOptions.End;
                _tertiaryButton.Size = Enums.Size.Small;
                _tertiaryButton.Clicked += OnTertiaryButtonClicked;
            }
        }

        private void OnTertiaryButtonClicked(object sender, EventArgs e)
        {
            TertiaryButtonClicked?.Invoke(this, e);
            CloseModal(sender, e);
        }

        private void ConstructDestructiveButton()
        {
            if (!string.IsNullOrEmpty(DestructiveButtonText))
            {
                _destructiveButton = new TMButton();
                _destructiveButton.BackgroundColor = (Color)BaseComponent.colorsDictionary()["TrimbleButtonRed"];
                _destructiveButton.TextColor = Colors.White;
                _destructiveButton.BorderColor = Colors.Transparent;
                _destructiveButton.HorizontalOptions = LayoutOptions.End;
                _destructiveButton.Size = Enums.Size.Small;
                _destructiveButton.Clicked += OnDestructiveButtonClicked;
            }
        }

        private void OnDestructiveButtonClicked(object sender, EventArgs e)
        {
            DestructiveButtonClicked?.Invoke(this, e);
            CloseModal(sender, e);
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