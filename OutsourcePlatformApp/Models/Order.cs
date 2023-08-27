using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OutsourcePlatformApp.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public int Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxWorkers { get; set; }
        public DateOnly Deadline { get; set; }
        public string CompanyName { get; set; }
        public Customer Customer { get; set; }

        public bool IsPublished { get; set; }
        public bool IsCompleted { get; set; }
        public List<Skill>? OrderSkills { get; set; }
        public List<Category>? OrderCategories { get; set; }
        public ICollection<Response>? Responses { get; set; }
        public ICollection<Review>? Reviews { get; set; }

        public Order()
        {
            
        }
    }
}