using Microsoft.AspNetCore.SignalR;
using NotificationApp.Entities;
using NotificationApp.Models;
using NotificationApp.Persistance;
using RabbitMQ.Client;
using Shared;

namespace NotificationApp.Hubs;

public class NotificationHub : Hub<INotificationHub>
{
    //private readonly IUserRepository userRepository;
    private readonly INotificationRepository notificationRepository;
    private readonly NotificationManager NotificationManager = new NotificationManager();
    private readonly IModel chanel;

    public NotificationHub(INotificationRepository notificationRepository)
    {
        this.notificationRepository = notificationRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var value = Context.GetHttpContext()!.Request.Headers.Authorization;
        var token = value.First().Replace(' ', '+');
        try
        {
            NotificationManager.ConnectUser(token, connectionId);
            await base.OnConnectedAsync();
        }
        catch (Exception e)
        {
            await OnDisconnectedAsync(null);
        }
    }


    public override Task OnDisconnectedAsync(Exception? exception)
    {
        NotificationManager.DisconnectUser(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task ReceiveAllUserNotifications(string? userToken, int offset = 0, int limit = 10)
    {
        if (userToken != null)
        {
            var connection = NotificationManager.GetConnectedUserByToken(userToken);
            if (connection != null)
            {
                var userId = int.Parse(JwtUtil.GetClaimsFromToken(connection!.Token)["id"]);
                var userNotification = await notificationRepository.GetAllUserNotifications(userId, offset, limit);
                var chatNotificationCount = await notificationRepository.GetChatNotificationCount(userId);
                var actionNotificationCount = await notificationRepository.GetActionNotificationCount(userId);
                await Clients.Client(connection.ConnectionId).ReceiveNotifications(userNotification,
                    new NotificationCountDto
                    {
                        ActionNotificationCount = actionNotificationCount,
                        ChatNotificationCount = chatNotificationCount
                    });
            }
        }
    }

    public async Task SendLatestNotification(string userToken, ActionNotification notification)
    {
        var connection = NotificationManager.GetConnectedUserByToken(userToken);
        if (connection != null)
            await Clients.Client(connection.ConnectionId)
                .ReceiveLastNotification(notification);
    }

    public async Task SetNotificationViewed(int notificationId)
    {
        await notificationRepository.SetNotificationViewed(notificationId);
    }

    public async Task SendNotificationCount(string? userToken)
    {
        if (userToken != null)
        {
            var connection = NotificationManager.GetConnectedUserByToken(userToken);
            var userId = int.Parse(JwtUtil.GetClaimsFromToken(connection!.Token)["id"]);
            var chatNotificationCount = await notificationRepository.GetChatNotificationCount(userId);
            var actionNotificationCount = await notificationRepository.GetActionNotificationCount(userId);
            if (connection != null)
                await Clients.Client(connection.ConnectionId)
                    .SendNotificationCount(new NotificationCountDto
                    {
                        ActionNotificationCount = actionNotificationCount,
                        ChatNotificationCount = chatNotificationCount
                    });
        }
    }
}