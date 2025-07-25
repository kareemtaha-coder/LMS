using LMS.Application.Abstractions.Data;
using LMS.Domain.Abstractions;
using LMS.Domain.Curriculums;
using LMS.Infrastructure.Persistence;
using LMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                 throw new ArgumentNullException(nameof(configuration));

            // This single line correctly registers the DbContext with a Scoped lifetime
            // AND maps the IApplicationDbContext interface to the ApplicationDbContext implementation.
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // The manual registration is no longer needed.

            // Register the repository
            services.AddScoped<ICurriculumRepository, CurriculumRepository>();

            // Register the Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}