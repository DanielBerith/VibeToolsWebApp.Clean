using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Application.Interfaces;
using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.Application.Features.Reviews.Command.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateReviewCommandHandler> _logger;

        public CreateReviewCommandHandler(
            IReviewRepository reviewRepository,
            IToolRepository toolRepository,
            IMapper mapper,
            ILogger<CreateReviewCommandHandler> logger
        )
        {
            _reviewRepository = reviewRepository;
            _toolRepository = toolRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Handling CreateReviewCommand: ToolId={ToolId}, Rating={Rating}",
                request.ToolId, request.Rating
            );

            // Map the command to the domain Review entity
            var review = _mapper.Map<Review>(request);
            review.Id = Guid.NewGuid();
            review.CreatedAt = DateTime.UtcNow;

            // Persist the new review
            await _reviewRepository.AddAsync(review);
            _logger.LogDebug("Persisted review: Id={ReviewId}", review.Id);

            // Recompute aggregates on the Tool
            var tool = await _toolRepository.GetByIdAsync(request.ToolId)
                       ?? throw new KeyNotFoundException($"Tool {request.ToolId} not found.");

            tool.Reviews = (await _reviewRepository.ListByToolAsync(request.ToolId)).ToList();
            tool.RecalculateRating();
            await _toolRepository.UpdateAsync(tool);

            _logger.LogInformation(
                "Updated Tool {ToolId}: AvgRating={Avg}, Hidden={Hidden}, Favorite={Fav}",
                tool.Id, tool.AverageRating, tool.IsHidden, tool.IsCommunityFavorite
            );

            return review.Id;
        }
    }
}
