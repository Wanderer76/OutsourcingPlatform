using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;

namespace OutsourcePlatformApp.Service;

public class UserService
{
    private readonly IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<RefreshToken> GetRefreshTokenByUserId(int id)
    {
        return await userRepository.GetRefreshTokenByUserId(id);
    }

    public async Task<bool> IsUserBanned(string username)
    {
        return await userRepository.IsBannedByUsernameAsync(username);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await userRepository.GetUserByUsernameAsync(username);
    }
    
}