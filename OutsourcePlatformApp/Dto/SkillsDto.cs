using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class SkillsDto
    {
        public List<SkillDto>? Skills { get; set; }

        public SkillsDto(Executor executor)
        {
            if(executor.Skills != null)
                Skills = executor.Skills.Select(skill => new SkillDto{Id = skill.Id, Name = skill.Name}).ToList();
        }
    }
}