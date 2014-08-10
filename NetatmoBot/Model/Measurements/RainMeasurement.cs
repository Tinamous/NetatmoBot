using System;
using System.Diagnostics;

namespace NetatmoBot.Model.Measurements
{
    [DebuggerDisplay("Rain: {Value}")]
    public class RainMeasurement : SensorMeasurement
    {
        public RainMeasurement(string moduleKey, DateTime timeStamp, decimal value)
            : base(moduleKey, timeStamp, value)
        { }
    }
}