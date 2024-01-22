using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public class ExecutorRepository : IExecutorRepository
{
    private readonly ApplicationDbContext DbContext;

    public ExecutorRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<List<Executor>> GetExecutorsAsync(int offset, int limit)
    {
        return await DbContext.Executors
            .Include(executor => executor.Skills)
            .Include(executor => executor.Categories)
            .Include(executor => executor.User)
            .ThenInclude(user => user.Reviews)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Executor>> GetExecutorListAsync()
    {
        return await DbContext.Executors
            .Include(executor => executor.Skills)
            .Include(executor => executor.Categories)
            .Include(executor => executor.User)
            .ThenInclude(user => user.Reviews)
            .ToListAsync();
    }

    public async Task<bool> IsExecutorConnectWithCustomer(Customer customer, int executorId)
    {
        return await DbContext.Responses
            .Where(response => response.OrderVacancy.Order.Creator.Customer != null &&
                               response.OrderVacancy.Order.Creator.Customer.Id == executorId)
            .AnyAsync(response => response.ExecutorId == executorId);
    }

    public async Task<int> GetTotalExecutorsCountAsync()
    {
        return await DbContext.Executors.CountAsync();
    }

    public async Task<int> GetExecutorFinishProjects(int executorId)
    {
        return await DbContext.Responses.Where(
            response => response.IsCompleted && response.ExecutorId == executorId
        ).CountAsync();
    }

    public async Task<Executor> GetExecutorById(int executorId)
    {
        return await DbContext.Executors
            .Where(executor => executor.Id == executorId)
            .Include(executor => executor.User)
            .FirstAsync();
    }

    public async Task UpdateExecutorAsync(Executor executor)
    {
        DbContext.Executors.Update(executor);
        await DbContext.SaveChangesAsync();
    }

    public async Task<int> GetFinishedCountProjectsAsync(int executorId)
    {
        return await DbContext.Responses.Where(response => response.ExecutorId == executorId && response.IsCompleted)
            .CountAsync();
    }

    public async Task<int> GetProjectsCountAsync(int executorId)
    {
        return await DbContext.Responses.Where(response => response.ExecutorId == executorId).CountAsync();
    }
}