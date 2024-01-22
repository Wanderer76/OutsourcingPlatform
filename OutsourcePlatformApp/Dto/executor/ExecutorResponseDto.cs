using System.ComponentModel.DataAnnotations;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class ExecutorResponseDto
{
    public int ExecutorId { get; set; }

    public int ResponseId { get; set; }
    public string Fio { get; set; }
    public bool? IsAccepted { get; set; }
    [DataType(DataType.Date)] public DateOnly Birthdate { get; set; }
    public bool IsResourceUploaded { get; set; }
    public string City { get; set; }

    public int CompletedOrders { get; set; }

    public bool isRated { get; set; }

    public bool isCompleted { get; set; }

    [DataType(DataType.PhoneNumber)] public string Phone { get; set; }
    [DataType(DataType.EmailAddress)] public string Email { get; set; }
    public ContactsUpdateDto Contacts { get; set; }
    public List<SkillDto> Skills { get; set; }
    public OrderVacancyDto OrderVacancy { get; set; }
    public List<CategoryDto> Categories { get; set; }
    public string Username { get; set; }

    public ExecutorResponseDto(){}
    public ExecutorResponseDto(User user, Order order, List<Review> reviews)
    {
        ExecutorId = user.Executor.Id;
        Fio = $"{user.Surname} {user.Name} {user.SecondName}";
        var a = order.OrderVacancies.Where(x => x.Responses != null).SelectMany(x => x.Responses)
            .FirstOrDefault(response => response.ExecutorId == user.Executor.Id);
        IsAccepted = a == null ? false : a.IsAccept;
        Birthdate = user.Executor.Birthdate;
        City = user.Executor.City;
        CompletedOrders = reviews.Count;
        isRated = reviews.Any(review =>
            review.Order.Id == order.Id);
        Phone = user.Phone;
        Email = user.Email;
        Contacts = new ContactsUpdateDto
        {
            Contacts = user.UserContacts == null
                ? null
                : user.UserContacts?.ContactLinks?.Select(x => new ContactDto(x.Name, x.Url)).ToList() ??
                  new List<ContactDto>()
        };
        var response = order.OrderVacancies.SelectMany(x => x.Responses)
            .FirstOrDefault(response => response.ExecutorId == ExecutorId);
        isCompleted = response == null ? false : response.IsCompleted;
        IsResourceUploaded = response == null ? false : response.IsResourceUploaded;
        ResponseId = response == null ? -1 : response.Id;
        OrderVacancy = new OrderVacancyDto
        {
            Id = response == null ?-1:response.OrderVacancyId,
            OrderRole = new OrderRoleDto
                { Id = response==null?-1:response.OrderVacancy.OrderRole.Id, Name = response==null?"":response.OrderVacancy.OrderRole.Name }
        };

        Skills = user.Executor.Skills.Select(skill => new SkillDto
        {
            Id = skill.Id,
            Name = skill.Name
        }).ToList();
        Categories = user.Executor.Categories.Select(skill => new CategoryDto
        {
            Id = skill.Id,
            Name = skill.Name
        }).ToList();
        Username = user.Username;
    }
}