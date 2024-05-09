using System;
namespace Trimble.Modus.Components.Helpers
{
    public class ThemeManager
    {
        private static ResourceDictionary LightThemeResourceDictionary { get; set; }
        private static ResourceDictionary DarkThemeResourceDictionary { get; set; }
        internal static AppTheme CurrentTheme { get; private set; }

        public static void Initialize(ResourceDictionary lightTheme = null, ResourceDictionary darkTheme = null, bool useDarkThemeAsLightTheme = false)
        {
            var defaultLightTheme = new Styles.LightTheme();
            var defaultDarkTheme = new Styles.DarkTheme();
            if (lightTheme != null && lightTheme.Count > 0)
            {

                LightThemeResourceDictionary = new ResourceDictionary();
                // Add lightTheme and LightThemeResourceDictionary to the MergedDictionaries of the new ResourceDictionary
                LightThemeResourceDictionary.MergedDictionaries.Add(defaultLightTheme);
                LightThemeResourceDictionary.MergedDictionaries.Add(lightTheme);

                // LightThemeResourceDictionary = lightTheme;
                // var notExistsInDictionary = defaultLightTheme.Where(x => !lightTheme.ContainsKey(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                // foreach (var item in notExistsInDictionary)
                // {
                //     var keyExists = LightThemeResourceDictionary.ContainsKey(item.Key);
                //     if (keyExists)
                //     {
                //         LightThemeResourceDictionary[item.Key] = item.Value;
                //     }
                //     else
                //     {
                //         LightThemeResourceDictionary.Add(item.Key, item.Value);
                //     }
                // }
            }
            else
            {
                LightThemeResourceDictionary = defaultLightTheme;
            }
            if (useDarkThemeAsLightTheme)
            {
                DarkThemeResourceDictionary = LightThemeResourceDictionary;
            }
            else
            {
                if (darkTheme != null && darkTheme.Count > 0)
                {
                    DarkThemeResourceDictionary = darkTheme;
                    var notExistsInDictionary = defaultDarkTheme.Where(x => !darkTheme.ContainsKey(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                    foreach (var item in notExistsInDictionary)
                    {
                        var keyExists = DarkThemeResourceDictionary.ContainsKey(item.Key);
                        if (keyExists)
                        {
                            DarkThemeResourceDictionary[item.Key] = item.Value;
                        }
                        else
                        {
                            DarkThemeResourceDictionary.Add(item.Key, item.Value);
                        }
                    }
                }
                else
                {
                    DarkThemeResourceDictionary = defaultDarkTheme;
                }
            }

            UpdateTheme(Application.Current.RequestedTheme);

            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
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

            IncludeFromResources(new Styles.Colors());
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

