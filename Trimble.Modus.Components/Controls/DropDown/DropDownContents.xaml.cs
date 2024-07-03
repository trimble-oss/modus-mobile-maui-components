using System.Collections;
using Trimble.Modus.Components.Popup.Animations;

namespace Trimble.Modus.Components;

public partial class DropDownContents : PopupPage
{
    internal DropDownContents()
    {

    }

    internal DropDownContents(View anchorView, Enums.ModalPosition position) : base(anchorView, position)
    {
        InitializeComponent();
    }

    public Thickness Margin { get; set; }
    public IEnumerable ItemSource { get; set; }
    public double DesiredHeight { get; set; }
    public EventHandler<SelectedItemChangedEventArgs> SelectedEventHandler { get; set; }
    public int SelectedIndex { get; set; }
    public double YPosition { get; set; }
    public new double Height { get; set; }

    public void Build()
    {
        Animation = new RevealAnimation(DesiredHeight);
        border.Margin = Margin;
        border.HeightRequest = DesiredHeight;
        border.WidthRequest = WidthRequest;
        listView.ItemsSource = ItemSource;
        if (SelectedIndex < 0)
        {
            listView.SelectedItem = null;
        }
        else
        {
            listView.SelectedItem = ItemSource?.Cast<object>()?.ToList()[SelectedIndex];
        }
        listView.ItemSelected += SelectedEventHandler;
        if (Height - YPosition < DesiredHeight)
        {
            this.Position = Enums.ModalPosition.Top;
            border.Margin = new Thickness(0, -16, 0, 0);
#if WINDOWS
            border.Margin = new Thickness(0, 24, 10, 0);
#endif
        }
    }
}
