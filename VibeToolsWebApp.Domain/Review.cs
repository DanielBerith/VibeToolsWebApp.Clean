using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeToolsWebApp.Domain
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid ToolId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } 
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Tool Tool { get; set; }
    }
}
