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
        var executorId = await DbContext.Responses
            .Where(x => x.Id == id)
            .Select(x => x.ExecutorId)
            .FirstAsync();
        return await DbContext.Executors
            .Include(executor => executor.User)
            .Where(executor => executor.Id ==
                               executorId)
            .FirstAsync();
    }

    public async Task<Order> GetOrderByResponseId(int responseId)
    {
        return await DbContext.Responses.Where(
                response => response.Id == responseId)
            .Include(response => response.OrderVacancy)
            .Select(response => response.OrderVacancy.Order)
            .FirstAsync();
    }

    public async Task<Response?> GetUserOrderResponseAsync(int executorId, int orderId)
    {
        return await DbContext.Responses
            .Where(response => response.ExecutorId == executorId && response.OrderVacancy.OrderId == orderId)
            .FirstOrDefaultAsync();
    }

    public async Task RemoveResponseAsync(Response response)
    {
        DbContext.Responses.Remove(response);
        await DbContext.SaveChangesAsync();
    }
}