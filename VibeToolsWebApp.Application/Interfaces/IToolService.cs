using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.Application.Interfaces
{
    public interface IToolService
    {
        Task<IEnumerable<Tool>> BrowseAsync(string? searchTerm);
        Task<Tool> GetDetailsAsync(Guid id);
        Task<Tool> CreateToolAsync(string name, string description, string category);
    }
}
