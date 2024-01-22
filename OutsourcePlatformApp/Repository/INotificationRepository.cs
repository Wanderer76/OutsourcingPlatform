using OutsourcePlatformApp.Dto.notification;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Models.Notifications;

namespace OutsourcePlatformApp.Repository;

public interface INotificationRepository
{
    Task<List<ActionNotification>> GetAllUserNotifications(int userId,int offset,int limit);
    Task<ActionNotification> CreateNotification(ActionNotification notification);
    Task SetNotificationViewed(int notificationId);
    Task<int> GetChatNotificationCount(User user);
    Task<int> GetActionNotificationCount(User user);
    Task SetMessageNotificationViewedAsync(int userId, int chatId,int messageId);
}