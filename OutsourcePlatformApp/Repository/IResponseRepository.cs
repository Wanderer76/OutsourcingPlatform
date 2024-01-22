using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface IResponseRepository
{
    Task<Executor> GetExecutorByResponseIdAsync(int id);
    Task<Order> GetOrderByResponseId(int responseId);
    Task<Response?> GetUserOrderResponseAsync(int userExecutorExecutorId, int orderId);
    Task RemoveResponseAsync(Response response);
}