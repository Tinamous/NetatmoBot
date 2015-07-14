using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NetatmoBot.Model;
using NetatmoBot.Model.Measurements;
using NetatmoBot.Services;
using NetatmoBot.Services.Wrappers;

namespace NetatmoBot.ConsoleHost
{
    internal class Program
    {
        private static AuthenticationToken _authenticationToken;
        private const decimal RainingThreshold = 0.1M;
        static readonly CancellationTokenSource CancellationToken = new CancellationTokenSource();

        private static void Main(string[] args)
        {
            // Arrange
            var authenticationService = new AuthenticationService();
            _authenticationToken = authenticationService.AuthenticateUser();

            Task task = Task.Factory.StartNew(GetRainMeasurements, CancellationToken.Token);

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();

            CancellationToken.Cancel();
            task.Wait();
        }

        private static void GetRainMeasurements()
        {
            var timeBetweenMeasurements = new TimeSpan(0, 0, 1, 0);
            var publicDataService = new PublicDataService(_authenticationToken, new HttpWrapper());

            var boundry = GetLocationBoundry();

            Console.WriteLine("Count\tWith gauge\tWith Rain\tAverage\tMin\tMax\t");    
            do
            {
                if (_authenticationToken.IsCloseToExpiry())
                {
                    RefreshToken();
                }

                // Get the public data
                PublicData publicData = publicDataService.Get(boundry).Result;

                // The total number of stations returned in the geographic area.
                int totalNumberOfStations = publicData.Stations.Count();
                Trace.WriteLine("Total number of stations: " + totalNumberOfStations);

                // The stations that have a rain gauge.
                var rainStations = GetStationsWithRainSensors(publicData);
                int numberOfStationsWithRainGauge = rainStations.Count();
                Trace.WriteLine("Number of stations with rain gauge: " + numberOfStationsWithRainGauge);

                // Stations where the rain gauge is reporting a level above the threshold.
                var stationsWithRain = rainStations.Where(x => x.Value > RainingThreshold).ToList();

                ShowComputedStatistics(totalNumberOfStations, numberOfStationsWithRainGauge, stationsWithRain);

                WaitForNextMeasurementTime(timeBetweenMeasurements);

            } while (!CancellationToken.IsCancellationRequested);
        }

        private static LocationBoundry GetLocationBoundry()
        {
            double latitute = Convert.ToDouble(ConfigurationManager.AppSettings["Latitude"]);
            double longitude = Convert.ToDouble(ConfigurationManager.AppSettings["Longitude"]);

            var center = new LatLongPoint(latitute, longitude);

            // Generate a boundry 2km around the center point.
            LocationBoundry boundry = LocationBoundry.ComputeBoundry(center, 20);
            return boundry;
        }

        private static void RefreshToken()
        {
            var authenticationService = new AuthenticationService();
            _authenticationToken = authenticationService.RefreshToken(_authenticationToken);
        }

        private static void WaitForNextMeasurementTime(TimeSpan sleepTime)
        {
            DateTime nextMeasurementTime = DateTime.UtcNow.Add(sleepTime);
            do
            {
                if (CancellationToken.IsCancellationRequested)
                {
                    return;
                }

                // Sleep for a second.
                Thread.Sleep(1000);
            } while (nextMeasurementTime.ToUniversalTime() > DateTime.UtcNow);
        }

        private static IList<SensorMeasurement> GetStationsWithRainSensors(PublicData publicData)
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
            return rainStations;
        }

        private static void ShowComputedStatistics(int totalStations, int withRainGauge, IList<SensorMeasurement> measurements)
        {
            Console.Write(totalStations + "\t");
            Console.Write(withRainGauge + "\t\t");

            int countWithRain = measurements.Count;
            Console.Write(countWithRain+ "\t\t");

            if (countWithRain > 0)
            {
                decimal average = Math.Round(measurements.Average(x => x.Value),2);

                decimal min = measurements.Min(x => x.Value);
                decimal max = measurements.Max(x => x.Value);

                Console.Write(average + "\t");
                Console.Write(min + "\t");
                Console.Write(max + "\t");   
            }
            else
            {
                Console.Write("It's Not Raining :-)");
            }
            Console.WriteLine();
        }
    }
}
