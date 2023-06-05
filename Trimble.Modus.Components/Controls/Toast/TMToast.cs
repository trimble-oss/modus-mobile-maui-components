using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast
{
    public class TMToast
    {
        private PopupNavigation popupNavigation;
        private string message, actionButtonText = null;
        private Action? action = null;
        public ToastTheme theme = ToastTheme.Default;
        public bool isDismissable = true;
        public TMToast(string message, string actionButtonText = null, Action? action = null)
        {
            popupNavigation = new PopupNavigation();
            this.message = message;
            this.actionButtonText = actionButtonText;
            this.action = action;
        }

        public void Show()
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("Message is required");
            }
            popupNavigation.PushAsync(new TMToastContents(message, actionButtonText, popupNavigation, theme, action, isDismissable), false);
        }
    }
}
