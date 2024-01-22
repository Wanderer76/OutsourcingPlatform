using OutsourcePlatformApp.Dto.chat;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Models.Chat;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Service;

public class ChatRoomService
{
    private readonly ICustomerRepository customerRepository;
    private readonly IExecutorRepository executorRepository;
    private readonly IUserRepository userRepository;
    private readonly IOrderRepository orderRepository;
    private readonly IChatRepository chatRepository;
    private readonly IAdminRepository adminRepository;

    public ChatRoomService(ICustomerRepository customerRepository, IExecutorRepository executorRepository,
        IUserRepository userRepository, IOrderRepository orderRepository, IChatRepository chatRepository,
        IAdminRepository adminRepository)
    {
        this.customerRepository = customerRepository;
        this.executorRepository = executorRepository;
        this.userRepository = userRepository;
        this.orderRepository = orderRepository;
        this.chatRepository = chatRepository;
        this.adminRepository = adminRepository;
    }

    public async Task<ChatRoom> CreateChatRoom(User user1, User user2, int orderId)
    {
        try
        {
            var hasChat = await chatRepository.HasChatRoomAsync(user1.Id, user2.Id);
            if (!hasChat)
            {
                var order = await orderRepository.GetOrderByIdAsync(orderId);
                var admin = await adminRepository.GetAdminAsync();
                var chatRoom = new ChatRoom("chatroom" + user2.Id + user2.Id + order?.Id,
                    user1.Id, user2.Id, orderId,
                    order.Name, order.CompanyName);
                chatRoom.Messages = new List<Message>();
                await chatRepository.CreateChatRoom(chatRoom);
                //     admin.ChatRooms!.Add(chatRoom);
                // user1.ChatRooms!.Add(chatRoom);
                // user2.ChatRooms!.Add(chatRoom);
                // await userRepository.UpdateUserAsync(user1);
                // await userRepository.UpdateUserAsync(user2);
                //     await adminRepository.UpdateAdminAsync(admin);
                //await customerRepository.UpdateCustomerAsync(customer);
                //await executorRepository.UpdateExecutorAsync(executor);
                return await chatRepository.GetUsersChatRoom(user1.Id, user2.Id);
            }
            else
            {
                return await chatRepository.GetUsersChatRoom(user1.Id, user2.Id);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ChatDto> GetChatInfoForCustomer(ChatRoom chat)
    {
        var customer = await userRepository.GetUserByIdAsync(chat.User1Id);
        var executor = await userRepository.GetUserByIdAsync(chat.User2Id);
        var unread = await chatRepository.GetUnreadCount(chat.Id, executor.Id) ?? 0;
        var last = await chatRepository.GetLastMessage(chat.Id) ?? null;
        var chatForFullstack = new ChatDto(chat.Id, chat.User1Id,
            chat.User2Id, executor.Name,
            executor.Surname, executor.Username, customer.Name, customer.Surname, chat.OrderId, chat.ProjectName,
            chat.CompanyName, unread, chat.CreatedAt);
        if (last == null)
            return await Task.Run(() => chatForFullstack);
        chatForFullstack.LastMessageText = last.MessageText;
        return chatForFullstack;
    }

    public async Task<ChatDto> GetChatInfoForExecutor(ChatRoom chat)
    {
        var customer = await userRepository.GetUserByIdAsync(chat.User1Id);
        var executor = await userRepository.GetUserByIdAsync(chat.User2Id);
        var unread = await chatRepository.GetUnreadCount(chat.Id, customer.Id) ?? 0;
        var last = await chatRepository.GetLastMessage(chat.Id) ?? null;
        var chatForFullstack = new ChatDto(chat.Id, chat.User1Id,
            chat.User2Id, customer.Name, customer.Surname, customer.Username, executor.Name,
            executor.Surname, chat.OrderId, chat.ProjectName,
            chat.CompanyName, unread, chat.CreatedAt);
        if (last == null)
            return await Task.Run(() => chatForFullstack);
        chatForFullstack.LastMessageText = last.MessageText;
        return chatForFullstack;
    }

    public async Task<List<ChatDto>> GetCustomerChatsInfo(List<ChatRoom> chats)
    {
        var chatsForFullstack = new List<ChatDto>();
        foreach (var chat in chats)
        {
            chatsForFullstack.Add(await GetChatInfoForCustomer(chat));
        }

        return chatsForFullstack;
    }

    public async Task<List<ChatDto>> GetExecutorChatsInfo(List<ChatRoom> chats)
    {
        var chatsForFullstack = new List<ChatDto>();
        foreach (var chat in chats)
        {
            chatsForFullstack.Add(await GetChatInfoForExecutor(chat));
        }

        return chatsForFullstack;
    }

    public async Task<Message> CreateMessage(int fromUserId, int chatId, string text)
    {
        try
        {
            var chat = await chatRepository.GetConversationAsync(chatId);
            var message = new Message(fromUserId, chat, text);
            chat.Messages!.Add(message);
            await chatRepository.CreateMessage(message);
            await chatRepository.SaveChangesAsync(chat);

            return message;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<ChatDto> GetIntersectedChats(string word, List<ChatDto> chats)
    {
        return chats
            .Where(chat => chat.ReceiverName.Contains(word, StringComparison.OrdinalIgnoreCase)
                           || chat.ReceiverSurname.Contains(word, StringComparison.OrdinalIgnoreCase)
                           || word.Contains(chat.ReceiverName, StringComparison.OrdinalIgnoreCase)
                           || word.Contains(chat.ReceiverSurname, StringComparison.CurrentCultureIgnoreCase))
            .ToList();
    }

    public async Task<MessageDto> GetMessageInfo(Message message)
    {
        var user = await userRepository.GetUserByIdAsync(message.FromUserId);
        var dto = new MessageDto(message.Id, message.FromUserId, user.Name,
            user.Surname, message.MessageText, message.CreatedAt, message.IsViewed);
        return dto;
    }

    public async Task<MessageObjectDto> GetMessageObjectInfo(Message message)
    {
        var user = await userRepository.GetUserByIdAsync(message.FromUserId);
        var dto = new MessageObjectDto(message.Id, message.FromUserId, user.Name,
            user.Surname, message.MessageText, message.CreatedAt, message.IsViewed, message.ChatId, user.Username);
        return dto;
    }

    public async Task<List<MessageObjectDto>> GetMessagesInfo(List<Message> messages)
    {
        var messagesForFullstack = new List<MessageObjectDto>();
        foreach (var message in messages)
        {
            messagesForFullstack.Add(await GetMessageObjectInfo(message));
        }

        return messagesForFullstack;
    }

    public async Task<ChatRoom> GetUserChat(User user, int receiverId)
    {
        return (await chatRepository.GetAllUserChatRooms(user.Id, 0, int.MaxValue))
            .First(x => x.User2Id == receiverId);
        // if (user.Customer != null)
        //     return (await chatRepository.GetAllCustomerChatRooms(user.Customer.Id, 0, int.MaxValue))
        //         .First(x => x.User2Id == receiverId);
        // return (await chatRepository.GetAllCustomerChatRooms(user.Executor.Id, 0, int.MaxValue))
        //     .First(x => x.User2Id == receiverId);
    }
}