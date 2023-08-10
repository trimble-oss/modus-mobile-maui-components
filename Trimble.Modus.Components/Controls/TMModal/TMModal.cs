using Trimble.Modus.Components.Modal;

namespace Trimble.Modus.Components
{
    public class TMModal
    {
        #region Private field
        TMModalContents TMModalPage;
        #endregion
        #region Public Properties
        public string Title
        {
            get => TMModalPage.Title;
            set => TMModalPage.Title= value;
        }
        public string Message
        {
            get => TMModalPage.Message;
            set => TMModalPage.Message = value;
        }
        public ImageSource TitleIcon
        {
            get => TMModalPage.TitleIcon;
            set => TMModalPage.TitleIcon = value;
        }
        public bool FullWidthButton
        {
            get => TMModalPage.FullWidthButton;
            set => TMModalPage.FullWidthButton = value;
        }
        #endregion
        #region Constructor
        public TMModal(string titleText, string messageText = null, ImageSource titleIconSource = null, bool fullWidthButton = false)
        {
            TMModalPage = new(titleText);
            Title = titleText;
            Message = messageText;
            TitleIcon = titleIconSource;
            FullWidthButton = fullWidthButton;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Action to be performed when modal is going to close
        /// </summary>
        public Action OnModalClosing
        {
            get => TMModalPage.OnModalClosing;
            set => TMModalPage.OnModalClosing = value;
        }
        /// <summary>
        /// Add Primary button
        /// </summary>
        /// <param name="title">Button titke</param>
        /// <param name="clickAction">Button click action</param>
        /// <exception cref="ArgumentNullException">Thrown when title is empty</exception>
        /// <exception cref="InvalidOperationException">Thrown while trying to add more that 3 button</exception>
        public void AddPrimaryAction(string buttonText, Action action = null)
        {
            TMModalPage.AddPrimaryAction(buttonText, action);
        }
        /// <summary>
        /// Add Secondary button
        /// </summary>
        /// <param name="title">Button titke</param>
        /// <param name="clickAction">Button click action</param>
        /// <exception cref="ArgumentNullException">Thrown when title is empty</exception>
        /// <exception cref="InvalidOperationException">Thrown while trying to add more that 3 button</exception>
        public void AddSecondaryAction(string buttonText, Action action = null)
        {
            TMModalPage.AddSecondaryAction(buttonText, action);
        }
        /// <summary>
        /// Add Tertiary button
        /// </summary>
        /// <param name="title">Button titke</param>
        /// <param name="clickAction">Button click action</param>
        /// <exception cref="ArgumentNullException">Thrown when title is empty</exception>
        /// <exception cref="InvalidOperationException">Thrown while trying to add more that 3 button</exception>
        public void AddTertiaryAction(string buttonText, Action action = null)
        {
            TMModalPage.AddTertiaryAction(buttonText, action);
        }
        /// <summary>
        /// Add Danger button
        /// </summary>
        /// <param name="title">Button titke</param>
        /// <param name="clickAction">Button click action</param>
        /// <exception cref="ArgumentNullException">Thrown when title is empty</exception>
        /// <exception cref="InvalidOperationException">Thrown while trying to add more that 3 button</exception>
        public void AddDangerAction(string buttonText, Action action = null)
        {
            TMModalPage.AddDangerAction(buttonText, action);
        }
        /// <summary>
        /// Adds TMInput in the body of the modal
        /// </summary>
        /// <param name="inputConfigurationHandler">Handler to configure the TMInput</param>
        public void AddTextInput(Action<TMInput> inputConfigurationHandler = null)
        {
            TMModalPage.AddTextInput(inputConfigurationHandler);
        }
        /// <summary>
        /// Display modal
        /// </summary>
        /// <param name="closeWhenBackgroundIsClicked">If false, clicking on background will not close the modal. true by default</param>
        public void Show(bool closeWhenBackgroundIsClicked=false)
        {
            TMModalPage.Show(closeWhenBackgroundIsClicked);
        }
        /// <summary>
        /// Close modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Dismiss()
        {
            TMModalPage.CloseModal(TMModalPage, null);
        }
        #endregion

    }
}
