using NetatmoBot.Model;
using NetatmoBot.Services;
using NetatmoBot.Services.AuthenticationModels;
using NUnit.Framework;

namespace NetatmoBot.Tests.Services
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
            var devicesService = new DevicesService(_authenticationToken);

            // Act
            var stationDetails = devicesService.Get();

            // Arrange
            Assert.IsNotNull(stationDetails, "Station Details");
            Assert.IsNotNull(stationDetails.Modules, "Modules");
            Assert.IsNotNull(stationDetails.Devices, "Devices");
        }
    }
}