using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;

namespace DemoApp.ViewModels
{
    public partial class MessageSamplePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private MessageSize _messageSize;
        public MessageSamplePageViewModel()
        {
        }

        [RelayCommand]
        private void SizeChanged(TMRadioButtonEventArgs e)
        {
            switch (e.RadioButtonIndex)
            {
                case 1:
                    MessageSize = MessageSize.Small;
                    break;
                case 2:
                    MessageSize = MessageSize.Large;
                    break;
                case 3:
                    MessageSize = MessageSize.XLarge;
                    break;
                default:
                    MessageSize = MessageSize.Default;
                    break;
            }
        }
    }
}

