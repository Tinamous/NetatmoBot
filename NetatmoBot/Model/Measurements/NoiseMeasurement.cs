using System;
using System.Diagnostics;

namespace NetatmoBot.Model.Measurements
{
    [DebuggerDisplay("NoiseLevel: {Value}")]
    public class NoiseMeasurement : SensorMeasurement
    {
        public NoiseMeasurement(string moduleKey, DateTime timeStamp, decimal sensorModuleValue)
            : base(moduleKey, timeStamp, sensorModuleValue)
        { }
    }
}