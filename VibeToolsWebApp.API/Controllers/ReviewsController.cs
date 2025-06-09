using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibeToolsWebApp.Application.Features.Reviews.Command.CreateReview;
using VibeToolsWebApp.Application.Features.Reviews.Queries.GetReviews;

namespace VibeToolsWebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// GET /api/tools/{toolId}/reviews
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetReviewsDto>>> Get(Guid toolId)
        {
            var reviews = await _mediator.Send(new GetReviewsQuery(toolId));
            return Ok(reviews);
        }

        /// <summary>
        /// POST /api/tools/{toolId}/reviews
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> Post([FromBody] CreateReviewCommand command)
        {

            var reviewId = await _mediator.Send(command);
            // Return 201 Created pointing to the parent tool resource
            return CreatedAtAction(
                actionName: nameof(ToolsController.Get),
                controllerName: "Tools",
                routeValues: new { id = command.ToolId },
                value: reviewId
            );
        }
    }
}
