using System;
using System.Diagnostics;

namespace NetatmoBot.Model.Measurements
{
    [DebuggerDisplay("{type}: {Value}")]
    public class GenericMeasurement : SensorMeasurement
    {
        public GenericMeasurement(string moduleKey, string type, DateTime timeStamp, decimal value)
            : base(moduleKey, timeStamp, value)
        {
            Type = type;
        }

        public string Type { get; set; }
    }
}