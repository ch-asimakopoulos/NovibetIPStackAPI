using Microsoft.Extensions.DependencyInjection;
using NovibetIPStackAPI.WebApi.Services;
using NovibetIPStackAPI.WebApi.Services.Interfaces;


namespace NovibetIPStackAPI.WebApi
{
    /// <summary>
    /// This class contains all the service registrations that are needed for the Web Api Services.
    /// </summary>
    public static class WebApiServiceInjection
    {
        /// <summary>
        /// This method contains all the service registrations that are needed for the Web Api Services.
        /// </summary>
        public static IServiceCollection InjectWebApiServices(this IServiceCollection services)
        {
            services.AddScoped<IIPDetailsService, IPDetailsService>();
            services.AddScoped<IBatchUpdateService, BatchUpdateService>();
            services.AddScoped<IJobProcessService, JobProcessService>();

            return services;
        }
    }
}
