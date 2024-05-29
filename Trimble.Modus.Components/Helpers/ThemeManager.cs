namespace Trimble.Modus.Components.Helpers
{
    public class ThemeManager
    {
        private static ResourceDictionary LightThemeColorResourceDictionary { get; set; }
        private static ResourceDictionary DarkThemeColorResourceDictionary { get; set; }
        internal static AppTheme CurrentTheme { get; private set; }

        public static void Initialize(ResourceDictionary lightThemeColors = null, ResourceDictionary darkThemeColors = null)
        {
            var defaultLightThemeColors = new Styles.LightThemeColors();
            var defaultDarkThemeColors = new Styles.DarkThemeColors();
            IncludeFromResources(new Styles.Colors());
            if (lightThemeColors != null && lightThemeColors.Count > 0)
            {
                LightThemeColorResourceDictionary = UpdateDefaulThemeWithCustomTheme(defaultLightThemeColors, lightThemeColors);
            }
            else
            {
                LightThemeColorResourceDictionary = defaultLightThemeColors;
            }

            if (darkThemeColors != null && darkThemeColors.Count > 0)
            {
                DarkThemeColorResourceDictionary = UpdateDefaulThemeWithCustomTheme(defaultDarkThemeColors, darkThemeColors);
            }
            else
            {
                if (lightThemeColors != null && lightThemeColors.Count > 0)
                {
                    DarkThemeColorResourceDictionary = LightThemeColorResourceDictionary;
                }
                else
                {
                    DarkThemeColorResourceDictionary = defaultDarkThemeColors;
                }
            }

            UpdateTheme(Application.Current.RequestedTheme);

            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
        }


        private static ResourceDictionary UpdateDefaulThemeWithCustomTheme(ResourceDictionary defaultTheme, ResourceDictionary customTheme)
        {
            var theme = customTheme;
            var notExistsInDictionary = defaultTheme.Where(x => !customTheme.ContainsKey(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            foreach (var item in notExistsInDictionary)
            {
                var keyExists = theme.ContainsKey(item.Key);
                if (keyExists)
                {
                    theme[item.Key] = item.Value;
                }
                else
                {
                    theme.Add(item.Key, item.Value);
                }
            }
            return theme;
        }

        private static void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            UpdateTheme(e.RequestedTheme);
        }

        public static void UpdateTheme(AppTheme theme = AppTheme.Light)
        {
            CurrentTheme = Application.Current.RequestedTheme;

            RemoveFromResources(LightThemeColorResourceDictionary);
            RemoveFromResources(DarkThemeColorResourceDictionary);
            if (theme == AppTheme.Dark)
            {
                IncludeFromResources(DarkThemeColorResourceDictionary);
                IncludeFromResources(new Styles.DarkTheme());
            }
            else
            {
                IncludeFromResources(LightThemeColorResourceDictionary);
                IncludeFromResources(new Styles.LightTheme());
            }
        }

        /// <summary>
        /// Includes the given resource dictionary with the Application.Current.Resources
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <param name="needToAdd"></param>
        public static void IncludeFromResources(ResourceDictionary keyValuePairs)
        {
            var dictionaryExists = Application.Current.Resources.MergedDictionaries.Contains(keyValuePairs);
            if (!dictionaryExists)
            {
                Application.Current.Resources.MergedDictionaries.Add(keyValuePairs);
            }
        }

        /// <summary>
        /// Removes the given resource dictionary from the Application.Current.Resources
        /// </summary>
        /// <param name="keyValuePairs"></param>
        public static void RemoveFromResources(ResourceDictionary keyValuePairs)
        {
            var dictionaryExists = Application.Current.Resources.MergedDictionaries.Contains(keyValuePairs);
            if (dictionaryExists)
            {
                Application.Current.Resources.MergedDictionaries.Remove(keyValuePairs);
            }
        }
    }
}

