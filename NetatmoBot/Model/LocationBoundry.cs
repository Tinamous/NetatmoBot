using System;
using NetatmoBot.Helpers.Location;

namespace NetatmoBot.Model
{
    public class LocationBoundry
    {
        // Semi-axes of WGS-84 geoidal reference
        private const double WGS84_a = 6378137.0; // Major semiaxis [m]
        private const double WGS84_b = 6356752.3; // Minor semiaxis [m]

        public LatLongPoint NorthEast { get; set; }
        public LatLongPoint SouthWest { get; set; }

        ///
        /// Taken from http://stackoverflow.com/questions/238260/how-to-calculate-the-bounding-box-for-a-given-lat-lng-location?lq=1
        /// See also http://janmatuschek.de/LatitudeLongitudeBoundingCoordinates
        // 'halfSideInKm' is the half length of the bounding box you want in kilometers.

        /// <summary>
        /// Compute the a box around the center point of 2x halfSideInKm.
        /// </summary>
        /// <param name="point">Center point to build a box around</param>
        /// <param name="halfSideInKm">Half the size of the width/height of the box.</param>
        /// <returns></returns>
        public static LocationBoundry ComputeBoundry(LatLongPoint point, double halfSideInKm)
        {
            // Bounding box surrounding the point at given coordinates,
            // assuming local approximation of Earth surface as a sphere
            // of radius given by WGS84
            var latitude = LocationHelper.DegreesToRadians(point.Latitude);
            var longitude = LocationHelper.DegreesToRadians(point.Longitude);
            var halfSide = 1000 * halfSideInKm;

            // Radius of Earth at given latitude
            var radius = WGS84EarthRadius(latitude);
            // Radius of the parallel at given latitude
            var pradius = radius * Math.Cos(latitude);

            var latMin = latitude - halfSide / radius;
            var latMax = latitude + halfSide / radius;
            var lonMin = longitude - halfSide / pradius;
            var lonMax = longitude + halfSide / pradius;

            return new LocationBoundry
            {
                NorthEast = new LatLongPoint
                {
                    Latitude = LocationHelper.RadiansToDegrees(latMax),
                    Longitude = LocationHelper.RadiansToDegrees(lonMax)
                },
                SouthWest = new LatLongPoint
                {
                    Latitude = LocationHelper.RadiansToDegrees(latMin),
                    Longitude = LocationHelper.RadiansToDegrees(lonMin)
                }
            };
        }

        // Earth radius at a given latitude, according to the WGS-84 ellipsoid [m]
        private static double WGS84EarthRadius(double lat)
        {
            // http://en.wikipedia.org/wiki/Earth_radius
            var An = WGS84_a * WGS84_a * Math.Cos(lat);
            var Bn = WGS84_b * WGS84_b * Math.Sin(lat);
            var Ad = WGS84_a * Math.Cos(lat);
            var Bd = WGS84_b * Math.Sin(lat);
            return Math.Sqrt((An * An + Bn * Bn) / (Ad * Ad + Bd * Bd));
        }



        public override string ToString()
        {
            return string.Format("MinPoint: {0}, MaxPoint: {1}", NorthEast, SouthWest);
        }
    }
}