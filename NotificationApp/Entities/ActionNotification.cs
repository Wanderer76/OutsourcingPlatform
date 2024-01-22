using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NotificationApp.Models;
using Shared.Entities;

namespace NotificationApp.Entities;

public class ActionNotification : BaseEntity
{
    public int ReceiverId { get; set; }
    public int SenderId { get; set; }
    public int? OrderId { get; set; }
    public string Message { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    [JsonIgnore] public int? MessageId { get; set; }
    public int? ChatId { get; set; }
    public virtual string Type { get; set; } = NotificationType.Action.ToString();
    public bool IsViewed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}