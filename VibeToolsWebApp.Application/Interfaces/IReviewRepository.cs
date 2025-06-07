using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> AddAsync(Review review);
        Task<IEnumerable<Review>> ListByToolAsync(Guid toolId);
    }
}
