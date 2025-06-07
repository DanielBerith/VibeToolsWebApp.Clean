namespace VibeToolsWebApp.Domain.Entities
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
