using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VibeToolsWebApp.Application.Features.Tools.Commands.CreateTool;
using VibeToolsWebApp.Application.Features.Tools.Queries.GetToolDetails;
using VibeToolsWebApp.Application.Features.Tools.Queries.GetTools;

namespace VibeToolsWebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ToolsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// GET /api/tools?search={term}
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetToolsDto>>> Get([FromQuery] string? search = null)
        {
            var tools = await _mediator.Send(new GetToolsQuery(search));
            return Ok(tools);
        }

        /// <summary>
        /// GET /api/tools/{id}
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetToolDetailsDto>> Get(Guid id)
        {
            var tool = await _mediator.Send(new GetToolDetailsQuery(id));
            return Ok(tool);
        }

        /// <summary>
        /// POST /api/tools
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> Post([FromBody] CreateToolCommand command)
        {
            var newId = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = newId }, newId);
        }
    }
}
