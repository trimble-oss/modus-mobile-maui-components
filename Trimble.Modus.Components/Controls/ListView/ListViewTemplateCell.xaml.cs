using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components
{
    public partial class ListViewTemplateCell : ViewCell
    {
        #region Private Fields
        private ListView _parent = null;
        #endregion

        #region Bindable Properties
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(ListViewTemplateCell));

        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        #endregion

        #region Constructor
        public ListViewTemplateCell()
        {
            InitializeComponent();
        }
        #endregion

        #region Protected Methods
        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (this.Parent != null)
            {
                _parent = Parent as ListView;
                var test = this.Parent;
                ((ListView)test).ItemTapped += CellItemSelected;
            }
            else
            {
                _parent.ItemTapped -= CellItemSelected;
                _parent = null;
            }
        }
        #endregion

        #region Private Methods
        private void CellItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView listView)
            {
                var test = listView.Parent.Parent;
                if (test is TMListView tMListView)
                {
                    grid.BackgroundColor = Colors.White;
                    foreach (var item in tMListView.selectableItems)
                    {
                        if (item == BindingContext)
                        {
                            grid.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
