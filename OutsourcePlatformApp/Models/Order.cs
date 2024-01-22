using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("Orders")]
    public class Order : BaseEntity
    {
        public int Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly Deadline { get; set; }
        public string CompanyName { get; set; }
        public User? Creator { get; set; }
        public bool IsPublished { get; set; }
        public bool IsCompleted { get; set; }
        public List<Category>? OrderCategories { get; set; }
        public List<Skill>? OrderSkills { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<OrderVacancy>? OrderVacancies { get; set; }
    }
}