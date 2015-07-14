using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using NetatmoBot.Model;
using NetatmoBot.Model.Measurements;
using NetatmoBot.Services;
using NetatmoBot.Services.Wrappers;
using NUnit.Framework;

namespace NetatmoBot.IntegrationTests.Services
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
        [TestCase(59.924752, 10.703852)] // Oslo, Norway
        [TestCase(55.668847, 12.548651)] // Denmark
        [TestCase(-26.890059, 152.112280)] // Blackbutt, Queensland, Australia
        [TestCase(49.281974, -123.117857)] // Vancouver, BC, Canada
        public void DifferentLocations(double latitute, double longitude)
        {
            // Arrange
            var publicDataService = new PublicDataService(_authenticationToken, new HttpWrapper());

            var center = new LatLongPoint(latitute, longitude);
            LocationBoundry boundry = LocationBoundry.ComputeBoundry(center, 10);

            // Act           
            PublicData publicData = publicDataService.Get(boundry).Result;

            // Assert
            Assert.IsNotNull(publicData);

            ShowStationInfo(publicData.Stations, center);
            ShowRainInfo(publicData);
        }

        [Test]
        public void GetPublicData()
        {
            // Arrange
            var publicDataService = new PublicDataService(_authenticationToken, new HttpWrapper());

            // home
            double latitute = Convert.ToDouble(ConfigurationManager.AppSettings["Latitude"]);
            double longitude = Convert.ToDouble(ConfigurationManager.AppSettings["Longitude"]);

            var center = new LatLongPoint(latitute, longitude);
            LocationBoundry boundry = LocationBoundry.ComputeBoundry(center, 10);

            // Act           
            PublicData publicData = publicDataService.Get(boundry).Result;

            // Assert
            Assert.IsNotNull(publicData);

            ShowStationInfo(publicData.Stations, center);

            ShowRainInfo(publicData);
        }

        private void ShowRainInfo(PublicData publicData)
        {
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

        private void ShowStationInfo(List<PublicDataStation> stations, LatLongPoint center)
        {
            foreach (PublicDataStation station in stations)
            {
                ShowStationDistances(station, center);
            }
        }

        private void ShowStationDistances(PublicDataStation station, LatLongPoint center)
        {
            Distance distance = station.ComputeDistanceAway(center);
            
            string message = string.Format("Station: {0} is {1} away", station, distance);
            Trace.WriteLine(message);
        }

        private void ShowComputedStatistics(IList<SensorMeasurement> measurements)
        {
            decimal count = measurements.Count();
            Trace.WriteLine("Points: " + count);

            if (count > 0)
            {
                decimal average = measurements.Average(x => x.Value);
                decimal min = measurements.Min(x => x.Value);
                decimal max = measurements.Max(x => x.Value);

                Trace.WriteLine("Average Rain: " + average);
                Trace.WriteLine("Min: " + min);
                Trace.WriteLine("Max: " + max);
            }
        }
    }
}