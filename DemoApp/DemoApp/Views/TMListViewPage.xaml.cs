using Trimble.Modus.Components;
using DemoApp.ViewModels;

namespace DemoApp.Views
{
    public partial class TMListViewPage : ContentPage
    {
        #region Private Fields
        private TMListViewPageViewModel tmListViewPageViewModel;
        #endregion
        #region Constructor
        public TMListViewPage()
        {
            InitializeComponent();
            tmListViewPageViewModel = new TMListViewPageViewModel();
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
