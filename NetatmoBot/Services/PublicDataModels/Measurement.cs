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

        public bool IsRain()
        {
            if (rain_60min != null)
            {
                return true;
            }
            return false;
        }
    }
}