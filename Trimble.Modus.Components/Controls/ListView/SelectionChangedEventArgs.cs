namespace Trimble.Modus.Components;
public class SelectionChangedEventArgs : EventArgs
{
    public object PreviousSelection { get; set; }
    public object CurrentSelection { get; set; }
    public int SelectedIndex { get; set; }
}

