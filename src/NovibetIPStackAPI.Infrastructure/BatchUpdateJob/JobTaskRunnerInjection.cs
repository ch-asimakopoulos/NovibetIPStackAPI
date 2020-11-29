using NovibetIPStackAPI.Infrastructure.BatchUpdateJob.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NovibetIPStackAPI.Infrastructure.BatchUpdateJob
{
    /// <summary>
    /// This class contains all the Task Asynchronous pattern related registrations that are needed for the Task that implements the batch update job.
    /// </summary>
    public static class JobTaskRunnerInjection
    {
        /// <summary>
        /// This method contains all the service registrations that are needed for the Task Runner.
        /// </summary>
        public static IServiceCollection InjectTaskRunner(this IServiceCollection services)
        {

            services.AddScoped<IBatchUpdateJobTaskRunner, BatchUpdateJobTaskRunner>();

            return services;
        }
    }
}
