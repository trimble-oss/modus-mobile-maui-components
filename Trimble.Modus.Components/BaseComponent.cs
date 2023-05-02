
using Microsoft.Maui.Controls.Xaml;
using Trimble.Modus.Core.Themes;

namespace Trimble.Modus.Core
{
    // All the code in this file is included in all platforms.
    public class BaseComponent
    {
        public static ResourceDictionary ResourceDictionary => Application.Current.Resources;

        public static ResourceDictionary getColors()
        {
            return new Colours();
            
        }

      
    }
}