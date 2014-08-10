using System;
using System.Diagnostics;

namespace NetatmoBot.Model.Measurements
{
    [DebuggerDisplay("Humidity: {Value}%")]
    public class HumidityMeasurement : SensorMeasurement
    {
        public HumidityMeasurement(string moduleKey, DateTime timeStamp, decimal value)
            : base(moduleKey, timeStamp, value)
        { }
    }
}