using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VibeToolsWebApp.Application.Interfaces;
using VibeToolsWebApp.Persistence.DatabaseContext;
using VibeToolsWebApp.Persistence.Repositories;

namespace VibeToolsWebApp.Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VibeToolsDatabaseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("VibeToolsDatabaseConnectionString"));
            });

            services.AddScoped<IToolRepository, ToolRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            return services;
        }
    }
}
