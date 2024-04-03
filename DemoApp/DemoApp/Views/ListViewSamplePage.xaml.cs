using Trimble.Modus.Components;
using DemoApp.ViewModels;

namespace DemoApp.Views
{
    public partial class ListViewSamplePage : ContentPage
    {
        #region Private Fields
        private ListViewSamplePageViewModel tmListViewPageViewModel;
        #endregion
        #region Constructor
        public ListViewSamplePage()
        {
            InitializeComponent();
            tmListViewPageViewModel = new ListViewSamplePageViewModel();
            textCellList.ItemTemplate = TextCell;
            BindingContext = tmListViewPageViewModel;
        }
        #endregion
        #region Private Methods
        private void OnCellGroupButtonChanged(object sender, TMRadioButtonEventArgs e)
        {
            if (e.RadioButtonIndex == 0)
            {
                textCellList.ItemTemplate = TextCell;
            }
            else
            {
                textCellList.ItemTemplate = ViewCell;
            }
        }
        #endregion
    }
}
