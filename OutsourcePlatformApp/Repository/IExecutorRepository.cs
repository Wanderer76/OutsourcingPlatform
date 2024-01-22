using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface IExecutorRepository
{
    Task<List<Executor>> GetExecutorsAsync(int offset, int limit);
    Task<List<Executor>> GetExecutorListAsync();
    Task<bool> IsExecutorConnectWithCustomer(Customer customer, int executorId);
    Task<int> GetTotalExecutorsCountAsync();
    Task<int> GetExecutorFinishProjects(int executorId);
    Task<Executor> GetExecutorById(int executorId);
    Task UpdateExecutorAsync(Executor executor);
    Task<int> GetFinishedCountProjectsAsync(int executorId);
    Task<int> GetProjectsCountAsync(int executorId);
}