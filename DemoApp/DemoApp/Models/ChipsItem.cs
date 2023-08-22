using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Models;

public class ChipsItem
{
    public string Title { get; set; }
    public string LeftIconSource { get; set; }

    public ChipsItem(string title, string iconSource)
    {
        Title = title;
        LeftIconSource = iconSource;
    }
}
