using System;
using System.Diagnostics;

namespace NetatmoBot.Model.Measurements
{
    [DebuggerDisplay("Temperature: {Value}°C")]
    public class TemperatureMeasurement : SensorMeasurement
    {
        public TemperatureMeasurement(string moduleKey, DateTime timeStamp, decimal value)
            : base(moduleKey, timeStamp, value)
        { }
    }
}