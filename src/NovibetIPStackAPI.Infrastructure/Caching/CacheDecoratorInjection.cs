using NovibetIPStackAPI.Infrastructure.Persistence.Caching.Interfaces;
using NovibetIPStackAPI.Infrastructure.Persistence.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace NovibetIPStackAPI.Infrastructure.Caching
{
    /// <summary>
    /// This class contains all the cache related registrations that are needed for the Caching layer that implements the decorator pattern.
    /// </summary>
    public static class CacheDecoratorInjection
    {
        /// <summary>
        /// This method contains all the service registrations that are needed for the Web Api Services.
        /// </summary>
        public static IServiceCollection InjectCacheDecorator(this IServiceCollection services)
        {

            services.AddTransient<ICachedIPDetailsRepositoryDecorator, CachedIPDetailsRepositoryDecorator>();

            return services;
        }
    }
}
