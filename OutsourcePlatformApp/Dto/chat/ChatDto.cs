namespace OutsourcePlatformApp.Dto.chat;

public class ChatDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ExecutorId { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverSurname { get; set; }
    public string SenderUsername { get; set; }
    public string SenderName { get; set; }
    public string SenderSurname { get; set; }
    public int? OrderId { get; set; }
    public string ProjectName { get; set; }
    public string CompanyName { get; set; }
    public int? UnreadCount { get; set; } = 0!;
    public string? LastMessageText { get; set; }

    public DateTime CreatedAt { get; set; }

    public ChatDto()
    {
    }

    public ChatDto(int id, int customerId, int executorId, string receiverName, string receiverSurname, string senderUsername,
        string senderName, string senderSurname, int? orderId, string projectName, string companyName, int? unreadCount,
        DateTime createdAt)
    {
        Id = id;
        CustomerId = customerId;
        ExecutorId = executorId;
        ReceiverName = receiverName;
        ReceiverSurname = receiverSurname;
        SenderName = senderName;
        SenderSurname = senderSurname;
        OrderId = orderId;
        ProjectName = projectName;
        CompanyName = companyName;
        UnreadCount = unreadCount;
        CreatedAt = createdAt;
        SenderUsername = senderUsername;
    }
}