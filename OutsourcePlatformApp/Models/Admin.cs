using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OutsourcePlatformApp.Models.Chat;

namespace OutsourcePlatformApp.Models;

[Table("Admin")]
public class Admin
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AdminId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public List<ChatRoom>? ChatRooms { get; set; } = new();
}