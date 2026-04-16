namespace Trimble.Modus.Components.Helpers;

public static class ResourcesDictionary
{
    public static Color GetColor(string styleKey)
    {
        if (Application.Current?.Resources is ResourceDictionary resources &&
            resources.TryGetValue(styleKey, out var applicationResource) &&
            applicationResource is Color applicationColor)
        {
            return applicationColor;
        }

        ResourceDictionary style = new Styles.LightThemeColors();
        if (Application.Current?.RequestedTheme == AppTheme.Dark)
        {
            style = new Styles.DarkThemeColors();
        }

        try
        {
            return style[styleKey] as Color ?? Colors.Transparent;
        }
        catch (KeyNotFoundException)
        {
            return Colors.Transparent;
        }
    }
}
