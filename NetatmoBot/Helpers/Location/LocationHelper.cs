using System;
using System.Device.Location;
using NetatmoBot.Model;

namespace NetatmoBot.Helpers.Location
{
    public class LocationHelper
    {
        /// <summary>
        /// Compute distance between two points.
        /// </summary>
        /// <param name="point2"></param>
        /// <param name="point1"></param>
        /// <see cref="https://msdn.microsoft.com/en-us/library/system.device.location.geocoordinate.aspx"/>
        /// <returns></returns>
        public static Distance ComputeDistance(LatLongPoint point1, LatLongPoint point2)
        {
            var p1 = new GeoCoordinate(point1.Latitude, point1.Longitude);
            var p2 = new GeoCoordinate(point2.Latitude, point2.Longitude);

            var distanceMeters = p1.GetDistanceTo(p2);

            return new Distance { Value = distanceMeters, Unit = DistanceUnit.Meters };
        }

        public static double DegreesToRadians(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        public static double RadiansToDegrees(double radians)
        {
            return 180.0 * radians / Math.PI;
        }
    }
}