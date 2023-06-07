using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemoApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// This variables is used to raise the event when the property value is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This method is triggered when any of the property is changed
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
