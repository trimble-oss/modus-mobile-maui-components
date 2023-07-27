namespace Trimble.Modus.Components;
public class SelectionChangedEventArgs : EventArgs
{
    public IReadOnlyList<object> PreviousSelection { get; set; }
    public IReadOnlyList<object> CurrentSelection { get; set; }
    public int SelectedIndex { get; set; }
}

