using System.Diagnostics;

namespace NetatmoBot.Model
{
    [DebuggerDisplay("Location: {Lattitude},{Longitude}")]
    public class StationPlace
    {
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public string Timezone { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1}", Lattitude, Longitude);
        }
    }
}