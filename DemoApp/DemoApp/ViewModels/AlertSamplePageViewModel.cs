using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;

namespace DemoApp.ViewModels
{
    public partial class AlertSamplePageViewModel : ObservableObject
    {
        TMAlert _alert;
        Layout _parentLayout;

        private ICommand _buttonCommand;

        [ObservableProperty]
        private bool _hideLeftIcon = true;

        [ObservableProperty]
        private bool _showButton = false;

        [ObservableProperty]
        private bool _dismissable = true;

        [ObservableProperty]
        private int _selectedAlertType = 0;

        public AlertSamplePageViewModel(Layout parent)
        {
            _alert = new TMAlert("This is a sample alert") { Type = AlertType.Success };
            _parentLayout = parent;

            _buttonCommand = new RelayCommand(HideAlert);
        }

        [RelayCommand]
        private void ShowAlert()
        {
            if (_parentLayout.Children.Contains(_alert)) return;
            _alert.ShowAlert(_parentLayout);
        }

        private void HideAlert()
        {
            _alert.DismissAlert();
        }

        partial void OnHideLeftIconChanged(bool value)
        {
            _alert.HideLeftIcon = !value;
        }

        partial void OnSelectedAlertTypeChanged(int value)
        {
            _alert.Type = (AlertType)value;
        }

        partial void OnDismissableChanged(bool value)
        {
            _alert.Dismissable = value;
        }

        partial void OnShowButtonChanged(bool value)
        {
            if (value)
            {
                _alert.ButtonClickedCommand = _buttonCommand;
                _alert.ButtonText = "Button";
            }
            else
            {
                _alert.ButtonText = null;
            }
        }

    }
}

