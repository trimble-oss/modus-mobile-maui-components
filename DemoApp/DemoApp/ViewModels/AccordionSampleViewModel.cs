using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.ObjectModel;
using Trimble.Modus.Components;
using Trimble.Modus.Components.Enums;

namespace DemoApp.ViewModels
{
    public partial class AccordionSampleViewModel : ObservableObject
    {
        [ObservableProperty]
        private AccordionSize _size;
        [ObservableProperty]
        private AccordionChevronPosition _chevronPosition;
        [ObservableProperty]
        private IEnumerable _items ;
        private ObservableCollection<User> Users = new ObservableCollection<User>();

        public AccordionSampleViewModel()
        {
            LoadData();
        }

        private async void LoadData()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("UserData.json");
            using var reader = new StreamReader(stream);

            var result = JsonConvert.DeserializeObject<ApiResponse>(reader.ReadToEnd());
            Users.Clear();
            for ( int i = 0; i < 20; i++)
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
            Items= Users;
        }

        [RelayCommand]
        private void SizeRadioButton(TMRadioButtonEventArgs e)
        {
           Size = (AccordionSize)Enum.Parse(typeof(AccordionSize), e.Value.ToString());
        }
        [RelayCommand]
        private void ChevronPositionRadioButton(TMRadioButtonEventArgs e)
        {
            ChevronPosition = (AccordionChevronPosition)Enum.Parse(typeof(AccordionChevronPosition), e.Value.ToString());
        }
    }
}
