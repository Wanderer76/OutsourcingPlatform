using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Models.Chat;

namespace OutsourcePlatformApp.Repository;

public class ChatRepository : IChatRepository
{
    private readonly ApplicationDbContext DbContext;
    private readonly IUserRepository userRepository;

    public ChatRepository(ApplicationDbContext dbContext, IUserRepository userRepository)
    {
        DbContext = dbContext;
        this.userRepository = userRepository;
    }

    public async Task<Message?> GetLastMessage(int chatId)
    {
        return await DbContext.Messages
            .Where(message => message.Chat.Id == chatId)
            .OrderByDescending(message => message.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<int?> GetUnreadCount(int chatId, int userId)
    {
        return await DbContext.Messages
            .Where(message => message.Chat.Id == chatId
                              && message.IsViewed == false
                              && message.FromUserId == userId)
            .CountAsync();
    }

    public async Task<bool> HasChatRoomAsync(int user1, int user2)
    {
        return await DbContext.ChatRooms
            .Where(room => (room.User1Id == user1 && room.User2Id == user2) ||
                           room.User1Id == user2 && room.User2Id == user1)
            .AnyAsync();
    }

    public async Task<ChatRoom> GetUsersChatRoom(int user1, int user2)
    {
        return await DbContext.ChatRooms
            .Where(room => (room.User1Id == user1 && room.User2Id == user2) ||
                           (room.User1Id == user2 && room.User2Id == user1))
            .FirstAsync();
    }

    public async Task<ChatRoom> GetChatByIdAsync(int chatId)
    {
        return await DbContext.ChatRooms.FirstAsync(x => x.Id == chatId);
    }

    public async Task<List<Message>> SetAllMessagesViewed(int chatId, int receiverId)
    {
        var messages = await DbContext.Messages
            .Include(x => x.Chat)
            .Where(message => message.ChatId == chatId && message.FromUserId != receiverId&&message.IsViewed==false)
            .ToListAsync();
        messages.ForEach(x => x.IsViewed = true);
        await DbContext.SaveChangesAsync();
        return messages;
    }

// public async Task<List<ChatRoom>> GetAllExecutorChatRooms(int executorId, int offset, int limit)
// {
//     return await DbContext.ChatRooms
//         .Where(chatRoom => chatRoom.User2Id == executorId)
//         .Skip(offset)
//         .Take(limit)
//         .ToListAsync();
// }
    public async Task<List<ChatRoom>> GetAllUserChatRooms(int userId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.User1Id == userId || chatRoom.User2Id == userId)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<ChatRoom> CreateChatRoom(ChatRoom chatRoom)
    {
        DbContext.ChatRooms.Add(chatRoom);
        await DbContext.SaveChangesAsync();
        return chatRoom;
    }

    public async Task<Message> CreateMessage(Message message)
    {
        DbContext.Messages.Add(message);
        await DbContext.SaveChangesAsync();
        return message;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await DbContext.Users
            .Include(user => user.RefreshToken)
            .Include(user => user.UserRoles)
            .Include(user => user.Executor!)
            .Include(user => user.Customer!)
            .Include(user => user.Admin!)

            //.Include(user=> user.Executor!.ChatRooms)
            .Include(user => user.Admin!.ChatRooms)
            .Where(user => user.Username == username)
            .FirstAsync();
    }

    public async Task<ChatRoom> SaveChangesAsync(ChatRoom chat)
    {
        DbContext.ChatRooms.Update(chat);
        await DbContext.SaveChangesAsync();
        return chat;
    }

    public async Task<List<Message>?> GetMessages(int chatId, int offset, int limit)
    {
        return await DbContext.Messages
            .Include(message => message.Chat)
            .Where(message => message.Chat.Id == chatId)
            .OrderByDescending(message => message.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<ChatRoom>> GetCustomerStartedChats(int customerId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.User1Id == customerId && chatRoom.Messages!.Any())
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<ChatRoom>> GetUserStartedChats(int userId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => (chatRoom.User1Id == userId || chatRoom.User2Id == userId) && chatRoom.Messages!.Any())
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<ChatRoom>> GetExecutorStartedChats(int executorId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.User2Id == executorId && chatRoom.Messages!.Any())
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<ChatRoom>> GetUserNotStartedChats(int userId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom =>
                (chatRoom.User1Id == userId || chatRoom.User2Id == userId) && chatRoom.Messages!.Count == 0)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<ChatRoom>> GetExecutorNotStartedChats(int executorId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.User2Id == executorId && chatRoom.Messages!.Count == 0)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Message?> SetMessageViewed(int messageId, int userId)
    {
        var message = await DbContext.Messages
            .Include(x => x.Chat)
            .FirstOrDefaultAsync(message => message.Id == messageId);
        if (message == null)
            throw new ArgumentException($"message with id = {messageId} doesn't exist");
        if (message.FromUserId != userId)
        {
            message.IsViewed = true;
            await DbContext.SaveChangesAsync();
        }

        return message;
    }

    public async Task<ChatRoom> GetConversationAsync(int chatId)
    {
        return await DbContext.ChatRooms
            .Include(chat => chat.Messages)
            .Where(chatRoom => chatRoom.Id == chatId)
            .FirstAsync();
    }

    public async Task<ChatRoom> GetChatAsync(int chatId)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.Id == chatId)
            .FirstAsync();
    }

    public async Task<User> GetUserByRefreshToken(string refreshToken)
    {
        return await DbContext.Users
            .Include(user => user.RefreshToken)
            .Include(user => user.UserRoles)
            .Include(user => user.Executor!)
            .Include(user => user.Customer!)
            .Include(user => user.Admin!)
            //.Include(user=> user.Executor!.ChatRooms)
            .Include(user => user.Admin!.ChatRooms)
            .Where(user => user.RefreshToken.Token == refreshToken)
            .FirstAsync();
    }

    public async Task<User> GetUserById(int userId)
    {
        return await DbContext.Users
            .Include(user => user.Executor!)
            .Include(user => user.Customer!)
            // .Include(user=> user.Executor!.ChatRooms)
            .Where(user => user.Id == userId)
            .FirstAsync();
    }
}