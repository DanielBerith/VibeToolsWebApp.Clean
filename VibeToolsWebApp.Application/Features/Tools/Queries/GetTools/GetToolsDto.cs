using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeToolsWebApp.Application.Features.Tools.Queries.GetTools
{
    public class GetToolsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public decimal AverageRating { get; set; }
        public bool IsCommunityFavorite { get; set; }
        public bool IsHidden { get; set; }
    }
}
