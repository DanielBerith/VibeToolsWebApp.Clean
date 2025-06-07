using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibeToolsWebApp.Application.Features.Reviews.Command.CreateReview;

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
        /// POST /api/tools/{toolId}/reviews
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> Post(Guid toolId, [FromBody] CreateReviewCommand command)
        {
            if (command.ToolId != toolId)
                return BadRequest("ToolId in URL must match ToolId in body.");

            var reviewId = await _mediator.Send(command);
            // Return 201 Created pointing to the parent tool resource
            return CreatedAtAction(
                actionName: nameof(ToolsController.Get),
                controllerName: "Tools",
                routeValues: new { id = toolId },
                value: reviewId
            );
        }
    }
}
