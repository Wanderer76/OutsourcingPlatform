using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface IAdminRepository
{
    Task<Admin> GetAdminAsync();
    Task UpdateAdminAsync(Admin admin);
    Task<int> GetTotalBannedCountAsync();
}