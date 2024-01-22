using System.Collections;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Models.Chat;

namespace OutsourcePlatformApp.Repository;

public interface IChatRepository
{
    //Task<List<ChatRoom>> GetAllExecutorChatRooms(int executorId, int offset,int limit);
    Task<List<ChatRoom>> GetAllUserChatRooms(int userId, int offset,int limit);
    Task<ChatRoom> CreateChatRoom(ChatRoom chatRoom);
    Task<List<Message>?> GetMessages(int chatId, int offset, int limit);
    Task<ChatRoom> GetChatAsync(int chatId);
    Task<ChatRoom> GetConversationAsync(int chatId);
    Task<Message> CreateMessage(Message message);
    Task<User> GetUserByRefreshToken(string refreshToken);
    Task<User> GetUserById(int userId);
    Task<User> GetUserByUsername(string username);
    Task<ChatRoom> SaveChangesAsync(ChatRoom chat);
    Task<List<ChatRoom>> GetCustomerStartedChats(int customerId, int offset, int limit);
    Task<List<ChatRoom>> GetExecutorStartedChats(int executorId, int offset, int limit);
    Task<List<ChatRoom>> GetUserStartedChats(int userId, int offset, int limit);
 //   Task<List<ChatRoom>> GetCustomerNotStartedChats(int customerId, int offset, int limit);
    Task<List<ChatRoom>> GetUserNotStartedChats(int userId, int offset, int limit);
   // Task<List<ChatRoom>> GetExecutorNotStartedChats(int executorId, int offset, int limit);
    Task<Message?> SetMessageViewed(int messageId, int userId);
    Task<Message?> GetLastMessage(int chatId);
    Task<int?> GetUnreadCount(int chatId, int userId);
    Task<bool> HasChatRoomAsync(int user1, int user2);
    Task<ChatRoom> GetUsersChatRoom(int user1, int user2);
    Task<ChatRoom> GetChatByIdAsync(int chatId);
    Task<List<Message>> SetAllMessagesViewed(int chatId, int receiverId);
}