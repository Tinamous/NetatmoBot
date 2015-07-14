using NetatmoBot.Helpers.Location;

namespace NetatmoBot.Model
{
    public class Distance
    {
        public double Value { get; set; }
        public DistanceUnit Unit { get; set; }

        public override string ToString()
        {
            switch (Unit)
            {
                    case DistanceUnit.Kilometers:
                        return string.Format("{0}km", Value);
                    case DistanceUnit.Miles:
                        return string.Format("{0}miles", Value);
                    case DistanceUnit.NauticalMiles:
                        return string.Format("{0}nm", Value);
                default:
                    return Value.ToString();
            }
        }
    }
}