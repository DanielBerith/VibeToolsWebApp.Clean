using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeToolsWebApp.Domain.Entities;

namespace VibeToolsWebApp.Persistence.DatabaseContext
{
    public class VibeToolsDatabaseContext : DbContext
    {
        public VibeToolsDatabaseContext(DbContextOptions<VibeToolsDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tool> Tools { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VibeToolsDatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
