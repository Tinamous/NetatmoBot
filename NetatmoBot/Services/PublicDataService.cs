using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using NetatmoBot.Exceptions;
using NetatmoBot.Extensions;
using NetatmoBot.Model;
using NetatmoBot.Model.Measurements;
using NetatmoBot.Services.PublicDataModels;

namespace NetatmoBot.Services
{
    public class PublicDataService
    {
        private readonly AuthenticationToken _authenticationToken;
        readonly Uri _uri = new Uri("https://api.netatmo.net/api/getpublicdata");

        public PublicDataService(AuthenticationToken authenticationToken)
        {
            if (authenticationToken == null) throw new ArgumentNullException("authenticationToken");
            _authenticationToken = authenticationToken;
        }

        public PublicData Get(LocationBoundry boundry)
        {
            string url = string.Format("{0}?access_token={1}&lat_ne={2}&lon_ne={3}&lat_sw={4}&lon_sw={5}&filter={6}",
                _uri,
                _authenticationToken.Token,
                boundry.NorthEast.Latitude,
                boundry.NorthEast.Longitude,
                boundry.SouthWest.Latitude,
                boundry.SouthWest.Longitude,
                false);

            var client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                Trace.WriteLine("Public data Failed!");
                throw new NetatmoReadException("Failed to read public data. Status code: " + response.StatusCode);
            }

            var publicDataResponse = response.Content.ReadAsAsync<PublicDataResponse>().Result;
            return Map(publicDataResponse);
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
                    Place = Map(publicDataResponseItem.place),
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

                for (int i = 0; i < measure.Value.type.Length; i++)
                {
                    string measurementType = measure.Value.type[i];
                    // For now assume their is only ever one set or results in the res dictionary.
                    long timeStamp = Convert.ToInt64(measure.Value.res.Keys.First());
                    var sensorModuleValue = measure.Value.res.Values.First()[i];

                    var sensorMeasurement = SensorMeasurementFactory.Create(moduleKey, measurementType, DateTime.Now.FromUnixTicks(timeStamp), sensorModuleValue);
                    measurements.Add(sensorMeasurement);
                }
            }

            return measurements;
        }      

        private StationPlace Map(Place place)
        {
            return new StationPlace
            {
                Altitude = place.altitude,
                Lattitude = place.location[0],
                Longitude = place.location[1],
                Timezone = place.timezone
            };
        }
    }
}
