using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task DeleteOrderAsync(Order order);
    Task<Order> SaveChangesAsync(Order order);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task CreateResponseAsync(Executor executor, Order order, int vacancyId);
    Task UpdateResponseAsync(Response response);
    Task<List<Order>> GetUserOrdersAsync(User user, int offset, int limit);
    Task<List<Order>> GetExecutorOrdersAsync(Executor executor, int offset, int limit);
    Task<Response> GetResponseById(int id);
    Task<List<Order>> GetOrdersListAsync(int offset, int limit);
    Task<List<Order>> GetActualOrdersAsync();
    int GetUserOrdersCount(int userId);
    Task<int> GetTotalOrderCountAsync();
}