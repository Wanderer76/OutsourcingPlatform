using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class OrderSkillsDto
{
    public List<SkillDto>? Skills { get; set; }

    public OrderSkillsDto(Order order)
    {
        if (order.OrderVacancies != null)
            Skills = order.OrderSkills
                .Select(skill => new SkillDto { Id = skill.Id, Name = skill.Name }).ToList();
    }
}