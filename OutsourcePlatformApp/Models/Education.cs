using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutsourcePlatformApp.Models
{
    [Table("Educations")]
    public class Education
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EducationId { get; set; }
        public string Place { get; set; }
        public string Speciality { get; set; }
        public int GraduationYear { get; set; }
        public Executor Executor { get; set; }

        public Education(string place, string speciality, int graduationYear)
        {
            Place = place;
            Speciality = speciality;
            GraduationYear = graduationYear;
        }
    }
}
