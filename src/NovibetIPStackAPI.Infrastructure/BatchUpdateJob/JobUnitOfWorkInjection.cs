using NovibetIPStackAPI.Infrastructure.BatchUpdateJob.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NovibetIPStackAPI.Infrastructure.BatchUpdateJob
{
    /// <summary>
    /// This class contains all the Task Asynchronous and Unit of Work pattern related registrations that are needed for the Task that implements the batch update job.
    /// </summary>
    public static class JobUnitOfWorkInjection
    {
        /// <summary>
        /// This method contains all the service registrations that are needed for the Web Api Services.
        /// </summary>
        public static IServiceCollection InjectJobUnitOfWork(this IServiceCollection services)
        {

            services.AddScoped<IBatchUpdateJobTaskRunner, BatchUpdateJobTaskRunner>();

            return services;
        }
    }
}
