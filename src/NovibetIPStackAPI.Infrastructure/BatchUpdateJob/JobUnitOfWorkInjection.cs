using NovibetIPStackAPI.Infrastructure.BatchUpdateJob.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NovibetIPStackAPI.Infrastructure.BatchUpdateJob
{
    /// <summary>
    /// This class contains all the cache related registrations that are needed for the Caching layer that implements the decorator pattern.
    /// </summary>
    public static class JobUnitOfWorkInjection
    {
        /// <summary>
        /// This method contains all the service registrations that are needed for the Web Api Services.
        /// </summary>
        public static IServiceCollection InjectJobUnitOfWork(this IServiceCollection services)
        {

            services.AddScoped<IBatchUpdateJobUnitOfWork, BatchUpdateJobUnitOfWork>();

            return services;
        }
    }
}
