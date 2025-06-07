using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Application.Interfaces;
using VibeToolsWebApp.Domain.Entities;
using VibeToolsWebApp.Persistence.DatabaseContext;

namespace VibeToolsWebApp.Persistence.Repositories
{
    public class ToolRepository : IToolRepository
    {
        private readonly VibeToolsDatabaseContext _dbContext;

        public ToolRepository(VibeToolsDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Tool tool)
        {
            _dbContext.Tools.Add(tool);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Tool> GetByIdAsync(Guid id)
        {
            return await _dbContext.Tools
                            .Include(t => t.Reviews)
                            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tool>> SearchAsync(string? searchTerm)
        {
            var query = _dbContext.Tools
                           .Include(t => t.Reviews)
                           .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t => t.Name.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }

        public async Task UpdateAsync(Tool tool)
        {
            _dbContext.Tools.Update(tool);
            await _dbContext.SaveChangesAsync();
        }
    }
}
