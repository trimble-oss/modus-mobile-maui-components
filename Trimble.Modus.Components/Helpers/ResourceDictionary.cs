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
            return DarkThemeColor(styleKey);
        }
        else
        {
            return LightThemeColor(styleKey);
        }
    }

    public static Color DarkThemeColor(string styleKey)
    {
        return (Color)new Styles.DarkTheme()[styleKey];
    }

    public static Color LightThemeColor(string styleKey)
    {
        return (Color)new Styles.LightTheme()[styleKey];
    }


    public static Color ColorsFromResources(string key)
    {
        // Retrieve the Primary color value which is in the page's resource dictionary
        var hasValue = Application.Current.Resources.TryGetValue(key, out object primaryColor);

        if (hasValue)
        {
            return (Color)primaryColor;
        }

        return Colors.White;
    }

    public static Style? StylesFromResources(string key)
    {
        // Retrieve the Primary color value which is in the page's resource dictionary
        var hasValue = Application.Current.Resources.TryGetValue(key, out object primaryColor);

        if (hasValue)
        {
            return (Style)primaryColor;
        }
        return null;
    }
}
