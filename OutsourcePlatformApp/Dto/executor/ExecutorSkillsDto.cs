using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto;

public class ExecutorSkillsDto
{
    public List<SkillDto>? Skills { get; set; }

    public ExecutorSkillsDto(Executor executor)
    {
        if (executor.Skills != null)
            Skills = executor.Skills.Select(skill => new SkillDto{ Id = skill.Id, Name = skill.Name }).ToList();
    }
}