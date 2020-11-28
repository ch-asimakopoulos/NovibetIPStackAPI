using NovibetIPStackAPI.IPStackWrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace NovibetIPStackAPI.UnitTests.IPStackWrapperTests
{
    public class IPWrapperTestSetup
    {
        public readonly IConfiguration Configuration;

        public IPWrapperTestSetup()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            
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

            serviceCollection.AddSingleton<IConfiguration>(configuration);

            serviceCollection.AddIPInfoProvider();
        }

    }
}
