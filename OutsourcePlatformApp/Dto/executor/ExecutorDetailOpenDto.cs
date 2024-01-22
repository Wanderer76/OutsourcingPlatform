using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class ExecutorDetailOpenDto
{
    public string Surname { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string? SecondName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string? About { get; set; }
    public DetailExecutorDto Executor { get; set; }
    public ContactsUpdateDto? Contacts { get; set; }
    public bool IsClose { get; set; } = false;

    public ExecutorDetailOpenDto(User user, int finishProjectCount)
    {
        Surname = user.Surname;
        Name = user.Name;
        SecondName = user.SecondName;
        UserName = user.Username;
        Email = user.Email;
        Phone = user.Phone;
        About = user?.UserContacts?.About;
        Executor = new DetailExecutorDto(user.Executor, finishProjectCount);
        if (user.UserContacts != null)
            Contacts = new ContactsUpdateDto
            {
                Id = user.UserContacts.Id,
                About = user.UserContacts.About,
                Contacts = user.UserContacts.ContactLinks == null
                    ? new List<ContactDto>()
                    : user.UserContacts.ContactLinks.Select(x => new ContactDto(x.Name, x.Url)).ToList(),
            };
    }
}