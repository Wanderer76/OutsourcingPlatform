namespace NotificationApp.Models;

public class ChatNotificationModel
{
    public int ChatId { get; set; }
    public int ReceiverId { get; set; }
    public string ProjectName { get; set; }
    public string CompanyName { get; set; }
    public int MessageId { get; set; }
    public string Message { get; set; }
}