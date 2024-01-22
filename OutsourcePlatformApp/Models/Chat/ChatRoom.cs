using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OutsourcePlatformApp.Models.Chat;

[Table("ChatRooms")]
public class ChatRoom
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [JsonIgnore]
    public string RoomName { get; set; }
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    public int? OrderId { get; set; }
    public string ProjectName { get; set; } = null!;
    public string CompanyName { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public ICollection<Message>? Messages { get; set; }

    public ChatRoom()
    {
    }

    public ChatRoom(string roomName, int user1Id, int user2Id, int? orderId, string projectName, string companyName)
    {
        RoomName = roomName;
        User1Id = user1Id;
        User2Id = user2Id;
        OrderId = orderId;
        ProjectName = projectName;
        CompanyName = companyName;
    }
}