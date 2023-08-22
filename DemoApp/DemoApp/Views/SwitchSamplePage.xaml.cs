using DemoApp.ViewModels;
using System.Windows.Input;
using Trimble.Modus.Components;

namespace DemoApp.Views;

public partial class SwitchSamplePage : ContentPage
{
    public SwitchSamplePage()
	{
		InitializeComponent();
        BindingContext = new SwitchViewModel();
	}
    private void TMSwitch_Toggled(object sender, TMSwitchEventArgs e)
    {
        Console.WriteLine("Toggled "+e.Value.ToString());    
    }
}
