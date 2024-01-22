using System.ComponentModel.DataAnnotations;

namespace OutsourcePlatformApp.Dto
{
    public class EducationUpdateDto
    {
        public int? Id { get; set; }
        public string Place { get; set; }
        public string Speciality { get; set; }
        public int GraduationYear { get; set; }

        public EducationUpdateDto(int? id, string place, string speciality, int graduationYear)
        {
            Id = id;
            Place = place;
            Speciality = speciality;
            GraduationYear = graduationYear;
        }
    }
}