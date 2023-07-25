using DemoApp.Resources;
using Trimble.Modus.Components;
using DemoApp.ViewModels;
using System.Windows.Input;

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
            tmListViewPageViewModel = new TMListViewPageViewModel(TextCell,ViewCell);
            BindingContext = tmListViewPageViewModel;
        }
        #endregion
    }
}
