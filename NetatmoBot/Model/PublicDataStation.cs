using System.Collections.Generic;
using System.Diagnostics;
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
            return LocationHelper.ComputeDistance(stationPoint, from, DistanceUnit.Kilometers);
        }
    }
}