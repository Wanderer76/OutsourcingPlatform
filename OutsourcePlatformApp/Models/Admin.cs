using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OutsourcePlatformApp.Models.Chat;
using Shared.Entities;

namespace OutsourcePlatformApp.Models;

[Table("Admin")]
public class Admin : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public List<ChatRoom>? ChatRooms { get; set; } = new();
}