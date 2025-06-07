using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Application.Interfaces;

namespace VibeToolsWebApp.Application.Features.Tools.Queries.GetToolDetails
{
    public class GetToolDetailsQueryHandler : IRequestHandler<GetToolDetailsQuery, GetToolDetailsDto>
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToolDetailsQueryHandler> _logger;

        public GetToolDetailsQueryHandler(
            IToolRepository toolRepository,
            IMapper mapper,
            ILogger<GetToolDetailsQueryHandler> logger
        )
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetToolDetailsDto> Handle(
            GetToolDetailsQuery request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation("Handling GetToolDetailsQuery for ToolId={ToolId}", request.Id);

            var tool = await _toolRepository.GetByIdAsync(request.Id);
            if (tool == null)
            {
                _logger.LogWarning("Tool with Id={ToolId} was not found", request.Id);
                throw new KeyNotFoundException($"Tool with Id {request.Id} was not found.");
            }

            var dto = _mapper.Map<GetToolDetailsDto>(tool);
            var reviewCount = dto.Reviews?.Count() ?? 0;

            _logger.LogInformation(
                "Mapped ToolId={ToolId} to GetToolDetailsDto with {ReviewCount} reviews",
                request.Id, reviewCount
            );

            return dto;
        }
    }
}
