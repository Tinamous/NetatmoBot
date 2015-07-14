using System;
using System.Collections.Generic;
using System.Linq;
using NetatmoBot.Model;
using NetatmoBot.Model.Measurements;
using NetatmoBot.Model.Modules;
using NetatmoBot.Services;
using NetatmoBot.Services.Wrappers;
using NUnit.Framework;

namespace NetatmoBot.IntegrationTests.Services
{
    public class MeasurementsServiceTest
    {
        private AuthenticationToken _authenticationToken;
        private User _user;

        [TestFixtureSetUp]
        public void SetUp()
        {
            // Arrange
            var authenticationService = new AuthenticationService();
            _authenticationToken = authenticationService.AuthenticateUser();

            var userService = new UserService(_authenticationToken, new HttpWrapper());
            _user = userService.Get().Result;
        }

        [Test]
        [TestCase(typeof(RainModule))]
        public void GetMeasurements_ForModule(Type moduleType)
        {
            // Arrange
            string firstDevice = _user.DeviceIds.First();

            // Get the (rain) module to look up measurements from.
            var devicesService = new DevicesService(_authenticationToken, new HttpWrapper());
            var stationDetails = devicesService.Get().Result;
            Module module = stationDetails.Modules.First(x => x.GetType() == moduleType);

            var measurementService = new MeasurementsService(_authenticationToken, new HttpWrapper());

            // Act
            List<SensorMeasurement> measurements = measurementService.Get(firstDevice, module).Result;

            // Assert
            Assert.IsNotNull(measurements);
        }
    }
}