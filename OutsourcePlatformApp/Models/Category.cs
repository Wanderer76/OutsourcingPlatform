using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("Categories")]
    public class Category: BaseEntity
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public ICollection<Executor> Executors { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Category()
        {
            
        }
        
        public Category(string name)
        {
            Name = name;
        }
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        [MaxLength(50)] public string Name { get; set; }
    }
}