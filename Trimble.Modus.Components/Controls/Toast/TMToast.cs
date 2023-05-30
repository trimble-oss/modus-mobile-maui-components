
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast
{
    public class TMToast
    {
        PopupNavigation popupNavigation;

        public TMToast()
        {
            popupNavigation = new PopupNavigation();
        }

        public void Show(string message, ToastTheme theme = ToastTheme.Default)
        {
            Show(message, null, null, theme);
        }

        public void Show(string message, string actionButtonText = null, Action? action = null, ToastTheme theme = ToastTheme.Default)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("Message is required");
            }
            popupNavigation.PushAsync(new TMToastContents(message, actionButtonText, popupNavigation, theme, action), false);
        }
    }
}
