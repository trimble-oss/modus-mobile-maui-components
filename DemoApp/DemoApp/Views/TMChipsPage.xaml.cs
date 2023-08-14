using CommunityToolkit.Mvvm.Input;
using DemoApp.Constant;
using DemoApp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Views;

public partial class TMChipsPage : ContentPage
{
    public TMChipsPage()
    {
        InitializeComponent();
        BindingContext = new TMChipsPageViewModel();
    }
}
