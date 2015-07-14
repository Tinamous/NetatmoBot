﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using NetatmoBot.Exceptions;
using NetatmoBot.Extensions;
using NetatmoBot.Model;
using NetatmoBot.Model.Measurements;
using NetatmoBot.Model.Modules;
using NetatmoBot.Services.MeasurementsModels;
using NetatmoBot.Services.Wrappers;

namespace NetatmoBot.Services
{
    public class MeasurementsService
    {
        private readonly Uri _uri = new Uri("https://api.netatmo.net/api/getmeasure");
        private readonly AuthenticationToken _authenticationToken;
        private readonly IHttpWrapper _httpWrapper;

        public MeasurementsService(AuthenticationToken authenticationToken, IHttpWrapper httpWrapper)
        {
            if (authenticationToken == null) throw new ArgumentNullException("authenticationToken");
            if (httpWrapper == null) throw new ArgumentNullException("httpWrapper");
            _authenticationToken = authenticationToken;
            _httpWrapper = httpWrapper;
        }

        public async Task<List<SensorMeasurement>> Get(string deviceId, Module module)
        {
            string type = GetMeasurementType(module);
            string scale = "max";

            string url = string.Format(
                "{0}?access_token={1}&device_id={2}&module_id={3}&type={4}&scale={5}",
                _uri,
                _authenticationToken.Token,
                deviceId,
                module.Id,
                type,
                scale);

            // Optional.
            long dateBegin = DateTime.UtcNow.AddHours(-4).ToUnixTicks();
            url += string.Format("&date_begin={0}", dateBegin);

            // Optional
            //long dateEnd = 1347556500;
            //url += string.Format("&date_end={0}", dateEnd);

            var measureResponse = await _httpWrapper.ReadGet<MeasureResponse>(url);
            return Map(measureResponse.body, type);
        }

        private static string GetMeasurementType(Module module)
        {
            string type;
            if (module is RainModule)
            {
                type = "Rain";
            }
            else
            {
                type = "Temperature,CO2,Humidity,Pressure,Noise";
            }
            return type;
        }

        private List<SensorMeasurement> Map(List<MeasureBodyItem> measureBodyItems, string type)
        {
            string[] types = type.Split(',');

            List<SensorMeasurement> sensorMeasurements = new List<SensorMeasurement>();

            foreach (var measureBodyItem in measureBodyItems)
            {
                sensorMeasurements.AddRange(Map(measureBodyItem, types));
            }

            return sensorMeasurements;
        }

        private IEnumerable<SensorMeasurement> Map(MeasureBodyItem measureBodyItem, string[] types)
        {
            long timeStamp = measureBodyItem.beg_time;

            List<SensorMeasurement> sensorMeasurements = new List<SensorMeasurement>();

            foreach (var measurements in measureBodyItem.value)
            {
                sensorMeasurements.AddRange(Map(measurements, types, timeStamp));
                timeStamp += measureBodyItem.step_time;
            }

            return sensorMeasurements;
        }

        private IEnumerable<SensorMeasurement> Map(IList<decimal?> measureBodyItems, string[] types, long timestamp)
        {
            List<SensorMeasurement> sensorMeasurements = new List<SensorMeasurement>();

            for (int i = 0; i < types.Length; i++)
            {
                decimal? measuredValue = measureBodyItems[i];
                string type = types[i];

                if (measuredValue.HasValue)
                {
                    sensorMeasurements.Add(CreateSensorMeasurement(type, measuredValue.Value, timestamp));
                }
            }

            return sensorMeasurements;
        }

        private SensorMeasurement CreateSensorMeasurement(string type, decimal value, long timestamp)
        {
            return SensorMeasurementFactory.Create("", type, DateTime.Now.FromUnixTicks(timestamp), value);
        }
    }
}