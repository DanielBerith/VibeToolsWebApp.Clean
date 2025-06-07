using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.Application.Interfaces
{
    public interface IToolRepository
    {
        Task<Tool> GetByIdAsync(Guid id);
        Task<IEnumerable<Tool>> SearchAsync(string? searchTerm);
        Task AddAsync(Tool tool);
        Task UpdateAsync(Tool tool);
    }
}
