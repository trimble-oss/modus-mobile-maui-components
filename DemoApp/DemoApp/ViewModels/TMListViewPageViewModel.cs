using DemoApp.Resources.Data_Models;
using System.ComponentModel;
using DemoApp.Resources;
using Newtonsoft.Json;
using Trimble.Modus.Components.Enums;
using System.Collections;

namespace DemoApp.ViewModels
{
    public class TMListViewPageViewModel : BaseViewModel
    {
        #region Private Fields
        private ListSelectionMode _selectedMode;
        private DataTemplate _itemTemplate, _textCellTemplate, _viewCellTemplate;
        private IEnumerable _itemSource;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Public Fields
        public List<User> Users { get; set; }

        public IEnumerable ItemSource
        {
            get => _itemSource;
            set
            {
                _itemSource = value;
                OnPropertyChanged(nameof(ItemSource));
            }
        }

        public DataTemplate TextCellTemplate
        {
            get => _textCellTemplate;
            set
            {
                _textCellTemplate = value;
                OnPropertyChanged(nameof(TextCellTemplate));
            }
        }

        public DataTemplate ItemTemplate
        {
            get => _itemTemplate;
            set
            {
                _itemTemplate = value;
                OnPropertyChanged(nameof(ItemTemplate));
            }
        }

        public DataTemplate ViewCellTemplate
        {
            get => _viewCellTemplate;
            set
            {
                _viewCellTemplate = value;
                OnPropertyChanged(nameof(ViewCellTemplate));
            }
        }

        public ListSelectionMode SelectionMode
        {
            get => _selectedMode;
            set
            {
                _selectedMode = value;
                OnPropertyChanged(nameof(SelectionMode));
            }
        }
        #endregion

        #region Constructor
        public TMListViewPageViewModel()
        {
            Users = new List<User>();
            LoadData();
            ItemTemplate = TextCellTemplate;
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
