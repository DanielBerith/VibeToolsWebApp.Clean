using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeToolsWebApp.Application.Features.Reviews.Queries.GetReviews
{
    public class GetReviewsDto
    {
        public Guid Id { get; set; }
        public string Comment { get; set; } = default!;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
