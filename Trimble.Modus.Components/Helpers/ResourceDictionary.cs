using Trimble.Modus.Components.Hosting;

namespace Trimble.Modus.Components.Helpers;

public static class ResourcesDictionary
{
    public static Color ColorsDictionary(string styleKey)
    {
        return (Color)new Styles.Colors()[styleKey];
    }

    public static Color ColorsBasedOnTheme(string styleKey)
    {
        if (AppBuilderExtensions.CurrentRequestedTheme == AppTheme.Dark)
        {
            return (Color)new Styles.DarkTheme()[styleKey];
        }
        else
        {
            return (Color)new Styles.LightTheme()[styleKey];
        }
    }
}
