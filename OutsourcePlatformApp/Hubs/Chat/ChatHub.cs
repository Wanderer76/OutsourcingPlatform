using Microsoft.AspNetCore.SignalR;
using OutsourcePlatformApp.Dto.chat;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Models.Chat;
using OutsourcePlatformApp.Repository;
using OutsourcePlatformApp.Service;

namespace OutsourcePlatformApp.Hubs.Chat;

public class ChatHub : Hub<IChatHub>
{
    private readonly IChatRepository chatRepository;
    private readonly ChatManager ChatManager;
    private readonly ChatRoomService chatService;
    private readonly IUserRepository userRepository;
    private readonly IAdminRepository adminRepository;
    private readonly ActionNotificationService notificationService;

    public ChatHub(IChatRepository chatRepository, ChatManager chatManager, ChatRoomService chatService, IUserRepository userRepository, IAdminRepository adminRepository, ActionNotificationService notificationService)
    {
        this.chatRepository = chatRepository;
        ChatManager = chatManager;
        this.chatService = chatService;
        this.userRepository = userRepository;
        this.adminRepository = adminRepository;
        this.notificationService = notificationService;
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        Context.GetHttpContext()!.Request.Query.TryGetValue("token", out var value);
        var token = value.First().Replace(' ', '+');
        try
        {
            ChatManager.ConnectUser(token, connectionId);
            var user = await chatRepository.GetUserByRefreshToken(token);
            if (user.Customer != null)
            {
                foreach (var item in user.Customer.ChatRooms!)
                {
                    await AddToGroup(item.RoomName);
                }
            }
            else if (user.Executor != null)
            {
                foreach (var item in user.Executor!.ChatRooms!)
                {
                    await AddToGroup(item.RoomName);
                }
            }
            else
            {
                foreach (var item in user.Admin!.ChatRooms!)
                {
                    await AddToGroup(item.RoomName);
                }
            }

            await base.OnConnectedAsync();
        }
        catch (Exception e)
        {
            await OnDisconnectedAsync(null);
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        ChatManager.DisconnectUser(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    private async Task AddAdminToGroup(Admin admin, ChatRoom chat)
    {
        if (!admin.ChatRooms!.Contains(chat))
        {
            admin.ChatRooms!.Add(chat);
            await adminRepository.UpdateAdminAsync(admin);
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.RoomName);
        }
    }

    public async Task AddAdminToChat(int chatId)
    {
        var admin = await adminRepository.GetAdminAsync();
        var chatRoom = await chatRepository.GetChatAsync(chatId);
        await AddAdminToGroup(admin, chatRoom);
    }
    
    public async Task AddAdminToUserChats(int userId)
    {
        var admin = await adminRepository.GetAdminAsync();
        var user = await chatRepository.GetUserById(userId);
        if (user!.Customer == null)
        {
            foreach (var chat in user.Executor!.ChatRooms!)
            {
                await AddAdminToGroup(admin, chat);
            }
        }
        else
        {
            foreach (var chat in user.Customer!.ChatRooms!)
            {
                await AddAdminToGroup(admin, chat);
            }
        }
    }
    
    private async Task RemoveAdminFromGroup(Admin admin, ChatRoom chat)
    {
        if (admin.ChatRooms!.Contains(chat))
        {
            admin.ChatRooms!.Remove(chat);
            await adminRepository.UpdateAdminAsync(admin);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.RoomName);
        }
    }
    
    public async Task RemoveAdminFromChat(int chatId)
    {
        var admin = await adminRepository.GetAdminAsync();
        var chatRoom = await chatRepository.GetChatAsync(chatId);
        await RemoveAdminFromGroup(admin, chatRoom);
    }
    
    public async Task RemoveAdminFromUserChats(int userId)
    {
        var admin = await adminRepository.GetAdminAsync();
        var user = await chatRepository.GetUserById(userId);
        if (user!.Customer == null)
        {
            foreach (var chat in user.Executor!.ChatRooms!)
            {
                await RemoveAdminFromGroup(admin, chat);
            }
        }
        else
        {
            foreach (var chat in user.Customer!.ChatRooms!)
            {
                await RemoveAdminFromGroup(admin, chat);
            }
        }
    }

    public Task SendMessageToGroup(string groupName, MessageObjectDto message)
    {
        return Clients.Group(groupName).SendAsync(message);
    }

    public async Task ReceiveMessages(string userToken, int chatId, int offset = 0, int limit = 10)
    {
        var connection = ChatManager.GetConnectedUserByToken(userToken);
        var messages = await chatRepository.GetMessages(chatId, offset, limit) ?? null;
        if (messages == null)
            throw new ArgumentException("сообщений нет");
        var messagesForFullstack = await chatService.GetMessagesInfo(messages);
        if (connection != null)
            await Clients.Client(connection.ConnectionId).SendMessages(messagesForFullstack);
    }
    
    public async Task ReceiveUserMessages(string adminToken, int chatId, int offset = 0, int limit = 10)
    {
        var connection = ChatManager.GetConnectedUserByToken(adminToken);
        var connectedUser = await chatRepository.GetUserByRefreshToken(connection!.Token);
        if (connectedUser.Admin != null)
        {
            var messages = await chatRepository.GetMessages(chatId, offset, limit) ?? null;
            if (messages == null)
                throw new ArgumentException("сообщений нет");
            var messagesForFullstack = await chatService.GetMessagesInfo(messages);
            if (connection != null)
                await Clients.Client(connection.ConnectionId).SendUserMessages(messagesForFullstack);
        }
    }

    public async Task ReceiveAllUserChatRooms(string userToken, int userId, int offset = 0, int limit = 10)
    {
        try
        {
            var connection = ChatManager.GetConnectedUserByToken(userToken);
            var connectedUser = await chatRepository.GetUserByRefreshToken(connection!.Token);
            if (connectedUser.Admin != null)
            {
                var user = await userRepository.GetUserByIdAsync(userId);
                List<ChatRoom> userChats;
                List<ChatDto> chatsForFullstack;
                if (user.Customer == null)
                {
                    userChats =
                        await chatRepository.GetAllExecutorChatRooms(user.Executor!.ExecutorId, offset, limit);
                    chatsForFullstack = await chatService.GetExecutorChatsInfo(userChats);
                }
                else
                {
                    userChats =
                        await chatRepository.GetAllCustomerChatRooms(user.Customer.CustomerId, offset, limit);
                    chatsForFullstack = await chatService.GetCustomerChatsInfo(userChats);
                }

                await Clients.Client(connection.ConnectionId).ReceiveUserChatRooms(chatsForFullstack);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task GetUserStartedChats(string userToken, int userId, int offset = 0, int limit = 10)
    {
        try
        {
            var connection = ChatManager.GetConnectedUserByToken(userToken);
            var connectedUser = await chatRepository.GetUserByRefreshToken(connection!.Token);
            var user = connectedUser;
            if (connectedUser.Admin != null && userId != 0)
            {
                user = await userRepository.GetUserByIdAsync(userId);
            }
            
            List<ChatRoom> userChats;
            List<ChatDto> chatsForFullstack;
            if (user.Customer != null)
            {
                userChats =
                    await chatRepository.GetCustomerStartedChats(user.Customer.CustomerId, offset, limit);
                chatsForFullstack = await chatService.GetCustomerChatsInfo(userChats);
            }
            else
            {
                userChats =
                    await chatRepository.GetExecutorStartedChats(user.Executor.ExecutorId, offset, limit);
                chatsForFullstack = await chatService.GetExecutorChatsInfo(userChats);
            }

            await Clients.Client(connection.ConnectionId).ReceiveUserStartedChatRooms(chatsForFullstack);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task GetUserNotStartedChats(string userToken, int userId, int offset = 0, int limit = 10)
    {
        try
        {
            var connection = ChatManager.GetConnectedUserByToken(userToken);
            var connectedUser = await chatRepository.GetUserByRefreshToken(connection!.Token);
            var user = connectedUser;
            if (connectedUser.Admin != null && userId != 0)
            {
                user = await userRepository.GetUserByIdAsync(userId);
            }
            
            List<ChatRoom> userChats;
            List<ChatDto> chatDtos;
            if (user.Customer != null)
            {
                userChats =
                    await chatRepository.GetCustomerNotStartedChats(user.Customer.CustomerId, offset, limit);
                chatDtos = await chatService.GetCustomerChatsInfo(userChats);
            }
            else
            {
                userChats =
                    await chatRepository.GetExecutorNotStartedChats(user.Executor!.ExecutorId, offset, limit);
                chatDtos = await chatService.GetExecutorChatsInfo(userChats);
            }

            await Clients.Client(connection.ConnectionId).ReceiveUserNotStartedChatRooms(chatDtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    

    public async Task ReceiveAllChatRooms(string userToken, int offset = 0, int limit = 10)
    {
        try
        {
            var connection = ChatManager.GetConnectedUserByToken(userToken);
            var user = await chatRepository.GetUserByRefreshToken(connection!.Token);

            List<ChatRoom> userChats;
            List<ChatDto> chatsForFullstack;
            if (user.Customer == null)
            {
                userChats =
                    await chatRepository.GetAllExecutorChatRooms(user.Executor!.ExecutorId, offset, limit);
                chatsForFullstack = await chatService.GetExecutorChatsInfo(userChats);
            }
            else
            {
                userChats =
                    await chatRepository.GetAllCustomerChatRooms(user.Customer.CustomerId, offset, limit);
                chatsForFullstack = await chatService.GetCustomerChatsInfo(userChats);
            }

            await Clients.Client(connection.ConnectionId).ReceiveChatRooms(chatsForFullstack);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ReceiveStartedChats(string userToken, int offset = 0, int limit = 10)
    {
        try
        {
            var connection = ChatManager.GetConnectedUserByToken(userToken);
            var user = await chatRepository.GetUserByRefreshToken(connection!.Token);

            List<ChatRoom> userChats;
            List<ChatDto> chatsForFullstack;
            if (user.Customer != null)
            {
                userChats =
                    await chatRepository.GetCustomerStartedChats(user.Customer.CustomerId, offset, limit);
                chatsForFullstack = await chatService.GetCustomerChatsInfo(userChats);
            }
            else
            {
                userChats =
                    await chatRepository.GetExecutorStartedChats(user.Executor.ExecutorId, offset, limit);
                chatsForFullstack = await chatService.GetExecutorChatsInfo(userChats);
            }

            await Clients.Client(connection.ConnectionId).ReceiveStartedChatRooms(chatsForFullstack);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ReceiveNotStartedChats(string userToken, int offset = 0, int limit = 10)
    {
        try
        {
            var connection = ChatManager.GetConnectedUserByToken(userToken);
            var user = await chatRepository.GetUserByRefreshToken(connection!.Token);

            List<ChatRoom> userChats;
            List<ChatDto> chatDtos;
            if (user.Customer != null)
            {
                userChats =
                    await chatRepository.GetCustomerNotStartedChats(user.Customer.CustomerId, offset, limit);
                chatDtos = await chatService.GetCustomerChatsInfo(userChats);
            }
            else
            {
                userChats =
                    await chatRepository.GetExecutorNotStartedChats(user.Executor!.ExecutorId, offset, limit);
                chatDtos = await chatService.GetExecutorChatsInfo(userChats);
            }

            await Clients.Client(connection.ConnectionId).ReceiveNotStartedChatRooms(chatDtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SetMessageViewed(int messageId, string userToken)
    {
        var connection = ChatManager.GetConnectedUserByToken(userToken);
        try
        {
            var user = await chatRepository.GetUserByRefreshToken(connection!.Token);
            if (user.Admin == null)
            {
                var message = await chatRepository.SetMessageViewed(messageId, user.Id);
                var messageForFullstack = await chatService.GetMessageObjectInfo(message);
                if (message.IsViewed)
                {
                    await notificationService.SetMessageNotificationViewed(user.Id, message.Chat.Id,message.Id);
                    await notificationService.SendNotificationsCount(userToken);
                }
            
                if (connection != null)
                    await Clients.Group(message.Chat.RoomName).MarkMessage(messageForFullstack);
            }
        }
        catch (Exception e)
        {
            if (connection != null)
                await Clients.Client(connection.ConnectionId).SendMessageNotFound(e.Message);
        }
    }
    
    public async Task GetUserChatRoom(string adminToken, int userId, int chatRoomId, int offset, int limit)
    {
        try
        {
            var connection = ChatManager.GetConnectedUserByToken(adminToken);
            var admin = await chatRepository.GetUserByRefreshToken(connection!.Token);
            if (admin.Admin != null)
            {
                var user = await userRepository.GetUserByIdAsync(userId);
                var chat = await chatRepository.GetConversationAsync(chatRoomId);
                ChatDto chatDtos;
                if (user.Customer != null)
                    chatDtos = await chatService.GetChatInfoForCustomer(chat);
                else
                {
                    chatDtos = await chatService.GetChatInfoForExecutor(chat);
                }

                var messages = await chatRepository.GetMessages(chatRoomId, offset, limit) ?? null;
                if (messages == null)
                    throw new ArgumentException("сообщений нет");
                var messagesForFullstack = await chatService.GetMessagesInfo(messages);
                if (connection != null)
                    await Clients.Client(connection.ConnectionId)
                        .SendUserChatRoom(chatDtos, messagesForFullstack);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task GetChatRoom(string userToken, int chatRoomId, int offset, int limit)
    {
        try
        {
            var connection = ChatManager.GetConnectedUserByToken(userToken);
            var user = await chatRepository.GetUserByRefreshToken(connection!.Token);
            var chat = await chatRepository.GetConversationAsync(chatRoomId);
            ChatDto chatDtos;
            if (user.Customer != null)
                chatDtos = await chatService.GetChatInfoForCustomer(chat);
            else
            {
                chatDtos = await chatService.GetChatInfoForExecutor(chat);
            }

            var messages = await chatRepository.GetMessages(chatRoomId, offset, limit) ?? null;
            if (messages == null)
                throw new ArgumentException("сообщений нет");
            var messagesForFullstack = await chatService.GetMessagesInfo(messages);
            if (connection != null)
                await Clients.Client(connection.ConnectionId)
                    .SendChatRoom(chatDtos, messagesForFullstack);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task SearchUserChats(string adminToken, int userId, string word, int offset = 0, int limit = 10)
    {
        var connection = ChatManager.GetConnectedUserByToken(adminToken);
        var admin = await chatRepository.GetUserByRefreshToken(connection!.Token);
        if (admin.Admin != null)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            List<ChatRoom> userChats;
            List<ChatDto> chatDtos;
            if (user!.Customer != null)
            {
                userChats =
                    await chatRepository.GetAllCustomerChatRooms(user.Customer!.CustomerId, offset, limit);
                chatDtos = await chatService.GetCustomerChatsInfo(userChats);
            }
            else
            {
                userChats =
                    await chatRepository.GetAllExecutorChatRooms(user.Executor!.ExecutorId, offset, limit);
                chatDtos = await chatService.GetExecutorChatsInfo(userChats);
            }

            await Clients.Client(connection.ConnectionId)
                .ReceiveUserFoundChats(chatService.GetIntersectedChats(word.Trim(), chatDtos));
        }
    }

    public async Task SearchChats(string userToken, string word, int offset = 0, int limit = 10)
    {
        var connection = ChatManager.GetConnectedUserByToken(userToken);
        var user = await chatRepository.GetUserByRefreshToken(connection!.Token);
        List<ChatRoom> userChats;
        List<ChatDto> chatDtos;
        if (user.Customer != null)
        {
            userChats =
                await chatRepository.GetAllCustomerChatRooms(user.Customer!.CustomerId, offset, limit);
            chatDtos = await chatService.GetCustomerChatsInfo(userChats);
        }
        else
        {
            userChats =
                await chatRepository.GetAllExecutorChatRooms(user.Executor!.ExecutorId, offset, limit);
            chatDtos = await chatService.GetExecutorChatsInfo(userChats);
        }

        await Clients.Client(connection.ConnectionId)
            .ReceiveFoundChats(chatService.GetIntersectedChats(word.Trim(), chatDtos));
    }

    public async Task Send(string message, string userToken, int chatId)
    {
        try
        {
            var connection = ChatManager.GetConnectedUserByToken(userToken);
            var chat = await chatRepository.GetChatAsync(chatId);
            var user = await chatRepository.GetUserByRefreshToken(userToken);
            if (user.Admin == null)
            {
                var text = await chatService.CreateMessage(user.Id, chatId, message);
                var messageForFullstack = await chatService.GetMessageObjectInfo(text);

                var receiver = user.Customer == null
                    ? await userRepository.GetUserByCustomerIdAsync(chat.CustomerId)
                    : await userRepository.GetUserByExecutorIdAsync(chat.ExecutorId);
                await notificationService.CreateChatNotification(receiver, chatId, text);
                if (connection != null)
                    await SendMessageToGroup(chat.RoomName, messageForFullstack);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}