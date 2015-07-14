using NetatmoBot.Model;
using NetatmoBot.Services;
using NetatmoBot.Services.Wrappers;
using NUnit.Framework;

namespace NetatmoBot.IntegrationTests.Services
{
    [TestFixture]
    public class DevicesServiceTest
    {
        private AuthenticationToken _authenticationToken;

        [TestFixtureSetUp]
        public void SetUp()
        {
            // Arrange
            var authenticationService = new AuthenticationService();
            _authenticationToken = authenticationService.AuthenticateUser();
        }

        [Test]
        public void GetDevices()
        {
            // Arrange
            var devicesService = new DevicesService(_authenticationToken, new HttpWrapper());

            // Act
            var stationDetails = devicesService.Get().Result;

            // Arrange
            Assert.IsNotNull(stationDetails, "Station Details");
            Assert.IsNotNull(stationDetails.Modules, "Modules");
            Assert.IsNotNull(stationDetails.Devices, "Devices");
        }
    }
}