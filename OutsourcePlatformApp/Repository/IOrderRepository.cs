using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task DeleteOrderAsync(Order order);
    Task<Order> SaveChangesAsync(Order order);
    Task<Order> GetOrderByIdAsync(int id);
    Task CreateResponseAsync(Executor executor, Order order);
    Task UpdateResponseAsync(Response response);
    Task<List<Order>> GetCustomerOrdersAsync(Customer customer, int offset, int limit);
    Task<List<Order>> GetExecutorOrdersAsync(Executor executor, int offset, int limit);
    Task<Response> GetResponseById(int id);

    Task<List<Order>> GetOrdersListAsync(int offset, int limit);
    Task<List<Order>> GetActualOrdersAsync();
    int GetCustomerOrdersCount(int customerId);
    Task<int> GetTotalOrderCountAsync();
}