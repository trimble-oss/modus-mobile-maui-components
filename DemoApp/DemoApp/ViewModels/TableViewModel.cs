using CommunityToolkit.Mvvm.ComponentModel;
using DemoApp.Constant;
using DemoApp.Models;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace DemoApp.ViewModels;
public partial class TableViewModel : ObservableObject
{
    Random rnd = new Random();

    [ObservableProperty]
    private List<User> _users;
    [ObservableProperty]
    private User _selectedItem;
    [ObservableProperty]
    private bool _dividerToggled = false;
    [ObservableProperty]
    private SelectionMode _selectionMode = SelectionMode.None;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectionMode))]
    private int _selectionModeRadioOption = 0;
    partial void OnSelectionModeRadioOptionChanging(int value)
    {
        SelectionMode = (SelectionMode)value;
    }

    public TableViewModel()
    {
        Users = new List<User>();
        LoadData();
    }

    private async void LoadData()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("UserData.json");
        using var reader = new StreamReader(stream);

        var result = JsonConvert.DeserializeObject<ApiResponse>(reader.ReadToEnd());
        Users.Clear();
        foreach (var userInfo in result.Results)
        {
            var randomNumber = rnd.Next(0, 50);
            var user = new User
            {
                Name = $"{userInfo.Name.First} {userInfo.Name.Last}",
                Gender = userInfo.Gender,
                Color = userInfo.Gender.Equals("male") ? Brush.LightSkyBlue : Brush.HotPink,
                DateofBirth = DateTime.Parse(userInfo.Dob.Date),
                Address = $"{userInfo.Location.Street.Number} {userInfo.Location.Street.Name}, {userInfo.Location.City}",
                ProfilePic = userInfo.Picture.Large,
                Phone = userInfo.Phone,
                Email = userInfo.Email,
                Score = randomNumber.ToString(),
                IsVerified = (randomNumber > 25 ? true : false)
            };

            Users.Add(user);
        }
    }
}
