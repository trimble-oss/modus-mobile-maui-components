using DemoApp.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace DemoApp.Helper
{
    public class UserDataCreator
    {
        /// <summary>
        /// Generate an ObservableCollection of users
        /// </summary>
        /// <param name="count">Count should be < 50</param>
        public async static Task<ObservableCollection<User>> LoadData(int? count = null)
        {
            ObservableCollection<User> Users = new ObservableCollection<User>();
            using var stream = await FileSystem.OpenAppPackageFileAsync("UserData.json");
            using var reader = new StreamReader(stream);

            var result = JsonConvert.DeserializeObject<ApiResponse>(reader.ReadToEnd());
            Users.Clear();
            int iterations = count ?? result.Results.Count;
            for (int i = 0; i < iterations; i++)
            {
                var userInfo = result.Results[i];
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
            return Users;
        }
    }
}
