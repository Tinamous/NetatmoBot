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
        private const decimal RainingThreshold = 0.15M;
        static readonly CancellationTokenSource CancellationToken = new CancellationTokenSource();
        private static LatLongPoint _center;

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

            Console.WriteLine("Count\tRain gauges\tWind gauges\tWith Rain\tAverage\tMin\tMax\t");    
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

                var windStations = GetStationsWithWindSensors(publicData);
                int numberOfStationsWithWindGauge = windStations.Count();
                Trace.WriteLine("Number of stations with wind gauge: " + numberOfStationsWithWindGauge);


                ShowComputedStatistics(totalNumberOfStations, numberOfStationsWithRainGauge, stationsWithRain, numberOfStationsWithWindGauge);

                ShowStationDetails(publicData.Stations);


                WaitForNextMeasurementTime(timeBetweenMeasurements);

            } while (!CancellationToken.IsCancellationRequested);
        }

        private static List<PublicDataStation> GetStationsWithWindSensors(PublicData publicData)
        {
            return publicData.Stations.Where(x => x.HasWindGauge()).ToList();
        }

        private static void ShowStationDetails(List<PublicDataStation> publicDataStations)
        {
            var stationsWithRainGauge = publicDataStations.Where(x => x.HasRainGauge()).ToList();
            var stationsWithRain = stationsWithRainGauge.Where(x => x.IsItRaining(RainingThreshold)).ToList();

            int i = 0;
            int lastDistance = 0;
            PublicDataStation closestStationWithRain = null;

            foreach (PublicDataStation publicDataStation in stationsWithRain)
            {
                var rain = publicDataStation.Measurements.First(y => y is RainMeasurement);
                Distance distanceKM = publicDataStation.ComputeDistanceAway(_center);
                int distanceMeters = Convert.ToInt32(distanceKM.Value);
                if (i == 0)
                {
                    lastDistance = distanceMeters;
                    closestStationWithRain = publicDataStation;
                } 
                else 
                {
                    if (lastDistance < distanceMeters)
                    {
                        lastDistance = distanceMeters;
                        closestStationWithRain = publicDataStation;
                    }
                }

                var details = string.Format("Station: {0}. Distance: {1}, Rain Level: {2}", i, distanceMeters , rain.Value);
                Console.WriteLine(details);

                i++;
            }

            if (closestStationWithRain != null)
            {
                Console.WriteLine("Closest Station Id = " + closestStationWithRain.Id);
                double closest = closestStationWithRain.ComputeDistanceAway(_center).Value;

                var stationsCloser =
                    stationsWithRainGauge
                    .Where(x => x.ComputeDistanceAway(_center).Value < closest)
                    .ToList();

                Console.WriteLine("There are " + stationsCloser.Count + " stations closer without rain (that have a rain gauge)");
                foreach (var publicDataStation in stationsCloser)
                {
                    var details = string.Format("Station: {0}. Distance: {1}", publicDataStation.Id, publicDataStation.ComputeDistanceAway(_center));
                    Console.WriteLine(details);
                }
            }
        }

        private static LocationBoundry GetLocationBoundry()
        {
            double latitute = Convert.ToDouble(ConfigurationManager.AppSettings["Latitude"]);
            double longitude = Convert.ToDouble(ConfigurationManager.AppSettings["Longitude"]);

            _center = new LatLongPoint(latitute, longitude);

            // Generate a boundry 2km around the center point.
            LocationBoundry boundry = LocationBoundry.ComputeBoundry(_center, 20);
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

        private static void ShowComputedStatistics(int totalStations, int withRainGauge, IList<SensorMeasurement> measurements, int numberOfStationsWithWindGauge)
        {
            int rounding = 3;
            Console.Write(totalStations + "\t");
            Console.Write(withRainGauge + "\t\t");
            Console.Write(numberOfStationsWithWindGauge+ "\t\t");

            int countWithRain = measurements.Count;
            Console.Write(countWithRain+ "\t\t");

            if (countWithRain > 0)
            {
                decimal average = Math.Round(measurements.Average(x => x.Value), rounding);

                decimal min = Math.Round(measurements.Min(x => x.Value), rounding);
                decimal max = Math.Round(measurements.Max(x => x.Value), rounding);

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
