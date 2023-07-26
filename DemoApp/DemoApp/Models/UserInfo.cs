using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class UserInfo
    {
        public NameInfo Name { get; set; }
        public DobInfo DateOfBirth { get; set; }
        public LocationInfo Location { get; set; }
        public PictureInfo Picture { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }
    public class ApiResponse
    {
        public List<UserInfo> Results { get; set; }
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
}
