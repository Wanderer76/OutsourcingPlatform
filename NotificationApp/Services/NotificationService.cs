using Microsoft.AspNetCore.SignalR;
using NotificationApp.Entities;
using NotificationApp.Hubs;
using NotificationApp.Models;
using NotificationApp.Persistance;
using Shared;

namespace NotificationApp.Services;

//TODO адаптировать под брокер сообщений
public class NotificationService
{
    private readonly INotificationRepository notificationRepository;
    private readonly IHubContext<NotificationHub, INotificationHub> hubContext;
    private readonly NotificationManager notificationManager;

    public NotificationService(NotificationManager notificationManager,
        IHubContext<NotificationHub, INotificationHub> hubContext, INotificationRepository notificationRepository)

    {
        this.notificationManager = notificationManager;
        this.hubContext = hubContext;
        this.notificationRepository = notificationRepository;
    }


    public async Task<ActionNotification> CreateActionNotification(ActionNotificationModel notificationModel)
    {
        var notification = new ActionNotification
        {
            SenderId = notificationModel.SenderId,
            ReceiverId = notificationModel.ReceiverId,
            Message = notificationModel.Message,
            ProjectName = notificationModel.ProjectName,
            OrderId = notificationModel.OrderId,
            CompanyName = notificationModel.CompanyName
        };
        await notificationRepository.CreateNotification(notification);
        var connection = notificationManager.GetConnectedUserByToken(notificationModel.UesrToken);
        if (connection != null)
            await hubContext.Clients
                .Client(connection.ConnectionId)
                .SendLatestNotification(notification,
                    notificationModel.UesrToken);

        return notification;
    }

    public async Task<ActionNotification> CreateChatNotification(ChatNotificationModel chatNotification)
    {
        var notification = new ActionNotification
        {
            ReceiverId = chatNotification.ReceiverId,
            Message = chatNotification.Message,
            ProjectName = "",
            ChatId = chatNotification.ChatId,
            CompanyName = "",
            MessageId = chatNotification.MessageId,
            Type = NotificationType.Chat.ToString()
        };

        await notificationRepository.CreateNotification(notification);
        var receiverId = chatNotification.ReceiverId;
        var connection = notificationManager.GetConnectedUserByReceiverId(receiverId);
        if (connection != null)
            await hubContext.Clients
                .Client(connection.ConnectionId)
                .SendNotificationCount(new NotificationCountDto
                {
                    ActionNotificationCount = await notificationRepository.GetActionNotificationCount(receiverId),
                    ChatNotificationCount = await notificationRepository.GetChatNotificationCount(receiverId)
                });

        return notification;
    }

    public async Task SendChatNotification(int receiverId, MessageModel message, string projectName)
    {
        var connection = notificationManager.GetConnectedUserByReceiverId(receiverId);
        if (connection != null)
            await hubContext.Clients
                .Client(connection.ConnectionId)
                .SendMessageNotification(new ActionNotification
                    {
                        Message = message.MessageText,
                        ChatId = message.Id,
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
        var userId = int.Parse(JwtUtil.GetClaimsFromToken(userToken)["id"]);
        var chatNotificationCount = await notificationRepository.GetChatNotificationCount(userId);
        var actionNotificationCount = await notificationRepository.GetActionNotificationCount(userId);
        var connection = notificationManager.GetConnectedUserByToken(userToken);

        if (connection != null)
            await hubContext.Clients.Client(connection.ConnectionId).SendNotificationCount(new NotificationCountDto
            {
                ActionNotificationCount = actionNotificationCount,
                ChatNotificationCount = chatNotificationCount
            });
    }
}