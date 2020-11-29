using NovibetIPStackAPI.Infrastructure.Persistence.Caching;
using Microsoft.Extensions.DependencyInjection;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.IPRelated;

namespace NovibetIPStackAPI.Infrastructure.Caching
{
    /// <summary>
    /// This class contains all the cache related registrations that are needed for the Caching layer that implements the decorator pattern.
    /// </summary>
    public static class CacheDecoratorInjection
    {
        /// <summary>
        /// This method registers all the cache related registrations that are needed for the Caching layer that implements the decorator pattern.
        /// </summary>
        public static IServiceCollection InjectCacheDecorator(this IServiceCollection services)
        {

            services.AddTransient<IIPDetailsRepository, CachedIPDetailsRepositoryDecorator>();

            return services;
        }
    }
}
