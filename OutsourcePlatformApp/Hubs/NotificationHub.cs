using Microsoft.AspNetCore.SignalR;
using OutsourcePlatformApp.Dto.notification;
using OutsourcePlatformApp.Models.Notifications;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Hubs;

public class NotificationHub : Hub<INotificationHub>
{
    private readonly IUserRepository userRepository;
    private readonly INotificationRepository notificationRepository;
    private readonly NotificationManager NotificationManager;

    public NotificationHub(IUserRepository userRepository, INotificationRepository notificationRepository,
        NotificationManager notificationManager)
    {
        this.userRepository = userRepository;
        this.notificationRepository = notificationRepository;
        NotificationManager = notificationManager;
    }

    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        Context.GetHttpContext()!.Request.Query.TryGetValue("token", out var value);
        var token = value.First();
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
                var user = await userRepository.GetUserByUsernameAsync(connection!.Username);
                var userNotification = await notificationRepository.GetAllUserNotifications(user.Id, offset, limit);
                var chatNotificationCount = await notificationRepository.GetChatNotificationCount(user);
                var actionNotificationCount = await notificationRepository.GetActionNotificationCount(user);
                await Clients.Client(connection.ConnectionId).ReceiveNotifications(userNotification,new NotificationCountDto
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
            var user = await userRepository.GetUserByRefreshToken(connection!.Token);
            var chatNotificationCount = await notificationRepository.GetChatNotificationCount(user);
            var actionNotificationCount = await notificationRepository.GetActionNotificationCount(user);
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