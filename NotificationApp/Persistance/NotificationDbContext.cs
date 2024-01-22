using Microsoft.EntityFrameworkCore;
using NotificationApp.Entities;

namespace NotificationApp.Persistance;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }
    
    public DbSet<ActionNotification> Notifications { get; set; }
}