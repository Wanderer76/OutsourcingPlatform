using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OutsourcePlatformApp.Models.Chat;
using Shared.Entities;

namespace OutsourcePlatformApp.Models
{
    [Table("Users")]
    public class User : BaseEntity
    {
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
        public bool IsVerified { get; set; } = false;
        public List<ActivationToken> ActivationTokens { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public Contact? UserContacts { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public List<Order> Orders { get; set; }
        //public List<ChatRoom>? ChatRooms { get; set; } = new();

    }
}