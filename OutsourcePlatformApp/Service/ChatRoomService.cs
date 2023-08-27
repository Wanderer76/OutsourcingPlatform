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

    public ChatRoomService(ICustomerRepository customerRepository, IExecutorRepository executorRepository, IUserRepository userRepository, IOrderRepository orderRepository, IChatRepository chatRepository, IAdminRepository adminRepository)
    {
        this.customerRepository = customerRepository;
        this.executorRepository = executorRepository;
        this.userRepository = userRepository;
        this.orderRepository = orderRepository;
        this.chatRepository = chatRepository;
        this.adminRepository = adminRepository;
    }

    public async Task CreateChatRoom(Customer customer, Executor executor, int orderId)
    {
        try
        {
            var hasChat = await chatRepository.HasChatRoomAsync(customer.CustomerId, executor.ExecutorId);
            if (!hasChat)
            {
                var order = await orderRepository.GetOrderByIdAsync(orderId);
                var admin = await adminRepository.GetAdminAsync();
                var chatRoom = new ChatRoom("chatroom" + customer.CustomerId + executor.ExecutorId + order.OrderId,
                    customer.CustomerId, executor.ExecutorId, orderId,
                    order.Name, order.CompanyName);
                chatRoom.Messages = new List<Message>();
                await chatRepository.CreateChatRoom(chatRoom);
           //     admin.ChatRooms!.Add(chatRoom);
                customer.ChatRooms!.Add(chatRoom);
                executor.ChatRooms!.Add(chatRoom);
           //     await adminRepository.UpdateAdminAsync(admin);
                await customerRepository.UpdateCustomerAsync(customer);
                await executorRepository.UpdateExecutorAsync(executor);
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
        var customer = await userRepository.GetUserByCustomerIdAsync(chat.CustomerId);
        var executor = await userRepository.GetUserByExecutorIdAsync(chat.ExecutorId);
        var unread = await chatRepository.GetUnreadCount(chat.Id, executor.Id) ?? 0;
        var last = await chatRepository.GetLastMessage(chat.Id) ?? null;
        var chatForFullstack = new ChatDto(chat.Id, chat.CustomerId,
            chat.ExecutorId, executor.Name,
            executor.Surname, customer.Name, customer.Surname, chat.OrderId, chat.ProjectName,
            chat.CompanyName, unread, chat.CreatedAt);
        if (last == null)
            return await Task.Run(() => chatForFullstack);
        chatForFullstack.LastMessageText = last.MessageText;
        return chatForFullstack;
    }

    public async Task<ChatDto> GetChatInfoForExecutor(ChatRoom chat)
    {
        var customer = await userRepository.GetUserByCustomerIdAsync(chat.CustomerId);
        var executor = await userRepository.GetUserByExecutorIdAsync(chat.ExecutorId);
        var unread = await chatRepository.GetUnreadCount(chat.Id, customer.Id) ?? 0;
        var last = await chatRepository.GetLastMessage(chat.Id) ?? null;
        var chatForFullstack = new ChatDto(chat.Id, chat.CustomerId,
            chat.ExecutorId, customer.Name, customer.Surname, executor.Name,
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
            user.Surname, message.MessageText, message.CreatedAt, message.IsViewed, message.Chat.Id, user.Username);
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
}