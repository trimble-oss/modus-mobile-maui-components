using System;

namespace Trimble.Modus.Components
{
    public class AlertDialogue
    {
        private string Title;
        private string Message;
        private string PrimaryButtonText;
        private string SecondaryText;

        TaskCompletionSource<bool> alertClosedTask = new TaskCompletionSource<bool>();

        private AlertDialogue()
        {

        }

        public AlertDialogue(string title, string message = "", string primaryButtonText = "Ok", string secondaryText = "")
        {
            Title = title;
            Message = message;
            PrimaryButtonText = primaryButtonText;
            SecondaryText = secondaryText;
        }

        public Task<bool> Show()
        {
            if (string.IsNullOrWhiteSpace(Title) && string.IsNullOrWhiteSpace(Message))
            {
                throw new ArgumentNullException("Title cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(PrimaryButtonText) && string.IsNullOrWhiteSpace(SecondaryText))
            {
                throw new ArgumentNullException("Alert should atleast have one button");
            }


            TMModal modal = new TMModal(Title, Message);
            if (!string.IsNullOrEmpty(PrimaryButtonText))
            {
                modal.AddPrimaryAction(PrimaryButtonText, () =>
                {
                    alertClosedTask.SetResult(true);
                });
            }
            if (!string.IsNullOrEmpty(SecondaryText))
            {
                modal.AddSecondaryAction(SecondaryText, () =>
                {
                    alertClosedTask.SetResult(false);
                });
            }
            modal.Show();
            return alertClosedTask.Task;
        }
    }
}

