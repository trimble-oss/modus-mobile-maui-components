using DemoApp.InFieldViewModel;

namespace DemoApp.InFieldViews;

public partial class BatterySamplePage : ContentPage
{
	public BatterySamplePage()
	{
		InitializeComponent();
        BindingContext = new BatterySamplePageViewModel();
    }
}
