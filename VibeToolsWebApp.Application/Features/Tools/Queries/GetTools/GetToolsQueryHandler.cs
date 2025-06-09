using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Application.Interfaces;

namespace VibeToolsWebApp.Application.Features.Tools.Queries.GetTools
{
    public class GetToolsQueryHandler : IRequestHandler<GetToolsQuery, IEnumerable<GetToolsDto>>
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToolsQueryHandler> _logger;

        public GetToolsQueryHandler(
            IToolRepository toolRepository,
            IMapper mapper,
            ILogger<GetToolsQueryHandler> logger
        )
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<GetToolsDto>> Handle(
            GetToolsQuery request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Handling GetToolsQuery (SearchTerm = {SearchTerm})",
                request.SearchTerm
            );

            // Fetch all matching tools
            var tools = await _toolRepository.SearchAsync(request.SearchTerm);

            // Recalculate rating &flags on each
            foreach (var tool in tools)
            {
                tool.RecalculateRating();
            }

            // Map domain entities to DTOs
            var dtos = _mapper.Map<IEnumerable<GetToolsDto>>(tools);

            _logger.LogInformation(
                "GetToolsQuery handled: returned {Count} tools",
                dtos.Count()
            );

            return dtos;
        }
    }
}
