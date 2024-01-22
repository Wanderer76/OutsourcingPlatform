using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("Reviewies")]
    public class Review : BaseEntity
    {
        [Required]
        public User User { get; set; }
        [Required]
        public Order Order { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        [MaxLength(int.MaxValue)]
        public string Description { get; set; }

    }
}
