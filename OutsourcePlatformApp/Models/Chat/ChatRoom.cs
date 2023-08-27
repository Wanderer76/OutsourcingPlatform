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
    public int CustomerId { get; set; }
    public int ExecutorId { get; set; }
    public int? OrderId { get; set; }
    public string ProjectName { get; set; } = null!;
    public string CompanyName { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public ICollection<Message>? Messages { get; set; }

    public ChatRoom()
    {
    }

    public ChatRoom(string roomName, int customerId, int executorId, int? orderId, string projectName, string companyName)
    {
        RoomName = roomName;
        CustomerId = customerId;
        ExecutorId = executorId;
        OrderId = orderId;
        ProjectName = projectName;
        CompanyName = companyName;
    }
}