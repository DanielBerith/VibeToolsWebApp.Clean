using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VibeToolsWebApp.Application.Interfaces;

namespace VibeToolsWebApp.Application.Features.Reviews.Queries.GetReviews
{
    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, IEnumerable<GetReviewsDto>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetReviewsQueryHandler> _logger;

        public GetReviewsQueryHandler(
            IReviewRepository reviewRepository,
            IMapper mapper,
            ILogger<GetReviewsQueryHandler> logger
        )
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<GetReviewsDto>> Handle(
            GetReviewsQuery request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation("Handling GetReviewsQuery for ToolId={ToolId}", request.ToolId);

            // Fetch reviews
            var reviews = await _reviewRepository.ListByToolAsync(request.ToolId);

            // Map to DTOs
            var dtos = _mapper.Map<IEnumerable<GetReviewsDto>>(reviews);

            _logger.LogInformation("GetReviewsQuery returned {Count} reviews", dtos is ICollection<GetReviewsDto> col ? col.Count : -1);
            return dtos;
        }
    }
}
