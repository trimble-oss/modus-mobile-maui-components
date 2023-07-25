using CommunityToolkit.Mvvm.ComponentModel;
using DemoApp.Resources.Data_Models;
using DemoApp.Resources;
using Newtonsoft.Json;
using Trimble.Modus.Components.Enums;
using System.Collections;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;

namespace DemoApp.ViewModels
{
    internal partial class TMListViewPageViewModel  : ObservableObject
    {
        #region Private Fields
        [ObservableProperty]
        private ListSelectionMode selectionMode;
        [ObservableProperty]
        private DataTemplate itemTemplate;
        [ObservableProperty]
        private IEnumerable itemSource;
        private List<User> Users { get; set; }
        private DataTemplate textCell, viewCell;

        public ICommand ItemSelectedCommand { get; }
        public ICommand SelectionGroupButtonCommand { get; }
        public ICommand OnCellGroupButtonCommand { get; }
        public ICommand EmailButtonCommand { get; }
        public ICommand PhoneButtonCommand { get; }

        #endregion

        #region Constructor
        public TMListViewPageViewModel(DataTemplate textCell,DataTemplate viewCell)
        {
            Users = new List<User>();
            LoadData();
            this.textCell = textCell;
            this.viewCell = viewCell;
            ItemTemplate = textCell;
            SelectionMode = ListSelectionMode.Single;
            ItemSelectedCommand = new RelayCommand<User>(ItemSelected);
            SelectionGroupButtonCommand = new RelayCommand<TMRadioButtonEventArgs>(OnSelectionGroupButtonChanged);
            OnCellGroupButtonCommand = new RelayCommand<TMRadioButtonEventArgs>(OnCellGroupButtonCommandChanged);
            EmailButtonCommand = new RelayCommand(OnEmailClicked);
            PhoneButtonCommand = new RelayCommand(OnPhoneClicked);
        }
        #endregion
        #region Private Methods
        private async void LoadData()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("UserData.json");
            using var reader = new StreamReader(stream);

            var result = JsonConvert.DeserializeObject<ApiResponse>(reader.ReadToEnd());
            Users.Clear();
            foreach (var userInfo in result.Results)
            {
                var user = new User
                {
                    Name = $"{userInfo.Name.First} {userInfo.Name.Last}",
                    DOB = DateTime.Parse(userInfo.Dob.Date),
                    Address = $"{userInfo.Location.Street.Number} {userInfo.Location.Street.Name}, {userInfo.Location.City}",
                    ProfilePic = userInfo.Picture.Large,
                    Phone = userInfo.Phone,
                    Email = userInfo.Email
                };

                Users.Add(user);
            }
            ItemSource = Users;
        }
        private void ItemSelected(User user)
        {
            Console.WriteLine(user.Name + " " + user.Address);
        }
        private void OnSelectionGroupButtonChanged(TMRadioButtonEventArgs parameter)
        {
                if (parameter is TMRadioButtonEventArgs radioButton)
                 {
                     SelectionMode = radioButton.RadioButtonIndex switch
                     {
                         0 => ListSelectionMode.Single,
                         1 => ListSelectionMode.Multiple,
                         2 => ListSelectionMode.None,
                         _ => ListSelectionMode.Single,
                     };
                 }
        }
        private void OnCellGroupButtonCommandChanged(TMRadioButtonEventArgs e)
        {
            if (e is TMRadioButtonEventArgs radioButton)
            {

                ItemTemplate = radioButton.RadioButtonIndex switch
                {
                    0 => textCell,
                    1 => viewCell,
                    _ => textCell
                };
            }
        }
        private void OnPhoneClicked()
        {
            Console.WriteLine("Phone Clicked");
        }

        private void OnEmailClicked()
        {
            Console.WriteLine("Email Clicked");
        }
        #endregion

    }
}
