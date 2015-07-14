using System;
using System.Diagnostics;

namespace NetatmoBot.Model.Measurements
{
    [DebuggerDisplay("CO2: {Value}")]
    public class CarbonDioxideMeasurement : SensorMeasurement
    {
        public CarbonDioxideMeasurement(string moduleKey, DateTime? timeStamp, decimal sensorModuleValue)
            : base(moduleKey, timeStamp, sensorModuleValue) 
        { }
    }
}