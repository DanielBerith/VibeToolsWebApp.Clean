using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Application.Interfaces;
using VibeToolsWebApp.Domain.Entities;
using VibeToolsWebApp.Persistence.DatabaseContext;

namespace VibeToolsWebApp.Persistence.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly VibeToolsDatabaseContext _dbContext;

        public ReviewRepository(VibeToolsDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Review> AddAsync(Review review)
        {
            _dbContext.Reviews.Add(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<IEnumerable<Review>> ListByToolAsync(Guid toolId)
        {
            return await _dbContext.Reviews
                            .Where(r => r.ToolId == toolId)
                            .OrderByDescending(r => r.CreatedAt)
                            .ToListAsync();
        }
    }
}
