
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using Trimble.Modus.Components.Contants;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components
{

    public class TMModal : Popup.Pages.PopupPage
    {
        private Border _border;

        private Grid _baseContainer;

        private Image _titleIcon;

        private ImageButton _closeButton;

        private Label _titleLabel;

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
            BindableProperty.Create(nameof(TitleIcon), typeof(ImageSource), typeof(TMModal), null, BindingMode.TwoWay);

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

        #endregion
        public TMModal()
        {
            _baseContainer = new Grid();
            _baseContainer.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Auto},
                new ColumnDefinition { Width = GridLength.Auto},
                new ColumnDefinition { Width = GridLength.Auto}
            };
            _baseContainer.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto },
            };

            _titleLabel = new Label { Text = string.Empty, HorizontalOptions = LayoutOptions.Start, FontFamily = "OpenSans-Semibold.ttf", FontSize = 16 };
            _baseContainer.Children.Add(_titleLabel);
            _baseContainer.SetColumn(_titleLabel, 1);
            _baseContainer.SetRow(_titleLabel, 0);

            _titleIcon = new Image();
            _baseContainer.Children.Add( _titleIcon );
            _baseContainer.SetColumn(_titleIcon, 0);
            _baseContainer.SetRow(_titleIcon, 0);

            _closeButton = new ImageButton { Source = ImageSource.FromResource(ImageConstants.CloseButtonImage), HeightRequest = 24, WidthRequest = 24 };
            _baseContainer.Children.Add( _closeButton );
            _baseContainer.SetColumn(_closeButton, 2);
            _baseContainer.SetRow(_closeButton, 0);

            double screenWidth = DeviceDisplay.MainDisplayInfo.Width;
            double desiredWidth = screenWidth * 0.5;

            // Center the grid horizontally using layout options
            AbsoluteLayout.SetLayoutFlags(_baseContainer, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(_baseContainer, new Rect(0.5, 0.5, desiredWidth, AbsoluteLayout.AutoSize));

            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            absoluteLayout.Children.Add(_baseContainer);


            _border = new Border
            {
                Padding = new Thickness(16),
                Content = absoluteLayout,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,
                StrokeShape = new Rectangle
                {
                    RadiusX = 4,
                    RadiusY = 4
                },
                Stroke = (Color)BaseComponent.colorsDictionary()["Black"],
            };

            BackgroundColor = Colors.Transparent;
            Content = _border;
            _closeButton.Clicked += CloseModal;
            SetBinding();
        }

        #region Private methods
        private void SetBinding()
        {
            _titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(TitleText), BindingMode.TwoWay, source: this));
            _titleIcon.SetBinding(Image.SourceProperty, new Binding(nameof(TitleIcon), BindingMode.TwoWay, source: this));
        }

        private void CloseModal(object sender, EventArgs e)
        {
            PopupService.Instance.RemovePageAsync(this);
        }

        #endregion
    }
}