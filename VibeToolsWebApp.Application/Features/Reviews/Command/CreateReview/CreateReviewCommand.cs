using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeToolsWebApp.Application.Features.Reviews.Command.CreateReview
{
    public record CreateReviewCommand(
        Guid ToolId,
        string Comment,
        int Rating
    ) : IRequest<Guid>;
}
