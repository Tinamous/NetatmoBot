using System;

namespace NetatmoBot.Model.Measurements
{
    public abstract class SensorMeasurement
    {
        protected SensorMeasurement(string moduleKey, DateTime timeStamp, decimal value)
        {
            ModuleKey = moduleKey;
            TimeStamp = timeStamp;
            Value = value;
        }

        public string ModuleKey { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Value { get; set; }
    }
}