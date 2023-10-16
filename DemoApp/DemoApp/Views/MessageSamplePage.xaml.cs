using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class MessageSamplePage : ContentPage
{
    private MessageSamplePageViewModel ViewModel = new();
    public MessageSamplePage()
    {
        InitializeComponent();
        BindingContext = ViewModel;
    }
}

