using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeToolsWebApp.Domain
{
    public class Tool
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal AverageRating { get; private set; }
        public bool IsCommunityFavorite { get; private set; }
        public bool IsHidden { get; private set; }

        // Navigation
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        // Domain behavior: encapsulate rating logic
        public void RecalculateRating()
        {
            if (Reviews == null || !Reviews.Any())
            {
                AverageRating = 0;
                IsCommunityFavorite = false;
                IsHidden = false;
                return;
            }

            AverageRating = Reviews.Average(r => (decimal)r.Rating);

            var latest5 = Reviews
                         .OrderByDescending(r => r.CreatedAt)
                         .Take(5)
                         .Select(r => r.Rating)
                         .ToList();

            IsCommunityFavorite = latest5.Count == 5 && latest5.All(r => r == 5);
            IsHidden = latest5.Count == 5 && latest5.All(r => r == 1);
        }
    }
}
