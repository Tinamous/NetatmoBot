using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetatmoBot.Helpers.Location;
using NetatmoBot.Model.Measurements;

namespace NetatmoBot.Model
{
    [DebuggerDisplay("Station: {Id}, Location: {Place}")]
    public class PublicDataStation
    {
        public string Id { get; set; }
        public StationPlace Place { get; set; }
        public List<SensorMeasurement> Measurements { get; set; }

        public Distance ComputeDistanceAway(LatLongPoint from)
        {
            LatLongPoint stationPoint = new LatLongPoint(Place.Lattitude, Place.Longitude);
            return LocationHelper.ComputeDistance(stationPoint, from);
        }

        public bool HasRainGauge()
        {
            return Measurements.Any(x => x is RainMeasurement);
        }

        public bool IsItRaining(decimal threshold)
        {
            var rainMeasurement = Measurements.FirstOrDefault(y => y is RainMeasurement);
            if (rainMeasurement != null)
            {
                return rainMeasurement.Value > threshold;
            }

            // Can't tell.
            return false;
        }

        public bool HasWindGauge()
        {
            return Measurements.Any(x => x is WindMeasurement);
        }
    }
}