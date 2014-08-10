using System;
using System.Collections.Generic;
using System.Linq;
using NetatmoBot.Model;
using NetatmoBot.Model.Measurements;
using NetatmoBot.Model.Modules;
using NetatmoBot.Services;
using NUnit.Framework;

namespace NetatmoBot.Tests.Services
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

            var userService = new UserService(_authenticationToken);
            _user = userService.Get();
        }

        [Test]
        [TestCase(typeof(RainModule))]
        public void GetMeasurements_ForModule(Type moduleType)
        {
            // Arrange
            var measurementService = new MeasurementsService(_authenticationToken);
            string firstDevice = _user.DeviceIds.First();

            var devicesService = new DevicesService(_authenticationToken);
            var stationDetails = devicesService.Get();
            Module module = stationDetails.Modules.First(x => x.GetType() == moduleType);

            // Act
            List<SensorMeasurement> measurements = measurementService.Get(firstDevice, module);

            // Assert
            Assert.IsNotNull(measurements);
        }
    }
}