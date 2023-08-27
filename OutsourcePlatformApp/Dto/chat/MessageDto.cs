namespace OutsourcePlatformApp.Dto.chat;

public class MessageDto
{
    public int Id { get; set; }
    
    public int FromUserId { get; set; }
    public string SenderName { get; set; }
    public string SenderSurname { get; set; }

    public string MessageText { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public bool IsViewed { get; set; } = false;
    
    public MessageDto()
    {
    }

    public MessageDto(int id, int fromUserId, string senderName, string senderSurname, string messageText, DateTime createdAt, bool isViewed)
    {
        Id = id;
        FromUserId = fromUserId;
        SenderName = senderName;
        SenderSurname = senderSurname;
        MessageText = messageText;
        CreatedAt = createdAt;
        IsViewed = isViewed;
    }
}