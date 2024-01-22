using OutsourcePlatformApp.Dto.chat;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Models.Chat;

namespace OutsourcePlatformApp.Hubs.Chat;

public interface IChatHub
{
    Task ReceiveAllChatRooms(string userToken, int offset = 0, int limit = 10);
    Task ReceiveChatRooms(List<ChatDto> chatRooms);
    Task ReceiveStartedChatRooms(List<ChatDto> userChatRooms);
    Task ReceiveNotStartedChatRooms(List<ChatDto> userChatRooms);
    Task GetChatRoom(string userToken, int chatRoomId, int offset, int limit);
    Task SendChatRoom(ChatDto chat, List<MessageObjectDto> messages);
    Task SendChats(List<ChatRoom> chats);
    Task SendAsync(MessageObjectDto senderName);

    Task Send(string message, string userToken, int chatId);
    Task SendMessage(string name, Message message, string username);
    Task ReceiveMessages(string userToken, int chatRoomId, int offset = 0, int limit = 20);
    Task SendMessages(List<MessageObjectDto> messages);
    Task SetMessageViewed(int messageId, string refreshToken);
    Task SetAllMessagesViewed(int chatId, string refreshToken);
    Task MarkMessage(MessageObjectDto message);
    Task SearchChats(string userToken, string word, int offset = 0, int limit = 10);
    Task ReceiveFoundChats(List<ChatDto> chats);
    Task SendMessageNotFound(string error);
    
    // ADMIN
    Task ReceiveAllUserChatRooms(string userToken, int userId, int offset = 0, int limit = 10);
    Task ReceiveUserChatRooms(List<ChatDto> chatRooms);
    Task GetUserStartedChats(string userToken, int userId, int offset = 0, int limit = 10);
    Task ReceiveUserStartedChatRooms(List<ChatDto> chatRooms);
    Task GetUserNotStartedChats(string userToken, int userId, int offset = 0, int limit = 10);
    Task ReceiveUserNotStartedChatRooms(List<ChatDto> chatRooms);
    Task GetUserChatRoom(string adminToken, int userId, int chatRoomId, int offset, int limit);
    Task SendUserChatRoom(ChatDto chatDtos, List<MessageObjectDto> messagesForFullstack);
    Task AddAdminToUserChats(int userId);
    Task AddAdminToChat(int chatId);
    Task AddAdminToGroup(Admin admin, ChatRoom chat);
    Task RemoveAdminFromUserChats(int userId);
    Task RemoveAdminFromChat(int chatId);
    Task RemoveAdminFromGroup(Admin admin, ChatRoom chat);
    Task ReceiveUserMessages(string adminToken, int chatId, int offset = 0, int limit = 10);
    Task SendUserMessages(List<MessageObjectDto> messages);
    Task SearchUserChats(string adminToken, int userId, string word, int offset = 0, int limit = 10);
    Task ReceiveUserFoundChats(List<ChatDto> chats);
}