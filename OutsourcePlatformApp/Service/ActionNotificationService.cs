using System.Text;
using Microsoft.AspNetCore.SignalR;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;
using Newtonsoft.Json;
using OutsourcePlatformApp.Dto.notification;
using OutsourcePlatformApp.Hubs;
using OutsourcePlatformApp.Models.Chat;
using OutsourcePlatformApp.Models.Notifications;
using OutsourcePlatformApp.Service.MessageBrocker;
using RabbitMQ.Client;
using Shared;

namespace OutsourcePlatformApp.Service;

public class ActionNotificationService
{
    private readonly IOrderRepository orderRepository;
    private readonly IUserRepository userRepository;
    private readonly INotificationRepository notificationRepository;
    private readonly IHubContext<NotificationHub, INotificationHub> hubContext;
    private readonly NotificationManager notificationManager;

    public ActionNotificationService(IOrderRepository orderRepository, NotificationManager notificationManager,
        IHubContext<NotificationHub, INotificationHub> hubContext, IServiceProvider serviceProvider,
        IUserRepository userRepository, INotificationRepository notificationRepository)
    {
        this.orderRepository = orderRepository;
        this.notificationManager = notificationManager;
        this.hubContext = hubContext;
        this.userRepository = userRepository;
        this.notificationRepository = notificationRepository;
    }

    public async Task<ActionNotification> CreateActionNotification(string token, User sender, User? receiver,
        int orderId,
        string message)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        var notification = new ActionNotification
        {
            ReceiverUsername = order.Creator.Id == receiver.Id ? receiver?.Username : "",
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            Message = message,
            ProjectName = order.Name,
            OrderId = orderId,
            CompanyName = order.CompanyName
        };
        await notificationRepository.CreateNotification(notification);
        var receiverToken = await userRepository.GetRefreshTokenByUserId(receiver.Id);
        var connection = notificationManager.GetConnectedUserByUsername(receiver.Username);
        if (connection != null)
            await hubContext.Clients
                .Client(connection.ConnectionId)
                .SendLatestNotification(notification,
                    token);

        return notification;
    }

    public async Task<ActionNotification> CreateChatNotification(User receiver, int chatId,
        Message message)
    {
        var notification = new ActionNotification
        {
            ReceiverId = receiver.Id,
            Message = message.MessageText,
            ProjectName = "",
            ChatId = chatId,
            CompanyName = "",
            MessageId = message.Id,
            Type = NotificationType.Chat.ToString()
        };

        await notificationRepository.CreateNotification(notification);
        var receiverToken = await userRepository.GetRefreshTokenByUserId(receiver.Id);
        var connection = notificationManager.GetConnectedUserByUsername(receiver.Username);
        if (connection != null)
            await hubContext.Clients
                .Client(connection.ConnectionId)
                .SendNotificationCount(new NotificationCountDto
                {
                    ActionNotificationCount = await notificationRepository.GetActionNotificationCount(receiver),
                    ChatNotificationCount = await notificationRepository.GetChatNotificationCount(receiver)
                });

        return notification;
    }

    public async Task SendChatNotification(User receiver, Message text, string projectName)
    {
        var receiverToken = await userRepository.GetRefreshTokenByUserId(receiver.Id);
        var connection = notificationManager.GetConnectedUserByUsername(receiver.Username);
        if (connection != null)
            await hubContext.Clients
                .Client(connection.ConnectionId)
                .SendMessageNotification(new ActionNotification
                    {
                        Message = text.MessageText,
                        ChatId = text.Id,
                        ProjectName = projectName
                    }
                );
    }

    public async Task SetMessageNotificationViewed(int userId, int chatId, int messageId)
    {
        await notificationRepository.SetMessageNotificationViewedAsync(userId, chatId, messageId);
    }

    public async Task SendNotificationsCount(string userToken)
    {
        var username = JwtUtil.GetClaimsFromToken(userToken)["username"];
        var user = await userRepository.GetUserByUsernameAsync(username);
        var chatNotificationCount = await notificationRepository.GetChatNotificationCount(user);
        var actionNotificationCount = await notificationRepository.GetActionNotificationCount(user);
        var connection = notificationManager.GetConnectedUserByUsername(username);

        if (connection != null)
            await hubContext.Clients.Client(connection.ConnectionId).SendNotificationCount(new NotificationCountDto
            {
                ActionNotificationCount = actionNotificationCount,
                ChatNotificationCount = chatNotificationCount
            });
    }
}