using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext DbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        await DbContext.Orders.AddAsync(order);
        await DbContext.SaveChangesAsync();
        return order;
    }

    public async Task<Order> SaveChangesAsync(Order order)
    {
        DbContext.Orders.Update(order);
        await DbContext.SaveChangesAsync();
        return order;
    }

    public async Task DeleteOrderAsync(Order order)
    {
        if (await DbContext.Orders.FindAsync(order.Id) != null)
        {
            DbContext.Orders.Remove(order);
            await DbContext.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Нет такого заказа");
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await DbContext.Orders
            .Where(order => order.Id == orderId)
            .Include(order => order.OrderVacancies)
            .ThenInclude(vacancy => vacancy.Responses)
            .Include(order => order.Reviews)
            .Include(order => order.OrderCategories)
            .Include(order => order.OrderSkills)
            .Include(order => order.OrderVacancies).ThenInclude(x => x.OrderRole)
            .Include(order => order.Creator)
            .Include(order => order.OrderVacancies)
            .ThenInclude(vacancy => vacancy.Responses)
            .FirstOrDefaultAsync();
    }

    public async Task CreateResponseAsync(Executor executor, Order order, int vacancyId)
    {
        if (await DbContext.Orders.FindAsync(order.Id) != null)
        {
            var isResponseExists =
                await DbContext.Responses
                    .FirstOrDefaultAsync(response =>
                        response.OrderVacancy.OrderId.Equals(order.Id)
                        && response.ExecutorId.Equals(executor.Id));
            if (isResponseExists != null)
                throw new ArgumentException("Вы уже откликались на этот заказ");
            var vacancy = await DbContext.OrderVacancies.FirstOrDefaultAsync(x => x.Id == vacancyId);
            await DbContext.Responses.AddAsync(new Response(executor.Id, vacancy));
            await DbContext.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Нет такого заказа");
        }
    }

    public async Task<Response> GetResponseById(int id)
    {
        return await DbContext.Responses
            .Where(response => response.Id == id)
            .Include(response => response.OrderVacancy)
            .FirstAsync();
    }

    public async Task<List<Order>> GetActualOrdersAsync()
    {
        return await DbContext.Orders
            .Where(order => order.IsCompleted == false && order.IsPublished)
            .Include(order => order.OrderVacancies)
            .ThenInclude(x => x.Responses)
            .Include(x => x.OrderVacancies)
            .ThenInclude(x => x.OrderRole)
            .Include(order => order.OrderCategories)
            .Include(order => order.OrderSkills)
            .ToListAsync();
    }

    public int GetUserOrdersCount(int userId)
    {
        return DbContext.Orders
            .Count(order => order.Creator.Id == userId);
    }

    public async Task<int> GetTotalOrderCountAsync()
    {
        return await DbContext.Orders.CountAsync();
    }

    public async Task<List<Order>> GetOrdersListAsync(int offset, int limit)
    {
        return await DbContext.Orders
            .Where(order => order.IsCompleted == false && order.IsPublished)
            .Include(order => order.OrderVacancies)
            .ThenInclude(x => x.Responses)
            .Include(x => x.OrderVacancies)
            .ThenInclude(x => x.OrderRole)
            .Include(order => order.OrderCategories)
            .Include(order => order.OrderSkills)
            .OrderBy(order => order.Deadline)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task UpdateResponseAsync(Response response)
    {
        if (await DbContext.Orders.FindAsync(response.OrderVacancy.OrderId) != null)
        {
            DbContext.Responses.Update(response);
            await DbContext.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Нет такого заказа");
        }
    }

    public async Task<List<Order>> GetUserOrdersAsync(User user, int offset, int limit)
    {
        return await DbContext.Orders.Where(order => order.Creator.Id == user.Id)
            .Include(order => order.OrderVacancies)
            .ThenInclude(x => x.Responses)
            .Include(x => x.OrderVacancies)
            .ThenInclude(x => x.OrderRole)
            .Include(order => order.OrderCategories)
            .Include(order => order.OrderSkills)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Order>> GetExecutorOrdersAsync(Executor executor, int offset, int limit)
    {
        var orderIds = await DbContext.Responses.Where(r => r.IsAccept == true && r.ExecutorId == executor.Id)
            .Select(x => x.OrderVacancy.OrderId).ToListAsync();
        return await DbContext.Orders.Where(x => orderIds.Contains(x.Id))
            .Include(order => order.OrderVacancies)
            .ThenInclude(x => x.Responses)
            .Include(x => x.OrderVacancies)
            .ThenInclude(x => x.OrderRole)
            .Include(x => x.OrderCategories)
            .Include(x => x.OrderSkills)
            .Include(x => x.Creator)
            .Include(x => x.Reviews)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
}