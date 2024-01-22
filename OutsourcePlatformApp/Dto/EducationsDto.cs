using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Dto
{
    public class EducationsDto
    {
        public List<EducationUpdateDto>? Educations { get; set; }

        public EducationsDto(Executor executor)
        {
            if(executor.Educations != null)
                Educations = executor.Educations
                    .Select(ed => new EducationUpdateDto(ed.Id, ed.Place, ed.Speciality, ed.GraduationYear))
                    .ToList();
        }
    }
}