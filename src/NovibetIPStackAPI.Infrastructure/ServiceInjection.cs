﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NovibetIPStackAPI.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using NovibetIPStackAPI.Infrastructure.Repositories;
using NovibetIPStackAPI.Infrastructure.BatchUpdateJob;
using NovibetIPStackAPI.Infrastructure.Caching;

namespace NovibetIPStackAPI.Infrastructure
{
    public static class ServiceInjection
    {
        /// <summary>
        /// Injects the infrastructure related services in StartUp using Dependency Injection.
        /// </summary>
        /// <param name="services">The application's service collection</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                                                options.UseSqlServer(configuration["SQLServerConnectionString"]));

            services.InjectRepositories();
            services.InjectCacheDecorator();
            services.InjectTaskRunner();

            return services;
        }
    }
}
