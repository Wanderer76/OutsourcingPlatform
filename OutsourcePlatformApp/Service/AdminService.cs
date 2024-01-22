using Microsoft.AspNetCore.Mvc;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Dto.user;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Service;

public class AdminService
{
    private readonly IUserRepository userRepository;
    private readonly IAdminRepository adminRepository;
    private readonly IOrderRepository orderRepository;
    private readonly IExecutorRepository executorRepository;
    private readonly ICustomerRepository customerRepository;

    public AdminService(IUserRepository userRepository, IAdminRepository adminRepository,
        IOrderRepository orderRepository, IExecutorRepository executorRepository,
        ICustomerRepository customerRepository)
    {
        this.userRepository = userRepository;
        this.adminRepository = adminRepository;
        this.orderRepository = orderRepository;
        this.executorRepository = executorRepository;
        this.customerRepository = customerRepository;
    }

    public async Task<Admin> GetAdminAsync()
    {
        return await adminRepository.GetAdminAsync();
    }

    public async Task ChangeAccountStatus(BanRequest banRequest)
    {
        var user = await userRepository.GetUserByIdAsync(banRequest.UserId);
        user!.IsBanned = banRequest.IsBanned;
        user.BannedMessage = banRequest.Message ?? string.Empty;
        await userRepository.UpdateUserAsync(user);
    }

    public async Task<IEnumerable<UserDto>> GetUsers(bool? isBanned, int offset, int limit)
    {
        List<User> users;
        if (isBanned == null)
        {
            users = await userRepository.GetUsers(offset, limit);
        }
        else
        {
            users = await userRepository.FilterUsers(isBanned, offset, limit);
        }

        return users.Select(user =>
            new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                SecondName = user.SecondName!,
                Surname = user.Surname,
                isBanned = user.IsBanned,
                UserRole = user.UserRoles
                    .FirstOrDefault()!.Name
            }
        );
    }

    public async Task<ExecutorAdminDetailDto> GetExecutorInfo(User? user)
    {
        if (user?.Executor == null)
            throw new ArgumentException("Исполнителя не существует");
        var finishedProjects = await executorRepository.GetFinishedCountProjectsAsync(user.Executor.Id);
        var notFinishedProjects = await executorRepository.GetProjectsCountAsync(user.Executor.Id);
        return new ExecutorAdminDetailDto(user, finishedProjects, notFinishedProjects);
    }

    public async Task<CustomerAdminDetailDto> GetCustomerInfo(User? user)
    {
        if (user?.Customer == null)
            throw new ArgumentException("Заказчика не существует");
        var finishedProjects = await customerRepository.GetFinishedCountProjectsAsync(user.Customer.Id);
        var notFinishedProjects = await customerRepository.GetProjectsCountAsync(user.Customer.Id);
        return new CustomerAdminDetailDto(user, notFinishedProjects, finishedProjects);
    }

    public async Task<SystemInfoResponse> GetSystemUserInfo()
    {
        var executorsCount = await executorRepository.GetTotalExecutorsCountAsync();
        var customersCount = await customerRepository.GetTotalCustomerCountAsync();
        var bannedCount = await adminRepository.GetTotalBannedCountAsync();

        return new SystemInfoResponse
        {
            ExecutorCount = executorsCount,
            CustomerCount = customersCount,
            BannedCount = bannedCount
        };
    }
}