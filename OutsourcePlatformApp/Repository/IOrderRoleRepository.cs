using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface IOrderRoleRepository
{
    Task<List<OrderRole>> GetOrderRolesByIds(List<int> ids);
    Task<List<OrderRole>> GetOrderRolesByNames(List<string> names);
    Task<List<OrderRole>> GetOrderRoles(int offset, int limit);
}

public class OrderRoleRepository : IOrderRoleRepository
{
    private readonly ApplicationDbContext dbContext;

    public OrderRoleRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<OrderRole>> GetOrderRolesByIds(List<int> ids)
    {
        return await dbContext.OrderRoles.Where(x => ids.Contains(x.Id)).ToListAsync();
    }

    public async Task<List<OrderRole>> GetOrderRolesByNames(List<string> names)
    {
        return await dbContext.OrderRoles.Where(x => names.Contains(x.Name)).ToListAsync();

    }

    public async Task<List<OrderRole>> GetOrderRoles(int offset, int limit)
    {
        return await dbContext.OrderRoles.Skip(offset).Take(limit).ToListAsync();
    }
}