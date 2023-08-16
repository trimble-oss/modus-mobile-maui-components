namespace Trimble.Modus.Components;
public class TabTappedEventArgs : EventArgs
{
    public TabTappedEventArgs(int position) => Position = position;

    public int Position { get; set; }
}
