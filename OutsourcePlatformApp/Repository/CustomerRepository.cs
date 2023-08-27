using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext DbContext;

    public CustomerRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<bool> IsCustomerConnectWithExecutorAsync(Executor executor, int customerId)
    {
        return await DbContext.Responses
            .Where(response => response.ExecutorId == executor.ExecutorId)
            .AnyAsync(response => response.Order.Customer.CustomerId == customerId);
    }

    public async Task<Customer> GetCustomerByIdAsync(int customerId)
    {
        return await DbContext.Customers
            .Include(customer => customer.Orders)
            .Include(customer => customer.User)
            .FirstAsync(customer => customer.CustomerId == customerId);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        DbContext.Customers.Update(customer);
        await DbContext.SaveChangesAsync();
    }

    public async Task<int> GetTotalCustomerCountAsync()
    {
        return await DbContext.Customers.CountAsync();
    }

    public async Task<int> GetFinishedCountProjectsAsync(int customerId)
    {
        return await DbContext.Orders
            .Where(order => order.Customer.CustomerId == customerId && order.IsCompleted)
            .CountAsync();
    }

    public async Task<int> GetProjectsCountAsync(int customerId)
    {
        return await DbContext.Orders
            .Where(order => order.Customer.CustomerId == customerId)
            .CountAsync();
    }
}