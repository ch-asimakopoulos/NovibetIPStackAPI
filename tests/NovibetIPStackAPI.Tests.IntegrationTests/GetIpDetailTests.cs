using Microsoft.Extensions.Logging.Abstractions;
using NovibetIPStackAPI.Core.Interfaces.IPRelated;
using NovibetIPStackAPI.Core.Models.IPRelated.DTOs;
using NovibetIPStackAPI.IPStackWrapper.Exceptions;
using NovibetIPStackAPI.IPStackWrapper.Services;
using NovibetIPStackAPI.Tests.Kernel;
using System.Collections.Generic;
using Xunit;

namespace NovibetIPStackAPI.Tests.IntegrationTests.IPDetailTests
{
    public class GetIPDetails
    {

        [Fact]
        public void GetsIPDetailsSuccessfully()
        {

            //Arrange
            IntegrationTestsStartup integrationTestsStartup = new IntegrationTestsStartup();
            string ip = "141.237.3.189";
            IPDetails detailsGolden = new IPDetailsDTO()
            {
                City = "Agios Dimitrios",
                Continent = "Europe",
                Country = "Greece",
                Latitude = 37.93333053588867,
                Longitude = 23.75
            };

            //Act
            IPDetails details = new IPInfoProvider(integrationTestsStartup.Configuration, new NullLogger<IPInfoProvider>()).GetDetails(ip);

            //Assert
            (bool, List<string>) AssertEqual = AreEqualHelper.HasEqualPropertyValues<IPDetails>(detailsGolden, details, null);

            Assert.True(AssertEqual.Item1);
        }

        [Fact]
        public void GetIPDetailsFromIPStackAPIWithInvalidIPThrowsIPServiceNotAvailableException()
        {

            //Arrange
            IntegrationTestsStartup integrationTestsStartup = new IntegrationTestsStartup();
            string ip = "novibet";

            //Assert
            Assert.Throws<IPServiceNotAvailableException>(() => new IPInfoProvider(integrationTestsStartup.Configuration, new NullLogger<IPInfoProvider>()).GetDetails(ip));
        }
    }
}
