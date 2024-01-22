using System.ComponentModel.DataAnnotations;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class DetailExecutorDto
    {
        [DataType(DataType.Date)] public DateTime BirthDate { get; set; }
        public string City { get; set; }
        public List<EducationUpdateDto>? Educations { get; set; }
        public List<CategoryDto>? Categories { get; set; }
        public List<SkillDto>? Skills { get; set; }

        public int CompletedProjectsCount { get; set; } = 0;

        public DetailExecutorDto(Executor executor, int finishProjectCount)
        {
            
            BirthDate = new DateTime(executor.Birthdate.Year, executor.Birthdate.Month, executor.Birthdate.Day);
            City = executor.City;
            if (executor.Educations != null)
            {
                Educations = executor.Educations
                    .Select(ed => new EducationUpdateDto(ed.Id,
                        ed.Place, ed.Speciality, ed.GraduationYear)).ToList();
            }

            CompletedProjectsCount = finishProjectCount;
            if (executor.Skills != null)
                Skills = executor.Skills.Select(skill => new SkillDto { Id = skill.Id, Name = skill.Name }).ToList();
            if (executor.Categories != null)
                Categories = executor.Categories
                    .Select(category => new CategoryDto { Id = category.Id, Name = category.Name }).ToList();
        }
    }
}