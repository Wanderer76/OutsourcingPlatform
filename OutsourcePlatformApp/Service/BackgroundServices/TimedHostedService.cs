using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Service.BackgroundServices;

public class TimedHostedService : BackgroundService
{
    private const int Duration = 1 * 60 * 1000; // 60 seconds

    private readonly ILogger<TimedHostedService> logger;
    private readonly ApplicationDbContext DbContext;
    public readonly List<DateOnly> OrderDeadlines = new();

    public TimedHostedService(IServiceProvider serviceProvider, ILogger<TimedHostedService> logger)
    {
        this.logger = logger;
        DbContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(Duration * 60 * 24, stoppingToken);
            await CloseProjectByDeadline();
        }
    }

    private async Task CloseProjectByDeadline()
    {
        var orders = await DbContext.Orders
            .Where(order => order.Deadline == DateOnly.FromDateTime(DateTime.Now))
            .ToListAsync();
        if (orders.Count != 0)
        {
            foreach (var order in orders)
            {
                CloseProject(order);
                logger.Log(LogLevel.Information, "Закрыт проект - " + order.Name + "с id = " + order.Id);
            }

            await DbContext.SaveChangesAsync();
        }
    }

    private static void CloseProject(Order order)
    {
        order.IsCompleted = true;
        order.IsPublished = false;
    }
}