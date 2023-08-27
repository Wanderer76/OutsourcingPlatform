namespace OutsourcePlatformApp.Dto.chat;

public class MessageObjectDto : MessageDto
{
    public int chatId { get; set; }
    public string senderUsername { get; set; }

    public MessageObjectDto(int id, int fromUserId, string senderName, string senderSurname,
        string messageText, DateTime createdAt,
        bool isViewed, int chatId, string senderUsername) 
        : base(id, fromUserId, senderName, senderSurname, messageText, createdAt, isViewed)
    {
        this.chatId = chatId;
        this.senderUsername = senderUsername;
    }
}