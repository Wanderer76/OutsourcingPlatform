using OutsourcePlatformApp.Dto.notification;
using OutsourcePlatformApp.Models.Notifications;

namespace OutsourcePlatformApp.Hubs;

public interface INotificationHub
{
    Task ReceiveAllUserNotifications(string userToken, int offset = 0, int limit = 10);
    Task SendLatestNotification(ActionNotification actionNotification, string userToken);
    Task ReceiveNotifications(List<ActionNotification> notifications,NotificationCountDto notificationCount);
    Task ReceiveLastNotification(ActionNotification actionNotifications);
    Task SendNotificationCount(NotificationCountDto notificationCountDto);
    Task SendChatNotification(ActionNotification notification);
    Task SendMessageNotification(ActionNotification chatNotification);
}