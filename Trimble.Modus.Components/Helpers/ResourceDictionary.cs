using Trimble.Modus.Components.Hosting;

namespace Trimble.Modus.Components.Helpers;

public static class ResourcesDictionary
{
    public static Color ColorsDictionary(string styleKey)
    {
        return (Color)new Styles.Colors()[styleKey];
    }
}