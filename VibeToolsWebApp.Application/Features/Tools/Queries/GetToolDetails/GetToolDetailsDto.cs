namespace VibeToolsWebApp.Application.Features.Tools.Queries.GetToolDetails
{
    public class GetToolDetailsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string Category { get; set; } = default!;

        public decimal AverageRating { get; set; }

        public bool IsCommunityFavorite { get; set; }

        public bool IsHidden { get; set; }

        public IEnumerable<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }

    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string toolId { get; set; } = default!;
        public string Comment { get; set; } = default!;

        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
