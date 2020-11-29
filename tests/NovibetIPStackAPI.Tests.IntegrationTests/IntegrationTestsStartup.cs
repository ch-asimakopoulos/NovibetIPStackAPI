using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NovibetIPStackAPI.Infrastructure;
using NovibetIPStackAPI.Infrastructure.Repositories;
using NovibetIPStackAPI.IPStackWrapper;
using NovibetIPStackAPI.WebApi;
using NovibetIPStackAPI.WebApi.Services;
using System.IO;

namespace NovibetIPStackAPI.Tests.IntegrationTests
{
    public class IntegrationTestsStartup
    {
        public readonly IConfiguration Configuration;
        public IntegrationTestsStartup()
        {

            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                .AddJsonFile(
                                                    path: "appsettings.tests.json",
                                                    optional: false,
                                                    reloadOnChange: true)
                                                .AddJsonFile(
                                                    path: "appsettings.tests.Development.json",
                                                    optional: false,
                                                    reloadOnChange: true)
                                                .Build();

            Configuration = configuration;
        }
    }

}
