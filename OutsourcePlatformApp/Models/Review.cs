using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutsourcePlatformApp.Models
{
    [Table("Reviewies")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
