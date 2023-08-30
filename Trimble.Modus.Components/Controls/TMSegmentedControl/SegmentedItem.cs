namespace Trimble.Modus.Components;

public class SegmentedItem
{
    public string Text { get; set; }
    public ImageSource IconSource { get; set; }
    public SegmentedItem(string text, ImageSource iconSource)
    {
        Text = text;
        IconSource = iconSource;
    }
}
