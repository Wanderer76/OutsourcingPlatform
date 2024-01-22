using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Service;

public class CustomerService
{
    private readonly IUserRepository userRepository;
    private readonly IOrderRepository orderRepository;
    private readonly ICustomerRepository customerRepository;

    public CustomerService(ICustomerRepository customerRepository, IOrderRepository orderRepository,
        IUserRepository userRepository)
    {
        this.customerRepository = customerRepository;
        this.orderRepository = orderRepository;
        this.userRepository = userRepository;
    }

    public async Task<CustomerInfoDto> GetDetailOpenInfo(int customerId)
    {
        var user = await userRepository.GetUserByCustomerIdAsync(customerId);
        var orders = await orderRepository.GetUserOrdersAsync(user, 0, int.MaxValue);
        return new CustomerInfoDto(user, orders);
    }

    public async Task<CustomerDetailClose> GetDetailCloseInfo(int customerId)
    {
        var user = await userRepository.GetUserByCustomerIdAsync(customerId);
        var orderCount = orderRepository.GetUserOrdersCount(customerId);
        return new CustomerDetailClose(user, orderCount);
    }

    public async Task<bool> IsCustomerConnectedToExecutor(Executor? executor, int customerId)
    {
        return await customerRepository.IsCustomerConnectWithExecutorAsync(executor, customerId);
    }

    public async Task<List<DetailOrderDto>> GetCustomerOrders(int customerId, int offset, int limit)
    {
        var customer = await customerRepository.GetCustomerByIdAsync(customerId);
        return (await orderRepository.GetUserOrdersAsync(customer.User, offset, limit))
            .Select(order => new DetailOrderDto(order, customer.User))
            .ToList();
    }
}