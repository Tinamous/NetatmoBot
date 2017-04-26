using System.Collections.Generic;

namespace NetatmoBot.Services.PublicDataModels
{
    /// <summary>
    /// Measurement will be either a collection or res/types or rain_xxxx
    /// </summary>
    public class Measurement
    {
        public Dictionary<string, decimal[]> res { get; set; }
        public string[] type { get; set; }

        #region Rain Gauge Measurements

        /// <summary>
        /// 60 minute rain (mm)
        /// </summary>
        public decimal? rain_60min { get; set; }

        /// <summary>
        /// 24 hour rain (mm)
        /// </summary>
        public decimal? rain_24h { get; set; }

        /// <summary>
        /// Live rain (mm)
        /// </summary>
        public decimal? rain_live { get; set; }

        /// <summary>
        /// UTC time
        /// </summary>
        public long? rain_timeutc { get; set; }

        #endregion

        #region Wind Gauge Measurements

        /// <summary>
        /// Wind Strength (might actually be a int)
        /// </summary>
        public decimal? wind_strength { get; set; }

        /// <summary>
        /// Wind Angle (might actually be a int)
        /// </summary>
        public decimal? wind_angle { get; set; }

        /// <summary>
        /// Gust Strength (might actually be a int)
        /// </summary>
        public decimal? gust_strength { get; set; }

        /// <summary>
        /// Gust Angle (might actually be a int)
        /// </summary>
        public decimal? gust_angle { get; set; }

        /// <summary>
        /// Wind Time (might actually be a int)
        /// </summary>
        public int? wind_timeutc { get; set; }

        #endregion

        public bool IsRain()
        {
            return rain_60min.HasValue;
        }

        public bool IsWind()
        {
            return wind_timeutc.HasValue;
        }
    }
}