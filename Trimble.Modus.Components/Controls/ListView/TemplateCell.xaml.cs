namespace Trimble.Modus.Components
{
    public partial class TemplateCell : ViewCell
    {
        #region Bindable Properties
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(TemplateCell));

        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        #endregion

        #region Constructor
        public TemplateCell()
        {
            InitializeComponent();
        }
        #endregion
        #region Internal Methods

        internal void UpdateBackgroundColor(Color backgroundColor)
        {
            grid.BackgroundColor = backgroundColor;
        }
    }
    #endregion
}
