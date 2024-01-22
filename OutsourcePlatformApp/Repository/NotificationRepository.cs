using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Models.Notifications;

namespace OutsourcePlatformApp.Repository;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext DbContext;

    public NotificationRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<List<ActionNotification>> GetAllUserNotifications(int userId, int offset, int limit)
    {
        var result = await DbContext.Notifications
            .Where(notification => notification.ReceiverId == userId && !notification.IsViewed)
            .OrderBy(notification => notification.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
        return result;
    }

    public async Task<ActionNotification> CreateNotification(ActionNotification notification)
    {
        DbContext.Notifications.Add(notification);
        await DbContext.SaveChangesAsync();
        return notification;
    }

    public async Task SetNotificationViewed(int notificationId)
    {
        var notification =
            await DbContext.Notifications.FirstAsync(notification => notification.Id == notificationId);
        notification.IsViewed = true;
        await DbContext.SaveChangesAsync();
    }

    public Task<int> GetChatNotificationCount(User user)
    {
        return Task.FromResult(DbContext.Notifications
            .Count(notification => notification.ReceiverId == user.Id && !notification.IsViewed &&
                                   notification.Type == NotificationType.Chat.ToString()));
    }

    public Task<int> GetActionNotificationCount(User user)
    {
        return Task.FromResult(DbContext.Notifications
            .Count(notification => notification.ReceiverId == user.Id && !notification.IsViewed &&
                                   notification.Type == NotificationType.Action.ToString()));
    }

    public async Task SetMessageNotificationViewedAsync(int userId, int chatId, int messageId)
    {
        var n = await DbContext.Notifications
            .Where(notification => notification.ReceiverId == userId && notification.ChatId == chatId &&
                                   notification.MessageId == messageId)
            .FirstAsync();
        n.IsViewed = true;
        DbContext.Notifications.Update(n);
        await DbContext.SaveChangesAsync();
    }
}