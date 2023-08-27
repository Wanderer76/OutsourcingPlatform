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

    public string City { get; set; }

    public int CompletedOrders { get; set; }

    public bool isRated { get; set; }

    public bool isCompleted { get; set; }

    [DataType(DataType.PhoneNumber)] public string Phone { get; set; }
    [DataType(DataType.EmailAddress)] public string Email { get; set; }

    public string? VkNickname { get; set; }
    public string? GithubUrl { get; set; }
    public string? Messager { get; set; }

    public ContactsUpdateDto Contacts { get; set; }
    public List<SkillDto> Skills { get; set; }

    public List<CategoryDto> Categories { get; set; }
    public string Username { get; set; }

    public ExecutorResponseDto(User user, Order order, List<Review> reviews)
    {
        ExecutorId = user.Executor.ExecutorId;
        Fio = $"{user.Surname} {user.Name} {user.SecondName}";
        IsAccepted = order.Responses.First(response => response.ExecutorId == user.Executor.ExecutorId).IsAccept;
        Birthdate = user.Executor.Birthdate;
        City = user.Executor.City;
        CompletedOrders = reviews.Count;
        isRated = reviews.Any(review =>
            review.Order.OrderId == order.OrderId);
        Phone = user.Phone;
        Email = user.Email;
        Contacts = new ContactsUpdateDto
        {
            VkNickname = user.UserContacts == null ? "" : user.UserContacts.VkNickname ?? "",
            GithubUrl = user.UserContacts == null ? "" : user.UserContacts.GithubUrl ?? "",
            Messager = user.UserContacts == null ? "" : user.UserContacts.Messager ?? ""
        };
        var response = order.Responses.First(response => response.ExecutorId == ExecutorId);
        isCompleted = response.IsCompleted;
        ResponseId = response.Id;
       
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