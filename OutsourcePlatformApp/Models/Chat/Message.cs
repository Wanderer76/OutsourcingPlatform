using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OutsourcePlatformApp.Models.Chat;

[Table("Messages")]
public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int FromUserId { get; set; }
    [JsonIgnore] public ChatRoom Chat { get; set; }
    public int ChatId { get; set; }
    public string MessageText { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsViewed { get; set; } = false;

    public Message()
    {
    }

    public Message(int fromUserId, ChatRoom chat, string messageText)
    {
        FromUserId = fromUserId;
        Chat = chat;
        MessageText = messageText;
    }
}