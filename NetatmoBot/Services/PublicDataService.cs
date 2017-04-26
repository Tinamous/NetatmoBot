using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NetatmoBot.Extensions;
using NetatmoBot.Model;
using NetatmoBot.Model.Measurements;
using NetatmoBot.Services.Mapping;
using NetatmoBot.Services.PublicDataModels;
using NetatmoBot.Services.Wrappers;

namespace NetatmoBot.Services
{
    public class PublicDataService
    {
        private readonly AuthenticationToken _authenticationToken;
        private readonly IHttpWrapper _httpWrapper;
        readonly Uri _uri = new Uri("https://api.netatmo.net/api/getpublicdata");

        public PublicDataService(AuthenticationToken authenticationToken, IHttpWrapper httpWrapper)
        {
            if (authenticationToken == null) throw new ArgumentNullException("authenticationToken");
            if (httpWrapper == null) throw new ArgumentNullException("httpWrapper");
            _authenticationToken = authenticationToken;
            _httpWrapper = httpWrapper;
        }

        public async Task<PublicData> Get(LocationBoundry boundry)
        {
            string url = string.Format(
                    "{0}?access_token={1}&lat_ne={2}&lon_ne={3}&lat_sw={4}&lon_sw={5}&filter={6}",
                    _uri,
                    _authenticationToken.Token,
                    boundry.NorthEast.Latitude,
                    boundry.NorthEast.Longitude,
                    boundry.SouthWest.Latitude,
                    boundry.SouthWest.Longitude,
                    false);

            Trace.WriteLine("NE:" + boundry.NorthEast);
            Trace.WriteLine("SW:" + boundry.SouthWest);

            try
            {
                var result = await _httpWrapper.ReadGet<PublicDataResponse>(url);
                return Map(result);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Url: " + url);
                throw new Exception("Error getting public data.", ex);
            }
        }

        private PublicData Map(PublicDataResponse publicDataResponse)
        {
            return new PublicData
            {
                Stations = Map(publicDataResponse.body)
            };
        }

        private List<PublicDataStation> Map(List<PublicDataResponseItem> publicDataResponseItems)
        {
            var stations = new List<PublicDataStation>();

            foreach (var publicDataResponseItem in publicDataResponseItems)
            {
                var station = new PublicDataStation
                {
                    Id = publicDataResponseItem._id,
                    Place = StationPlaceMapper.Map(publicDataResponseItem.place),
                    Measurements = Map(publicDataResponseItem.measures)
                };

                stations.Add(station);
            }

            return stations;
        }

        private List<SensorMeasurement> Map(Dictionary<string, Measurement> measures)
        {
            var measurements = new List<SensorMeasurement>();

            foreach (var measure in measures)
            {
                string moduleKey = measure.Key;
                var measurement = measure.Value;

                if (measurement != null)
                {
                    measurements.AddRange(BuildSensorMeasurements(moduleKey, measurement));
                }
                else
                {
                    Trace.WriteLine("Measurement null for key: " + moduleKey);
                }
            }

            return measurements;
        }

        private static List<SensorMeasurement> BuildSensorMeasurements(string moduleKey, Measurement measurement)
        {
            List<SensorMeasurement> measurements = new List<SensorMeasurement>();

            if (measurement.IsRain())
            {
                measurements.Add(CreateRainMeasurement(moduleKey, measurement));
                return measurements;
            }

            if (measurement.IsWind())
            {
                measurements.Add(CreateWindnMeasurement(moduleKey, measurement));
                return measurements;
            }

            if (measurement.type == null)
            {
                Trace.WriteLine("Measurement type null for module: " + moduleKey);
                return measurements;
            }

            for (int i = 0; i < measurement.type.Length; i++)
            {
                string measurementType = measurement.type[i];
                // For now assume their is only ever one set or results in the res dictionary.
                long timeStamp = Convert.ToInt64(measurement.res.Keys.First());
                var sensorModuleValue = measurement.res.Values.First()[i];

                DateTime? date = DateTime.Now.FromUnixTicks(timeStamp);
                var sensorMeasurement = SensorMeasurementFactory.Create(moduleKey, measurementType, date, sensorModuleValue);
                measurements.Add(sensorMeasurement);
            }

            return measurements;
        }

        private static SensorMeasurement CreateWindnMeasurement(string moduleKey, Measurement measurement)
        {
            DateTime? date = DateTime.Now.FromUnixTicks(measurement.wind_timeutc);
            var value = measurement.wind_strength;
            if (!value.HasValue)
            {
                return null;
            }

            var rainMeasurement = new WindMeasurement(moduleKey, date, value.Value);
            if (measurement.wind_angle.HasValue)
            {
                rainMeasurement.WindAngle = measurement.wind_angle.Value;
            }

            if (measurement.gust_strength.HasValue)
            {
                rainMeasurement.GustStrength = measurement.gust_strength.Value;
            }

            if (measurement.gust_angle.HasValue)
            {
                rainMeasurement.GustAngle = measurement.gust_angle.Value;
            }
            return rainMeasurement;
        }

        private static SensorMeasurement CreateRainMeasurement(string moduleKey, Measurement measurement)
        {
            DateTime? date = DateTime.Now.FromUnixTicks(measurement.rain_timeutc);
            var value = measurement.rain_live;
            if (!value.HasValue)
            {
                return null;
            }

            var rainMeasurement = new RainMeasurement(moduleKey, date, value.Value);
            if (measurement.rain_24h.HasValue)
            {
                rainMeasurement.MillimetersIn24Hour = measurement.rain_24h.Value;
            }

            if (measurement.rain_60min.HasValue)
            {
                rainMeasurement.MillimetersIn60Minutes = measurement.rain_60min.Value;
            }
            return rainMeasurement;
        }
    }
}
