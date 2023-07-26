using CommunityToolkit.Mvvm.ComponentModel;
using DemoApp.Models;
using Newtonsoft.Json;
using Trimble.Modus.Components.Enums;
using System.Collections;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Trimble.Modus.Components;

namespace DemoApp.ViewModels
{
    internal partial class TMListViewPageViewModel : ObservableObject
    {
        #region Private Fields
        [ObservableProperty]
        private ListSelectionMode selectionMode;

        [ObservableProperty]
        private IEnumerable itemSource;
        private List<User> Users { get; set; }
        #endregion

        #region Constructor
        public TMListViewPageViewModel()
        {
            Users = new List<User>();
            LoadData();
            SelectionMode = ListSelectionMode.Single;
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
                    Gender = userInfo.Gender,
                    Color = userInfo.Gender.Equals("male") ? Brush.LightSkyBlue : Brush.HotPink,
                    DateofBirth = DateTime.Parse(userInfo.Dob.Date),
                    Address = $"{userInfo.Location.Street.Number} {userInfo.Location.Street.Name}, {userInfo.Location.City}",
                    ProfilePic = userInfo.Picture.Large,
                    Phone = userInfo.Phone,
                    Email = userInfo.Email
                };

                Users.Add(user);
            }
            ItemSource = Users;
        }
        [RelayCommand]
        private void ItemSelected(User user)
        {
            Console.WriteLine(user.Name + " " + user.Address);
        }
        [RelayCommand]
        private void SelectionGroup(TMRadioButtonEventArgs parameter)
        {
            if (parameter is TMRadioButtonEventArgs radioButton)
            {
                SelectionMode = radioButton.RadioButtonIndex switch
                {
                    1 => ListSelectionMode.Multiple,
                    2 => ListSelectionMode.None,
                    _ => ListSelectionMode.Single,
                };
            }
        }
        [RelayCommand]
        private void PhoneClicked()
        {
            Console.WriteLine("Phone Clicked");
        }
        [RelayCommand]
        private void EmailClicked()
        {
            Console.WriteLine("Email Clicked");
        }
        #endregion

    }
}
