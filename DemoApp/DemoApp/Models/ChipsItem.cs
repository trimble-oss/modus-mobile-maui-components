using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace DemoApp.Models;
public class ChipsItem
{
    public string Title { get; set; }
    public ChipType Type { get; set; }
    public ChipSize ChipSize { get; set; }
    public ICommand ClickChipCommand { get; set; }

    public string LeftIconSource { get; set; }
    public ICommand CloseChipCommand { get; set; }

    public ChipsItem(string title, string iconSource, ICommand clickChipCommand, ICommand closeChipCommand)
    {
        Title = title;
        ClickChipCommand = clickChipCommand;
        LeftIconSource = iconSource;
        CloseChipCommand = closeChipCommand;
    }
}
