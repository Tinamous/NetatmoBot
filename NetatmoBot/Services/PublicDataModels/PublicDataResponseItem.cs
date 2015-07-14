using System.Collections.Generic;

namespace NetatmoBot.Services.PublicDataModels
{
    public class PublicDataResponseItem
    {
        public string _id { get; set; }

        /// <summary>
        /// Informations about the localisation of the Weather Station
        /// </summary>
        public Place place { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public string mark { get; set; }

        /// <summary>
        /// Measures (sensor measurements)
        /// </summary>
        /// <example>
        /// "measures":{
        /// "02:00:00:01:f4:9e":{"res":{"1436098731":[20,66]},"type":["temperature","humidity"]},
        /// "05:00:00:00:9b:5c":{"rain_60min":0,"rain_24h":0,"rain_live":0,"rain_timeutc":1436098750},
        /// "70:ee:50:01:8b:28":{"res":{"1436098756":[1017.2]},"type":["pressure"]}},
        /// </example>
        public Dictionary<string, Measurement> measures { get; set; }

        public string[] modules { get; set; }

    }
}