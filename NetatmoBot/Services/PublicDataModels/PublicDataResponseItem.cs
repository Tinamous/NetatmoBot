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

        public Dictionary<string, Measurement> measures { get; set; }

    }
}