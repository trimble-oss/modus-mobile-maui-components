namespace DemoApp.Views;

public partial class BadgeSamplePage : ContentPage
{
    public BadgeSamplePageViewModel ViewModel = new();
	public BadgeSamplePage()
	{
		InitializeComponent();
        BindingContext = ViewModel;
	}
}
