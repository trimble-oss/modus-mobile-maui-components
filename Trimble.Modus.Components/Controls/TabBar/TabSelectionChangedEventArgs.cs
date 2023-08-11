namespace Trimble.Modus.Components;
public class TabSelectionChangedEventArgs : EventArgs
{
    public int NewPosition { get; set; }

    public int OldPosition { get; set; }
}
