using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class CommonOrderInfoDto
{
    public int OrderId { get; set; }
    public int Price { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsResponded { get; set; }
    public bool IsCompleted { get; set; }
    public string Address { get; set; }
    public string INN { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Vk { get; set; }
    public string Messanger { get; set; }

    public string Nickname { get; set; }

    public string CompanyName { get; set; }
    public DateOnly Deadline { get; set; }

    public ContactsUpdateDto Contacts { get; set; }
    public List<CategoryDto>? OrderCategories { get; set; }
    public List<SkillDto>? OrderSkills { get; set; }
    public List<OrderVacancyDto> OrderVacancies { get; set; }
}