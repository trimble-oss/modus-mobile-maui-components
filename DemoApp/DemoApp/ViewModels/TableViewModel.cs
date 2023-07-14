using CommunityToolkit.Mvvm.ComponentModel;
using DemoApp.Constant;
using DemoApp.Model;
using Microsoft.Maui.Controls;

namespace DemoApp.ViewModels;
public partial class TableViewModel : ObservableObject
{
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
        var user1 = new User()
        {
            Name = "User 1",
            Description = "New User",
            ID = "1",
            Logo = ImageConstants.AccountIcon,
            Qualified = true
        };
        var user3 = new User()
        {
            Name = "User 2",
            Description = "New User",
            ID = "2",
            Logo = ImageConstants.ContactIcon,
            Qualified = false
        };
        var user2 = new User()
        {
            Name = "User 3",
            Description = "New User",
            ID = "3",
            Logo = ImageConstants.SearchIcon,
            Qualified = true
        };
        var user4 = new User()
        {
            Name = "User 4",
            Description = "New User",
            ID = "4",
            Logo = ImageConstants.PasswordIcon,
            Qualified = false
        };
        
        Users = new List<User>() { user1, user2, user3, user4 };
    }
}
