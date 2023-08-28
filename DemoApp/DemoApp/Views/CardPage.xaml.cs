using System.Reflection.Metadata;
using System.Windows.Input;
using Trimble.Modus.Components;

namespace DemoApp.Views;

public partial class CardPage : ContentPage
{
    public ICommand Command => new Command(OnClickedCommand);
    public CardPage()
    {
        InitializeComponent();
        BindingContext = this;
    }


    private void OnClickedCommand(object e)
    {
        Console.WriteLine("Execute " + e);
    }

    private void IsSelectedToggled(object sender, ToggledEventArgs e)
    {
        List<TMCard> cardlist = new List<TMCard>() { card1, card2, card3, card4, card5, card6 };
        foreach (TMCard card in cardlist)
        {
            card.IsSelected = e.Value;
        }
    }

    private void Card1Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("Card1 Clicked");
    }
}
