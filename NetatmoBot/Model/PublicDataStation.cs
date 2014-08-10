using System.Collections.Generic;
using System.Diagnostics;
using NetatmoBot.Model.Measurements;

namespace NetatmoBot.Model
{
    [DebuggerDisplay("Station: {Id}, Location: {Place}")]
    public class PublicDataStation
    {
        public string Id { get; set; }
        public StationPlace Place { get; set; }
        public List<SensorMeasurement> Measurements { get; set; }
    }
}