using System;
namespace Trimble.Modus.Components.Helpers
{
    public class ThemeManager
    {
        private static ResourceDictionary LightThemeResourceDictionary { get; set; }
        private static ResourceDictionary DarkThemeResourceDictionary { get; set; }
        internal static AppTheme CurrentTheme { get; private set; }

        public static void Initialize(ResourceDictionary lightTheme = null, ResourceDictionary darkTheme = null)
        {
            var defaultLightTheme = new Styles.LightTheme();
            var defaultDarkTheme = new Styles.DarkTheme();
            IncludeFromResources(new Styles.Colors());
            if (lightTheme != null && lightTheme.Count > 0)
            {
                LightThemeResourceDictionary = UpdateDefaulThemeWithCustomTheme(defaultLightTheme, lightTheme);
            }
            else
            {
                LightThemeResourceDictionary = defaultLightTheme;
            }

            if (darkTheme != null && darkTheme.Count > 0)
            {
                DarkThemeResourceDictionary = UpdateDefaulThemeWithCustomTheme(defaultDarkTheme, darkTheme);
            }
            else
            {
                if (lightTheme != null && lightTheme.Count > 0)
                {
                    DarkThemeResourceDictionary = LightThemeResourceDictionary;
                }
                else
                {
                    DarkThemeResourceDictionary = defaultDarkTheme;
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

            RemoveFromResources(LightThemeResourceDictionary);
            RemoveFromResources(DarkThemeResourceDictionary);
            if (theme == AppTheme.Dark)
            {
                IncludeFromResources(DarkThemeResourceDictionary);
            }
            else
            {
                IncludeFromResources(LightThemeResourceDictionary);
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

