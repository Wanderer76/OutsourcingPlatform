using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class CompetenciesDto
{
    public List<SkillDto>? Skills { get; set; }
    public List<CategoryDto>? Categories { get; set; }

    public CompetenciesDto()
    {
    }

    public CompetenciesDto(List<SkillDto>? skills, List<CategoryDto>? categories)
    {
        Skills = skills;
        Categories = categories;
    }
}