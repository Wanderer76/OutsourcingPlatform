using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class DetailOrderDto
{
    public int OrderId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<CategoryDto>? OrderCategories { get; set; }
    public List<SkillDto>? OrderSkills { get; set; }
    public DateOnly Deadline { get; set; }
    public int Price { get; set; }
    public int MaxWorkers { get; set; }
    public string CompanyName { get; set; }
    public bool IsCompleted { get; set; }
    public CommonCustomerDto Customer { get; set; }
    public int Notifications { get; set; }

    public string Addres { get; set; }
    public string INN { get; set; }
    public string Fullname { get; set; }
    public ContactsUpdateDto Contacts { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string VkNickname { get; set; }
    public string Messager { get; set; }
    public ICollection<ExecutorResponseDto>? ExecutorResponse { get; set; }

    public DetailOrderDto(Order order, User user)
    {
        OrderId = order.OrderId;
        Name = order.Name;
        Description = order.Description;
        Deadline = order.Deadline;
        Price = order.Price;
        MaxWorkers = order.MaxWorkers;
        CompanyName = order.CompanyName;
        IsCompleted = order.IsCompleted;
        OrderSkills = order.OrderSkills.Select(skill => new SkillDto
        {
            Id = skill.Id,
            Name = skill.Name
        }).ToList();
        OrderCategories = order.OrderCategories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();

        Customer = new CommonCustomerDto
        {
            Addres = user.Customer!.CompanyName,
            Phone = user.Phone,
            Email = user.Email,
            Fullname = $"{user.Surname} {user.Name} {user.SecondName}",
          
            INN = user.Customer.INN
        };
        Addres = user.Customer!.CompanyName;
        INN = user.Customer.INN;
        Fullname = $"{user.Surname} {user.Name} {user.SecondName}";
        Phone = user.Phone;
        Email = user.Email;
        Contacts = new ContactsUpdateDto
        {
            VkNickname = user.UserContacts == null ? "" : user.UserContacts.VkNickname ?? "",
            Messager = user.UserContacts == null ? "" : user.UserContacts.Messager ?? ""
        };
      

        Notifications = order.Responses.Count(re => re.IsCompleted);
    }
}