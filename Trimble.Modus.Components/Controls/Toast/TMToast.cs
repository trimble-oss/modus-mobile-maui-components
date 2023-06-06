using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast
{
    public class TMToast
    {
        #region Private Properties
        private PopupNavigation popupNavigation;
        private string message, actionButtonText = null;
        #endregion
        #region Public Properties
        private Action? action = null;
        public ToastTheme theme = ToastTheme.Default;
        public bool isDismissable = true;
        #endregion
        public TMToast(string message, string actionButtonText = null, Action? action = null)
        {
            popupNavigation = new PopupNavigation();
            this.message = message;
            this.actionButtonText = actionButtonText;
            this.action = action;
        }
        #region Public Methods
        /// <summary>
        /// To show the toast page
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void Show()
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("Message is required");
            }
            popupNavigation.PushAsync(new TMToastContents(message, actionButtonText, popupNavigation, theme, action, isDismissable), false);
        }
        #endregion
    }
}
