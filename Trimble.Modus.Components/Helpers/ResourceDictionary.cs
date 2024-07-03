namespace Trimble.Modus.Components.Helpers;

public static class ResourcesDictionary
{
    public static Color GetColor(string styleKey)
    {
        if (Application.Current.Resources.ContainsKey(styleKey))
        {
            return Application.Current.Resources[styleKey] as Color;
        }
        else
        {
            ResourceDictionary style = new Styles.LightThemeColors();
            if (Application.Current.RequestedTheme == AppTheme.Dark)
            {
                style = new Styles.DarkThemeColors();
            }
            if (style.ContainsKey(styleKey))
            {
                return style[styleKey] as Color;
            }
        }
        return Colors.Transparent;
    }
}
