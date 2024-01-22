using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface IActivationTokenRepository
{
    Task<List<ActivationToken>> GetActivationTokensByUserId(int id);
    Task UpdateTokenAsync(ActivationToken currentCode);
}

public class ActivationTokenRepository : IActivationTokenRepository
{
    private readonly ApplicationDbContext dbContext;

    public ActivationTokenRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<ActivationToken>> GetActivationTokensByUserId(int userId)
    {
        return await dbContext.ActivationTokens
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public Task UpdateTokenAsync(ActivationToken currentCode)
    {
        dbContext.ActivationTokens.Update(currentCode);
        return Task.CompletedTask;
    }
}