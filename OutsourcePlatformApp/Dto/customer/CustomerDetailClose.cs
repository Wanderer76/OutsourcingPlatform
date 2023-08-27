using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class CustomerDetailClose
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string SecondName { get; set; }
    public string Username { get; set; }
    public string? About { get; set; }
    
    public int OrderCount { get; set; }

    public CustomerDetailClose(User user,int orderCount)
    {
        Username = user.Username;
        Surname = user.Surname;
        Name = user.Name;
        SecondName = user.SecondName;
        OrderCount = orderCount;
        if (user.UserContacts != null)
            About = user.UserContacts.About;
    }
}