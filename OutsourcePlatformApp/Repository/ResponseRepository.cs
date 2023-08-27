using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public class ResponseRepository : IResponseRepository
{
    private readonly ApplicationDbContext DbContext;

    public ResponseRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<Executor> GetExecutorByResponseIdAsync(int id)
    {
        return await DbContext.Executors
            .Where(executor => executor.ExecutorId ==
                               DbContext.Responses.First(response => response.Id == id).ExecutorId)
            .Include(executor => executor.User)
            .FirstAsync();
    }

    public async Task<Order> GetOrderByResponseId(int responseId)
    {
        return await DbContext.Responses.Where(
                response => response.Id == responseId)
            .Include(response => response.Order)
            .Select(response => response.Order)
            .FirstAsync();
    }

    public async Task<Response?> GetUserOrderResponseAsync(int executorId, int orderId)
    {
        return await DbContext.Responses
            .Where(response => response.ExecutorId == executorId && response.Order.OrderId == orderId)
            .FirstOrDefaultAsync();
    }

    public async Task RemoveResponseAsync(Response response)
    {
        DbContext.Responses.Remove(response);
        await DbContext.SaveChangesAsync();
    }
}