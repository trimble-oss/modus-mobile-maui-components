using Trimble.Modus.Components;
using DemoApp.ViewModels;

namespace DemoApp.Views
{
    public partial class ListViewPage : ContentPage
    {
        #region Private Fields
        private TMListViewSamplePageViewModel tmListViewPageViewModel;
        #endregion
        #region Constructor
        public ListViewPage()
        {
            InitializeComponent();
            tmListViewPageViewModel = new TMListViewSamplePageViewModel();
            textCellList.ItemTemplate = TextCell;
            BindingContext = tmListViewPageViewModel;
        }
        #endregion
        #region Private Methods
        private void OnCellGroupButtonChanged(object sender, TMRadioButtonEventArgs e)
        {
            textCellList.ItemTemplate = e.RadioButtonIndex switch
            {
                1 => ViewCell,
                _ => TextCell
            };
        }
        #endregion
    }
}
