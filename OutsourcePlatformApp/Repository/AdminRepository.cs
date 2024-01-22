using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext DbContext;

    public AdminRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task<Admin> GetAdminAsync()
    {
        return await DbContext.Admin
            .Include(admin => admin.ChatRooms)
            .Include(admin=> admin.User)
            .FirstAsync();
    }

    public async Task UpdateAdminAsync(Admin admin)
    {
        DbContext.Admin.Update(admin);
        await DbContext.SaveChangesAsync();
    }

    public async Task<int> GetTotalBannedCountAsync()
    {
        return await DbContext.Users.Where(user => user.IsBanned).CountAsync();
    }
}