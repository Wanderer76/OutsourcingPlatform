using Microsoft.EntityFrameworkCore;
using NotificationApp.Entities;
using NotificationApp.Models;

namespace NotificationApp.Persistance;

public interface INotificationRepository
{
    Task<List<ActionNotification>> GetAllUserNotifications(int userId, int offset, int limit);
    ValueTask<int> GetChatNotificationCount(int userId);
    ValueTask<int> GetActionNotificationCount(int userId);
    Task SetNotificationViewed(int notificationId);
    Task SetMessageNotificationViewedAsync(int userId, int chatId, int messageId);
    Task<ActionNotification> CreateNotification(ActionNotification notification);
}

internal class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext context;

    public NotificationRepository(NotificationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<ActionNotification>> GetAllUserNotifications(int userId, int offset, int limit)
    {
        return await context.Notifications
            .Where(x => x.ReceiverId == userId && !x.IsViewed)
            .Skip(offset)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync();
    }

    public async ValueTask<int> GetChatNotificationCount(int userId)
    {
        return await context.Notifications
            .Where(x => x.ReceiverId == userId && !x.IsViewed && x.Type.Equals(NotificationType.Chat.ToString()))
            .CountAsync();
    }

    public async ValueTask<int> GetActionNotificationCount(int userId)
    {
        return await context.Notifications
            .Where(x => x.ReceiverId == userId && !x.IsViewed && x.Type.Equals(NotificationType.Action.ToString()))
            .CountAsync();
    }

    public async Task SetNotificationViewed(int notificationId)
    {
        var a = await context.Notifications.Where(x => x.Id == notificationId).FirstOrDefaultAsync();
        if (a == null)
            throw new ArgumentException("");
        a.IsViewed = true;
        await context.SaveChangesAsync();
    }

    public async Task SetMessageNotificationViewedAsync(int userId, int chatId, int messageId)
    {
        var a = await context.Notifications
            .Where(x => x.ReceiverId == userId && !x.IsViewed && x.Type.Equals(NotificationType.Action.ToString()))
            .Where(x => x.ChatId == chatId && x.MessageId == messageId)
            .FirstOrDefaultAsync();
        if (a == null)
            throw new ArgumentException("");
        a.IsViewed = true;
        await context.SaveChangesAsync();
    }

    public async Task<ActionNotification> CreateNotification(ActionNotification notification)
    {
        await context.Notifications.AddAsync(notification);
        await context.SaveChangesAsync();
        return notification;
    }
}