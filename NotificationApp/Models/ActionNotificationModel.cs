namespace NotificationApp.Models;

public class ActionNotificationModel
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public int OrderId { get; set; }
    public string ProjectName { get; set; }
    public string CompanyName { get; set; }
    public string UesrToken { get; set; }
    public string Message { get; set; }
}