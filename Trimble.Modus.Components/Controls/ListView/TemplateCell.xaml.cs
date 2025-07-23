namespace Trimble.Modus.Components
{
    public partial class TemplateCell : ViewCell
    {
        #region Bindable Properties
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(TemplateCell));
        public static readonly BindableProperty BackgroundColorProperty =
           BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(TextCell), Colors.White,
               propertyChanged: OnBackgroundColorChanged);

        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        public Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        #endregion

        #region Constructor
        public TemplateCell()
        {
            InitializeComponent();

            grid.BackgroundColor = BackgroundColor;
        }
        #endregion
        #region Private Methods
        private static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null && bindable is TemplateCell cell)
            {
                cell.grid.BackgroundColor = (Color)newValue;
            }
        }

        #endregion
    }
}
