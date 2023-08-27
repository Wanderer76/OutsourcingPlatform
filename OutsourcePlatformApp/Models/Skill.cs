
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutsourcePlatformApp.Models
{
    [Table("Skills")]
    public class Skill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
