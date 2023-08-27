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
        if (await DbContext.Orders.FindAsync(order.OrderId) != null)
        {
            DbContext.Orders.Remove(order);
            await DbContext.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Нет такого заказа");
        }
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await DbContext.Orders
            .Where(order => order.OrderId == id)
            .Include(order => order.Responses)
            .Include(order => order.OrderCategories)
            .Include(order => order.OrderSkills)
            .Include(order => order.Customer)
            .FirstAsync();
    }

    public async Task CreateResponseAsync(Executor executor, Order order)
    {
        if (await DbContext.Orders.FindAsync(order.OrderId) != null)
        {
            var isResponseExists =
                await DbContext.Responses
                    .FirstOrDefaultAsync(response => 
                        response.Order.OrderId.Equals(order.OrderId) 
                        && response.ExecutorId.Equals(executor.ExecutorId));
            if (isResponseExists != null)
                throw new ArgumentException("Вы уже откликались на этот заказ");

            await DbContext.Responses.AddAsync(new Response(executor.ExecutorId, order));
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
            .Include(response => response.Order)
            .FirstAsync();
    }
    
    public async Task<List<Order>> GetActualOrdersAsync()
    {
        return await DbContext.Orders
            .Where(order => order.IsCompleted == false && order.IsPublished)
            .Include(order => order.Responses)
            .Include(order => order.OrderSkills)
            .Include(order => order.OrderCategories)
            .ToListAsync();
    }

    public int GetCustomerOrdersCount(int customerId)
    {
        return DbContext.Orders
            .Count(order => order.Customer.CustomerId == customerId);
    }

    public async Task<int> GetTotalOrderCountAsync()
    {
        return await DbContext.Orders.CountAsync();
    }

    public async Task<List<Order>> GetOrdersListAsync(int offset, int limit)
    {
        return await DbContext.Orders
            .Where(order => order.IsCompleted == false && order.IsPublished)
            .Include(order=>order.Responses)
            .Include(order => order.OrderSkills)
            .Include(order => order.OrderCategories)
            .OrderBy(order => order.Deadline)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task UpdateResponseAsync(Response response)
    {
        if (await DbContext.Orders.FindAsync(response.Order.OrderId) != null)
        {
            DbContext.Responses.Update(response);
            await DbContext.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Нет такого заказа");
        }
    }

    public async Task<List<Order>> GetCustomerOrdersAsync(Customer customer, int offset, int limit)
    {
        return await DbContext.Orders.Where(order => order.Customer == customer)
            .Include(order => order.Responses)
            .Include(order => order.OrderSkills)
            .Include(order => order.OrderCategories)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task<List<Order>> GetExecutorOrdersAsync(Executor executor, int offset, int limit)
    {
        return await DbContext.Responses.Where(r => r.IsAccept == true && r.ExecutorId == executor.ExecutorId)
            .Include(response => response.Order.Responses)
            .Include(response => response.Order.OrderSkills)
            .Include(response => response.Order.OrderCategories)
            .Skip(offset)
            .Take(limit)
            .Select(r => r.Order)
            .ToListAsync();
    }
    
}