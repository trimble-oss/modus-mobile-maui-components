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

        if (Application.Current?.Resources is ResourceDictionary appResources)
        {
            var preferredType = Application.Current.RequestedTheme == AppTheme.Dark
                ? typeof(Styles.DarkThemeColors)
                : typeof(Styles.LightThemeColors);

            foreach (var dictionary in appResources.MergedDictionaries)
            {
                if (dictionary.GetType() == preferredType &&
                    dictionary.TryGetValue(styleKey, out var themeResource) &&
                    themeResource is Color themeColor)
                {
                    return themeColor;
                }
            }

            foreach (var dictionary in appResources.MergedDictionaries)
            {
                if (dictionary.TryGetValue(styleKey, out var mergedResource) &&
                    mergedResource is Color mergedColor)
                {
                    return mergedColor;
                }
            }
        }

        return Colors.Transparent;
    }
}
