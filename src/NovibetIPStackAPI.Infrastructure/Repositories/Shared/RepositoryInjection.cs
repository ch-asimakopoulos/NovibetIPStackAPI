using Microsoft.Extensions.DependencyInjection;
using NovibetIPStackAPI.Infrastructure.Repositories.BatchRelated;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.BatchRelated;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.IPRelated;
using NovibetIPStackAPI.Infrastructure.Repositories.IPRelated;

namespace NovibetIPStackAPI.Infrastructure.Repositories
{
    /// <summary>
    /// This class contains all the repository registrations that are needed for the Peristence layer.
    /// </summary>
    public static class RepositoryInjection
    {
        /// <summary>
        /// This method contains all the service registrations that are needed for the Web Api Services.
        /// </summary>
        public static IServiceCollection InjectRepositories(this IServiceCollection services)
        {
            services.AddTransient<IJobRepository, JobRepository>();

            return services;
        }
    }
}
