using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeToolsWebApp.Application.Features.Reviews.Queries.GetReviews
{
    public record GetReviewsQuery(Guid ToolId) : IRequest<IEnumerable<GetReviewsDto>>;
}
