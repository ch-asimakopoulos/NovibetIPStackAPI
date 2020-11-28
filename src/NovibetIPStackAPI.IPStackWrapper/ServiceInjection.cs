using NovibetIPStackAPI.IPStackWrapper.Services;
using NovibetIPStackAPI.IPStackWrapper.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NovibetIPStackAPI.IPStackWrapper
{
    public static class ServiceInjection
    {
        /// <summary>
        /// Injects IPStack's API Wrapper provider service in StartUp using Dependency Injection.
        /// </summary>
        /// <param name="services">The application's service collection</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddIPInfoProvider(this IServiceCollection services)
        {
            services.AddTransient<IIPInfoProvider, IPInfoProvider>();
            return services;
        }
    }
}
