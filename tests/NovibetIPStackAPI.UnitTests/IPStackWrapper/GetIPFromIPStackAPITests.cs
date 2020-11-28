using NovibetIPStackAPI.Core.Interfaces;
using NovibetIPStackAPI.Core.Models.DTOs;
using NovibetIPStackAPI.IPStackWrapper.Exceptions;
using NovibetIPStackAPI.IPStackWrapper.Services;
using NovibetIPStackAPI.Tests.Kernel;
using System.Collections.Generic;
using Xunit;

namespace NovibetIPStackAPI.UnitTests.IPStackWrapperTests
{
    public class GetIPFromIPStackAPITests
    {

        [Fact]
        public void GetsIPDetailsFromIPStackAPISuccessfully()
        {

            //Arrange
            IPWrapperTestSetup IPWrapperTestSetupObject = new IPWrapperTestSetup();
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
            IPDetails details = new IPInfoProvider(IPWrapperTestSetupObject.Configuration).GetDetails(ip);

            //Assert
            (bool, List<string>) AssertEqual = AreEqualHelper.HasEqualPropertyValues<IPDetails>(detailsGolden, details, null);

            Assert.True(AssertEqual.Item1);
        }

        [Fact]
        public void GetIPDetailsFromIPStackAPIWithInvalidIPThrowsIPServiceNotAvailableException()
        {

            //Arrange
            IPWrapperTestSetup IPWrapperTestSetupObject = new IPWrapperTestSetup();
            string ip = "novibet";

            //Assert
            Assert.Throws<IPServiceNotAvailableException>(() => new IPInfoProvider(IPWrapperTestSetupObject.Configuration).GetDetails(ip));
        }
    }
}
