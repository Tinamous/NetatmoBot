using System;

namespace NetatmoBot.Model.Measurements
{
    public class WindMeasurement : SensorMeasurement
    {
        public WindMeasurement(string moduleKey, DateTime? timeStamp, decimal value)
            : base(moduleKey, timeStamp, value)
        { }

        /// <summary>
        /// Wind Angle (might actually be a int)
        /// </summary>
        public decimal WindAngle { get; set; }

        /// <summary>
        /// Gust Strength (might actually be a int)
        /// </summary>
        public decimal GustStrength { get; set; }

        /// <summary>
        /// Gust Angle (might actually be a int)
        /// </summary>
        public decimal GustAngle { get; set; }
    }
}