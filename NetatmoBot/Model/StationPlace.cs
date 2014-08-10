using System.Diagnostics;

namespace NetatmoBot.Model
{
    [DebuggerDisplay("Location: {Lattitude},{Longitude}")]
    public class StationPlace
    {
        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Altitude { get; set; }
        public string Timezone { get; set; }
    }
}