using System;
using NetatmoBot.Model.Measurements;

namespace NetatmoBot.Model
{
    public static class SensorMeasurementFactory
    {
        public static SensorMeasurement Create(string moduleKey, string measurementType, DateTime? timeStamp, decimal sensorModuleValue)
        {
            switch (measurementType.ToLower())
            {
                case "temperature":
                    return new TemperatureMeasurement(moduleKey, timeStamp, sensorModuleValue);
                case "humidity":
                    return new HumidityMeasurement(moduleKey, timeStamp, sensorModuleValue);
                case "pressure":
                    return new PressureMeasurement(moduleKey, timeStamp, sensorModuleValue);
                case "rain":
                    return new RainMeasurement(moduleKey, timeStamp, sensorModuleValue);
                case "co2":
                    return new CarbonDioxideMeasurement(moduleKey, timeStamp, sensorModuleValue);
                case "noise":
                    return new NoiseMeasurement(moduleKey, timeStamp, sensorModuleValue);
                default:
                    return new GenericMeasurement(moduleKey, measurementType, timeStamp, sensorModuleValue);
            }
        }
    }
}