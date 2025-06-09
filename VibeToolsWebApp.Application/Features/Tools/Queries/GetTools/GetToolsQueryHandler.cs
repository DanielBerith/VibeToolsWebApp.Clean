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

            // 1) Retrieve and filter
            var tools = await _toolRepository.SearchAsync(request.SearchTerm);
            var visible = tools.Where(t => !t.IsHidden);

            // 2) Map to DTOs
            var dtos = _mapper
                .Map<IEnumerable<GetToolsDto>>(visible)
                .ToList();   // materialize so we can scan

            // 3) Find the highest average rating
            if (dtos.Any())
            {
                var maxAvg = dtos.Max(d => d.AverageRating);

                // 4) Mark any DTO with that highest rating as community favorite
                foreach (var dto in dtos)
                {
                    dto.IsCommunityFavorite = dto.AverageRating == maxAvg;
                }
            }

            _logger.LogInformation("GetToolsQuery handled: returned {Count} tools", dtos.Count);
            return dtos;
        }
    }
}
