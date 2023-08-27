using OutsourcePlatformApp.Models;

namespace OutsourcePlatformApp.Repository;

public interface IReviewRepository
{
   Task<List<Review>> GetReviewsByUserIdAsync(int userId);
   Task<Review> CreateReviewAsync(Review review);
   Task<List<Review>> GetReviewsByExecutorId(int executorId, int offset, int limit);
   Task<bool> IsReviewExists(Order order, User user);
}