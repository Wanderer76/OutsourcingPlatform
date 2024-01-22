using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class DetailOrderDto
{
    public int OrderId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<CategoryDto>? OrderCategories { get; set; }
    public List<SkillDto>? OrderSkills { get; set; }
    public List<OrderVacancyDto>? OrderVacancies { get; set; }
    public DateOnly Deadline { get; set; }
    public int Price { get; set; }
    public int MaxWorkers { get; set; }
    public string CompanyName { get; set; }
    public bool IsCompleted { get; set; }
    public CommonCustomerDto? Customer { get; set; }
    public CommonExecutorDto? Executor { get; set; }
    public int Notifications { get; set; }
    public string Address { get; set; }
    public string INN { get; set; }
    public string Fullname { get; set; }
    public ContactsUpdateDto Contacts { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public ICollection<ExecutorResponseDto>? ExecutorResponse { get; set; }

    public DetailOrderDto(Order order, User user)
    {
        OrderId = order.Id;
        Name = order.Name;
        Description = order.Description;
        Deadline = order.Deadline;
        Price = order.Price;
        CompanyName = order.CompanyName;
        IsCompleted = order.IsCompleted;
        OrderVacancies = order.OrderVacancies.Select(vacancy => new OrderVacancyDto
        {    
            MaxWorkers = vacancy.MaxWorkers,
            OrderRole = new OrderRoleDto
            {
                Id = vacancy.OrderRole.Id,
                Name = vacancy.OrderRole.Name
            }
        }).ToList();
        OrderCategories = order.OrderCategories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList(); 
        OrderSkills = order.OrderSkills.Select(category => new SkillDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();
        if (user.Customer != null)
            Customer = new CommonCustomerDto
            {
                Addres = user.Customer!.CompanyName,
                Phone = user.Phone,
                Email = user.Email,
                Fullname = $"{user.Surname} {user.Name} {user.SecondName}",

                INN = user.Customer.INN
            };
        else
            Executor = new CommonExecutorDto(user.Executor.Id, user.Username, user.Name,user.Surname,user.SecondName,null,
                new ExecutorSkillsDto(user.Executor), new ExecutorCategoriesDto(user.Executor));
        Address = user.Customer?.Address ?? "";
        INN = user.Customer?.INN ?? "";
        Fullname = $"{user.Surname} {user.Name} {user.SecondName}";
        Phone = user.Phone;
        Email = user.Email;
        Contacts = new ContactsUpdateDto
        {
            Contacts = user.UserContacts?.ContactLinks.Select(x => new ContactDto(x.Name, x.Url)).ToList() ??
                       new List<ContactDto>()
        };


        Notifications = order.OrderVacancies.Where(x=>x.Responses!=null).SelectMany(x => x.Responses).Count(re => re.IsCompleted);
    }
}