using System;
using System.Diagnostics;

namespace NetatmoBot.Model.Measurements
{
    [DebuggerDisplay("Pressure: {Value}")]
    public class PressureMeasurement : SensorMeasurement
    {
        public PressureMeasurement(string moduleKey, DateTime? timeStamp, decimal value)
            : base(moduleKey, timeStamp, value)
        { }
    }
}