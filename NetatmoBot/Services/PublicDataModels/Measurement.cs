using System.Collections.Generic;

namespace NetatmoBot.Services.PublicDataModels
{
    public class Measurement
    {
        public Dictionary<string, decimal[]> res { get; set; }
        public string[] type { get; set; }
    }
}