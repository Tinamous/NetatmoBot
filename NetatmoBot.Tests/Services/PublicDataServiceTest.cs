using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using NetatmoBot.Model;
using NetatmoBot.Model.Measurements;
using NetatmoBot.Services;
using NUnit.Framework;

namespace NetatmoBot.Tests.Services
{
    [TestFixture]
    public class PublicDataServiceTest
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
        public void GetPublicData()
        {
            // Arrange
            var publicDataService = new PublicDataService(_authenticationToken);

            double latitute = Convert.ToDouble(ConfigurationManager.AppSettings["Latitude"]);
            double longitude = Convert.ToDouble(ConfigurationManager.AppSettings["Longitude"]);

            var center = new LatLongPoint(latitute, longitude);
            LocationBoundry boundry = LocationBoundry.ComputeBoundry(center);

            // Act           
            PublicData publicData = publicDataService.Get(boundry);

            // Assert
            Assert.IsNotNull(publicData);


            IList<SensorMeasurement> rainStations = new List<SensorMeasurement>();
            foreach (var station in publicData.Stations)
            {
                var rainMeasurement = station.Measurements.FirstOrDefault(y => y is RainMeasurement);
                if (rainMeasurement != null)
                {
                    rainStations.Add(rainMeasurement);
                }
            }

            Trace.WriteLine("All Stations: ");
            ShowComputedStatistics(rainStations);

            Trace.WriteLine("Stations with Rain: ");
            var stationsWithRain = rainStations.Where(x => x.Value > 0.1M).ToList();
            ShowComputedStatistics(stationsWithRain);
        }

        private void ShowComputedStatistics(IList<SensorMeasurement> measurements)
        {
            decimal average = measurements.Average(x => x.Value);
            decimal count = measurements.Count();
            decimal min = measurements.Min(x => x.Value);
            decimal max = measurements.Max(x => x.Value);

            Trace.WriteLine("Average Rain: " + average);
            Trace.WriteLine("Min: " + min);
            Trace.WriteLine("Max: " + max);
            Trace.WriteLine("Points: " + count);
        }
    }
}