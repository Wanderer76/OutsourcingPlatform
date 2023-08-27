using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface ICustomerRepository
{
    Task<bool> IsCustomerConnectWithExecutorAsync(Executor customer,int customerId);
    Task<Customer> GetCustomerByIdAsync(int customerId);
    Task UpdateCustomerAsync(Customer customer);
    Task<int> GetTotalCustomerCountAsync();
    Task<int> GetFinishedCountProjectsAsync(int customerId);
    Task<int> GetProjectsCountAsync(int customerId);
}