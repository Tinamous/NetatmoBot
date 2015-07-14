namespace NetatmoBot.Model
{
    public class LatLongPoint
    {
        public LatLongPoint()
        { }

        public LatLongPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1}", Latitude, Longitude);
        }
    }
}