using System;
using NetatmoBot.Model;

namespace NetatmoBot.Helpers.Location
{
    public class LocationHelper
    {
        /// <summary>
        /// Compute distance between two points. From http://www.geodatasource.com/developers/c-sharp
        /// </summary>
        /// <param name="point2"></param>
        /// <param name="unit"></param>
        /// <param name="point1"></param>
        /// <returns></returns>
        public static Distance ComputeDistance(LatLongPoint point1, LatLongPoint point2, DistanceUnit unit)
        {
            double theta = point1.Longitude - point1.Longitude;

            double distanceRadians = Math.Sin(DegreesToRadians(point1.Latitude)) *
                Math.Sin(DegreesToRadians(point2.Latitude)) +
                    Math.Cos(DegreesToRadians(point1.Latitude)) *
                    Math.Cos(DegreesToRadians(point2.Latitude)) *
                    Math.Cos(DegreesToRadians(theta));

            distanceRadians = Math.Acos(distanceRadians);

            var distanceDegrees = RadiansToDegrees(distanceRadians);

            distanceDegrees = distanceDegrees * 60 * 1.1515;

            return CreateDistanceByUnit(unit, distanceDegrees);
        }

        private static Distance CreateDistanceByUnit(DistanceUnit unit, double distanceMiles)
        {
            switch (unit)
            {
                case DistanceUnit.Miles:
                    return new Distance
                    {
                        Value = distanceMiles,
                        Unit = DistanceUnit.Miles
                    };
                case DistanceUnit.Kilometers:
                    return new Distance
                    {
                        Value = distanceMiles * 1.609344D,
                        Unit = DistanceUnit.Kilometers
                    };
                case DistanceUnit.NauticalMiles:
                    return new Distance
                    {
                        Value = distanceMiles * 0.8684D,
                        Unit = DistanceUnit.NauticalMiles
                    };
                default:
                    throw new Exception("Unknown distance unit: " + unit);
            }
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