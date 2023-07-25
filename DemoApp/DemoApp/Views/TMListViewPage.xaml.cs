using Trimble.Modus.Components;
using DemoApp.ViewModels;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

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
