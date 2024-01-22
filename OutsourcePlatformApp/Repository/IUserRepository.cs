using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface IUserRepository
{
    Task<Executor> CreateExecutorAsync(Executor registerDto);
    Task<Customer> CreateCustomerAsync(Customer registerDto);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
    Task DeleteUserByIdAsync(int id);
    Task<User?> GetUserByIdAsync(int id);
    Task<List<User>> GetUsersByIdAsync(List<int> ids);
    Task<User?> GetUserByUsernameAsync(string username);
    Task UpdateUserInfoAsync();
    Task<IQueryable<Education>> GetExecutorEducations(Executor executor);
    Task<Education> GetEducationToUpdate(EducationUpdateDto updatedEducation);
    Task<List<User>> GetUsers(int offset, int limit);
    Task<List<User>> FilterUsers(bool? isBanned, int offset, int limit);

    Task<User> GetUserByExecutorIdAsync(int id);
    Task<User> GetUserByCustomerIdAsync(int id);  
    Task<User> GetCommonUserByExecutorIdAsync(int id);
    Task<User> GetCommonUserByCustomerIdAsync(int id);

    Task<User> GetUserByRefreshToken(string refreshToken);

    Task<RefreshToken> GetRefreshTokenByUserId(int userId);
    Task<bool> IsBannedByUsernameAsync(string username);
    Task<User> GetUserbyActivationCode(Guid code);
}