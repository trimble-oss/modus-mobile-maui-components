using Trimble.Modus.Components.Hosting;

namespace Trimble.Modus.Components.Helpers
{
    public class ThemeManager
    {
        private static ResourceDictionary LightThemeColorResourceDictionary { get; set; }
        private static ResourceDictionary DarkThemeColorResourceDictionary { get; set; }
        private static ResourceDictionary DarkThemeStylingResourceDictionary { get; set; }
        private static ResourceDictionary LightThemeStylingResourceDictionary { get; set; }
        internal static AppTheme CurrentTheme { get; private set; }

        public static void Initialize(AppConfig appConfig = null)
        {
            var defaultLightThemeColors = new Styles.LightThemeColors();
            var defaultDarkThemeColors = new Styles.DarkThemeColors();

            IncludeFromResources(new Styles.Colors());
      
            LightThemeColorResourceDictionary = UpdateDictionary(appConfig.LightThemeColors, defaultLightThemeColors);
            DarkThemeColorResourceDictionary = UpdateDictionary(appConfig.DarkThemeColors, defaultDarkThemeColors);
            LightThemeStylingResourceDictionary = appConfig.LightThemeStyles;
            DarkThemeStylingResourceDictionary = appConfig.DarkThemeStyles;

            UpdateTheme(Application.Current.RequestedTheme);

            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
        }

        private static ResourceDictionary UpdateDictionary(ResourceDictionary keyValuePairs, ResourceDictionary defaultKeyValuePairs)
        {
            if (keyValuePairs != null && keyValuePairs.Count > 0)
            {
                return UpdateDefaulThemeWithCustomTheme(defaultKeyValuePairs, keyValuePairs);
            }
            else
            {
                return defaultKeyValuePairs;
            }
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

        private static void UpdateTheme(AppTheme theme = AppTheme.Light)
        {
            CurrentTheme = Application.Current.RequestedTheme;

            if (theme == AppTheme.Dark)
            {
                IncludeFromResources(DarkThemeColorResourceDictionary);
                IncludeFromResources(new Styles.DarkThemeStyling());
                IncludeFromResources(DarkThemeStylingResourceDictionary);
            }
            else
            {
                IncludeFromResources(LightThemeColorResourceDictionary);
                IncludeFromResources(new Styles.LightThemeStyling());
                IncludeFromResources(LightThemeStylingResourceDictionary);
            }
        }

        /// <summary>
        /// Includes the given resource dictionary with the Application.Current.Resources
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <param name="needToAdd"></param>
        private static void IncludeFromResources(ResourceDictionary keyValuePairs)
        {
            var dictionaryExists = Application.Current.Resources.MergedDictionaries.Contains(keyValuePairs);
            if (dictionaryExists)
            {
                Application.Current.Resources.MergedDictionaries.Remove(keyValuePairs);
            }

            Application.Current.Resources.MergedDictionaries.Add(keyValuePairs);

        }
    }
}

