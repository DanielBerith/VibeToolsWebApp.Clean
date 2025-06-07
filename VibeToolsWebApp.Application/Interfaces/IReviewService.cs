using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.Application.Interfaces
{
    public interface IReviewService
    {
        Task<Review> AddReviewAsync(Guid toolId, string author, string comment, int rating);
    }
}
