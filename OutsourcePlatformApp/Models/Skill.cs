
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("Skills")]
    public class Skill : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Executor> Executors { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Skill()
        {
            
        }
        public Skill(string name)
        {
            Name = name;
        }
    }

    public class SkillDto
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
