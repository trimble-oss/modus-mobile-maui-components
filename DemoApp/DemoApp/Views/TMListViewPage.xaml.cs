using DemoApp.Resources;
using Newtonsoft.Json;
using Trimble.Modus.Components;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text.Json;
using System.Diagnostics;
namespace DemoApp.Views;

public partial class TMListViewPage : ContentPage
{
    HttpClient _client;
    public DataTemplate TextCellTemplate { get; set; }
    public DataTemplate ViewCellTemplate { get; set; }

    public List<User> users;
    public TMListViewPage()
    {
        users = new List<User>();
        InitializeComponent();
        //RefreshDataAsync();
        LoadData();
        TextCellTemplate = TextCell;
        ViewCellTemplate = ViewCell;
       // textCellList.ItemTemplate = ViewCellTemplate;
        textCellList.SelectionMode = Trimble.Modus.Components.Enums.ListSelectionMode.Single;
        BindingContext = this;
    }
    public async Task TestApi()
    {
        var uri = new Uri("https://randomuser.me/api/?results=20");
        Console.WriteLine(uri?.ToString());

        var res = await _client.GetByteArrayAsync(uri);
        Console.WriteLine("res1Value" + res);

        var response = await _client.GetStringAsync(uri);
        Console.WriteLine("Response" + response);
    }
    private void list2_ItemSelected(object sender, SelectableItemEventArgs e)
    {
     //   Console.WriteLine(((Person)e.SelectableItem).Title +" "+ ((Person)e.SelectableItem).Description+" "+e.SelectedItemsCount);
    }

    private void list_ItemSelected(object sender, SelectableItemEventArgs e)
    {
       // Console.WriteLine(((Person)e.SelectableItem).Title + " " + ((Person)e.SelectableItem).Description + " " + e.SelectedItemsCount);
    }

    private void OnCellGroupButtonChanged(object sender, TMRadioButtonEventArgs e)
    {
        if (e.RadioButtonIndex == 0)
        {
            textCellList.ItemTemplate = TextCellTemplate;
        }
        else if (e.RadioButtonIndex == 1)
        {
            textCellList.ItemTemplate = ViewCellTemplate;
        }
    }

    private void OnSelectionGroupButtonChanged(object sender, TMRadioButtonEventArgs e)
    {
        if (e.RadioButtonIndex == 0)
        {
            textCellList.SelectionMode = Trimble.Modus.Components.Enums.ListSelectionMode.Single;
        }
        else if (e.RadioButtonIndex == 1)
        {
            textCellList.SelectionMode = Trimble.Modus.Components.Enums.ListSelectionMode.Multiple;
        }
        else if (e.RadioButtonIndex == 2)
        {
            textCellList.SelectionMode = Trimble.Modus.Components.Enums.ListSelectionMode.ReadOnly;
        }
    }
    private async void LoadData()
    {
        HttpClient client = new HttpClient();

        using (client)
        {
            string response = await client.GetStringAsync("https://randomuser.me/api/?results=3");
            Console.WriteLine("Response"+response);
            var result = JsonConvert.DeserializeObject<ApiResponse>(response);
            Console.WriteLine("Result"+result);

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

                users.Add(user);
            }
          //  textCellList.ItemsSource = users;
        }
    }

    private void OnPhoneClicked(object sender, EventArgs e)
    {
        Console.WriteLine("Phone Clicked");

    }
    private void OnEmailClicked(object sender, EventArgs e)
    {
        Console.WriteLine("Email Clicked");
    }
}
public class ApiResponse
{
    public List<UserInfo> Results { get; set; }
}

public class UserInfo
{
    public NameInfo Name { get; set; }
    public DobInfo Dob { get; set; }
    public LocationInfo Location { get; set; }
    public PictureInfo Picture { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
public class NameInfo
{
    public string First { get; set; }
    public string Last { get; set; }
}

public class DobInfo
{
    public string Date { get; set; }
}

public class LocationInfo
{
    public StreetInfo Street { get; set; }
    public string City { get; set; }
}

public class StreetInfo
{
    public string Number { get; set; }
    public string Name { get; set; }
}

public class PictureInfo
{
    public string Large { get; set; }
}
public class Person
{
    public string Title { get; set; }
    public int Age { get; set; }
    public ImageSource LeftIconSource { get; set; }

    public ImageSource RightIconSource { get; set; }
    public string Description { get; set; }
}

public class TestUser
{ 
    public string Title { get; set; }
 
    public string Description { get; set; }
}
