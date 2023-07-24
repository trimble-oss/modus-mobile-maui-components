using CommunityToolkit.Mvvm.ComponentModel;
using DemoApp.Resources.Data_Models;
using DemoApp.Resources;
using Newtonsoft.Json;
using Trimble.Modus.Components.Enums;
using System.Collections;

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
        #endregion

        #region Constructor
        public TMListViewPageViewModel(DataTemplate cell)
        {
            Users = new List<User>();
            LoadData();
            ItemTemplate = cell;
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
        #endregion
    }
}
