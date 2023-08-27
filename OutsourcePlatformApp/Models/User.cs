using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OutsourcePlatformApp.Models.Chat;

namespace OutsourcePlatformApp.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? SecondName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public bool IsBanned { get; set; }
        public string BannedMessage { get; set; } = string.Empty;
        public Customer? Customer { get; set; }
        public Executor? Executor { get; set; }
        public Admin? Admin { get; set; }

        public RefreshToken RefreshToken { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public Contact? UserContacts { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}