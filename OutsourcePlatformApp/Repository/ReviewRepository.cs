using Microsoft.EntityFrameworkCore;
using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext DbContext;


    public ReviewRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<List<Review>> GetReviewsByUserIdAsync(int userId)
    {
        return await DbContext.Reviews
            .Where(review => review.User.Id == userId)
            .Include(review => review.Order)
            .ToListAsync();
    }

    public async Task<Review> CreateReviewAsync(Review review)
    {
        await DbContext.Reviews.AddAsync(review);
        await DbContext.SaveChangesAsync();
        return review;
    }

    public async Task<List<Review>> GetReviewsByExecutorId(int executorId, int offset, int limit)
    {
        return await DbContext.Reviews
            .Where(review => review.User.Executor.Id == executorId)
            .Skip(offset)
            .Take(limit)
            .Include(review => review.User)
            .Include(review => review.Order)
            .Include(review => review.Order.OrderCategories)
            .ToListAsync();
    }

    public async Task<bool> IsReviewExists(Order order, User user)
    {
        return await DbContext.Reviews
            .Where(review => review.Order == order && review.User == user)
            .AnyAsync();
    }
}