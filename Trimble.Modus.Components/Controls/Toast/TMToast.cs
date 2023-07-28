using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Controls.Toast
{
    public class TMToast
    {
        #region Private Properties
        private string message, actionButtonText = null;
        #endregion
        #region Public Properties
        private Action? action = null;
        public ToastTheme theme = ToastTheme.Default;
        public bool isDismissable = true;
        #endregion
        public TMToast(string message, string actionButtonText = null, Action? action = null)
        {
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
            PopupService.Instance.PresentAsync(new TMToastContents(message, actionButtonText, theme, action, isDismissable), false);
        }
        #endregion
    }
}
