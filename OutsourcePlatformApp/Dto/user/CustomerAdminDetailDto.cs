using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto.user;

public class CustomerAdminDetailDto
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? SecondName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public ContactsUpdateDto? Contacts { get; set; }
    public string INN { get; set; }
    public int CustomerId { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public int OrderCount { get; set; }
    public int ClosedOrderCount { get; set; }
    public string Role { get; set; }
    public bool IsBanned { get; set; }
    public string BannedMessage { get; set; }

    public CustomerAdminDetailDto(User user, int orderCount, int closedOrderCount)
    {
        Surname = user.Surname;
        Name = user.Name;
        SecondName = user.SecondName;
        UserName = user.Username;
        Email = user.Email;
        Phone = user.Phone;
        var customer = user.Customer;
        INN = customer.INN;
        CustomerId = customer.Id;
        CompanyName = customer.CompanyName;
        Address = customer.Address;
        OrderCount = orderCount;
        ClosedOrderCount = closedOrderCount;
        Role = user.UserRoles.First().Name;
        IsBanned = user.IsBanned;
        BannedMessage = user.BannedMessage;
        if (user.UserContacts != null)
            Contacts = new ContactsUpdateDto
            {
                Id = user.UserContacts.Id,
                About = user.UserContacts.About,
                // GithubUrl = user.UserContacts.GithubUrl,
                //Messager = user.UserContacts.Messager,
                //VkNickname = user.UserContacts.VkNickname
            };
    }
}