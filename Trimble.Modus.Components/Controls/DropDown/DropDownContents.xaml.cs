using CommunityToolkit.Mvvm.DependencyInjection;
using System.Collections;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Animations;

namespace Trimble.Modus.Components;

public partial class DropDownContents : PopupPage
{
    public DropDownContents(View anchorView, Enums.ModalPosition position, Thickness margin, double desiredHeight, double widthRequest, EventHandler<SelectedItemChangedEventArgs> eventHandler, int selectedIndex, double Y, double height) : base(anchorView, position)
    {
        InitializeComponent();
        Animation = new RevealAnimation(desiredHeight);
        border.Margin = margin;
        border.HeightRequest = desiredHeight;
        border.WidthRequest = widthRequest;
        listView.ItemSelected += eventHandler;
        if (height - Y < desiredHeight)
        {
            this.Position = Enums.ModalPosition.Top;
            border.Margin = new Thickness(0, -16, 0, 0);
#if WINDOWS
            border.Margin = new Thickness(0, 24, 10, 0);
#endif
        }
        //listView.SelectedItem = value?.Cast<object>()?.ToList()[selectedIndex];
    }

    public IEnumerable ItemSource
    {
        set
        {
            listView.ItemsSource = value;
        }
    }
}
