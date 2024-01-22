using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class CustomerInfoDto
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DetailCustomerDto Customer { get; set; }
        public ContactsUpdateDto? Contacts { get; set; }

        public CustomerInfoDto(User user,List<Order> orders)
        {
            Surname = user.Surname;
            Name = user.Name;
            SecondName = user.SecondName;
            Email = user.Email;
            Phone = user.Phone;
            Customer = new DetailCustomerDto(user.Customer,orders);
            if (user.UserContacts != null)
                Contacts = new ContactsUpdateDto
                {
                    About = user.UserContacts.About, 
                    Contacts = user.UserContacts.ContactLinks.Select(x=>new ContactDto(x.Name,x.Url)).ToList(),
                };
        }
    }
}