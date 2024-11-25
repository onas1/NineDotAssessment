

using Microsoft.EntityFrameworkCore;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Infrastructure.Persistence;
using NineDotAssessment.Infrastructure.Services;

namespace NineDotAssessment.Infrastructure;
/// <summary>
///  Class for service Configuration
/// </summary>
public static class ConfigurService
{

        /// <summary>
        /// Configure Infrastructure Services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IJWTService, JWTService>();


        return services;
        }
    }
