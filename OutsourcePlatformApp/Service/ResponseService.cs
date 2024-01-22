using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Service;

public class ResponseService
{
    private readonly IResponseRepository responseRepository;

    public ResponseService(IResponseRepository responseRepository)
    {
        this.responseRepository = responseRepository;
    }

    public async Task<Executor> GetExecutorByResponseId(int responseId)
    {
        return await responseRepository.GetExecutorByResponseIdAsync(responseId);
    }

    public async Task<Order> GetOrderByResponseId(int responseId)
    {
        return await responseRepository.GetOrderByResponseId(responseId);
    }
}