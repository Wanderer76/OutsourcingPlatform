using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("Educations")]
    public class Education : BaseEntity
    {
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