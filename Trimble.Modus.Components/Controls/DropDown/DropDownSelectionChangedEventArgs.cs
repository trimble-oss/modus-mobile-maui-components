using CommunityToolkit.Mvvm.Collections;

namespace Trimble.Modus.Components;
public class DropDownSelectionChangedEventArgs : EventArgs
{
    public object PreviousSelection { get; set; }
    public object CurrentSelection { get; set; }
    public int SelectedIndex { get; set; }
}

