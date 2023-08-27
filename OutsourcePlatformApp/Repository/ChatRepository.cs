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
    public async Task<bool> HasChatRoomAsync(int customerId, int executorId)
    {
        return await DbContext.ChatRooms
            .Where(room => room.CustomerId == customerId && room.ExecutorId == executorId)
            .AnyAsync();
    }
    public async Task<List<ChatRoom>> GetAllExecutorChatRooms(int executorId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.ExecutorId == executorId)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
    public async Task<List<ChatRoom>> GetAllCustomerChatRooms(int customerId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.CustomerId == customerId)
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
            .Where(chatRoom => chatRoom.CustomerId == customerId && chatRoom.Messages!.Any())
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<ChatRoom>> GetExecutorStartedChats(int executorId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.ExecutorId == executorId && chatRoom.Messages!.Any())
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<ChatRoom>> GetCustomerNotStartedChats(int customerId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.CustomerId == customerId && chatRoom.Messages!.Count == 0)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<ChatRoom>> GetExecutorNotStartedChats(int executorId, int offset, int limit)
    {
        return await DbContext.ChatRooms
            .Where(chatRoom => chatRoom.ExecutorId == executorId && chatRoom.Messages!.Count == 0)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task<Message?> SetMessageViewed(int messageId, int userId)
    {
        var message = await DbContext.Messages
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
            .Include(user=> user.UserRoles)
            .Include(user=> user.Executor!)
            .Include(user=> user.Customer!)
            .Include(user=> user.Admin!)
            .Include(user=> user.Customer!.ChatRooms)
            .Include(user=> user.Executor!.ChatRooms)
            .Include(user=> user.Admin!.ChatRooms)
            .Where(user => user.RefreshToken.Token == refreshToken)
            .FirstAsync();
    }
    
    public async Task<User> GetUserById(int userId)
    {
        return await DbContext.Users
            .Include(user=> user.Executor!)
            .Include(user=> user.Customer!)
            .Include(user=> user.Customer!.ChatRooms)
            .Include(user=> user.Executor!.ChatRooms)
            .Where(user => user.Id == userId)
            .FirstAsync();
    }
}