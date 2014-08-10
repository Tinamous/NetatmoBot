namespace NetatmoBot.Model
{
    public class LocationBoundry
    {
        public LatLongPoint NorthEast { get; set; }
        public LatLongPoint SouthWest { get; set; }

        /// <summary>
        /// Compute a square around the center point.
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public static LocationBoundry ComputeBoundry(LatLongPoint center)
        {
            var boundry = new LocationBoundry
            {
                // Hack to create a box of "fixed" size around the center point.
                NorthEast = new LatLongPoint(center.Latitude + 0.35D, center.Longitude + 0.7),
                SouthWest = new LatLongPoint(center.Latitude - 0.35D, center.Longitude - 0.7),
            };

            return boundry;
        }
    }
}