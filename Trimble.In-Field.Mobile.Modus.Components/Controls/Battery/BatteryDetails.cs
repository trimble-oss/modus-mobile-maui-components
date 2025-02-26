namespace Trimble.Modus.Components.Mobile.InField;

public class BatteryDetails
{
    public string Source { get; set; } = string.Empty;
    public int Level { get; set; }
    public bool IsCharging { get; set; }
}
