namespace DemoApp.Views;

public partial class TMBadgesPage : ContentPage
{
    public TMBadgesPageViewModel ViewModel = new();
	public TMBadgesPage()
	{
		InitializeComponent();
        BindingContext = ViewModel;
	}
}
