using System;
namespace Trimble.Modus.Components.Helpers
{
    public class ThemeManager
    {
        private static ResourceDictionary LightThemeResourceDictionary { get; set; }
        private static ResourceDictionary DarkThemeResourceDictionary { get; set; }
        internal static AppTheme CurrentTheme { get; private set; }

        public static void Initialize()
        {
            LightThemeResourceDictionary = new Styles.LightTheme();
            DarkThemeResourceDictionary = new Styles.DarkTheme();

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

