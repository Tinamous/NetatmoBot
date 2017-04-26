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

        //private static Distance CreateDistanceByUnit(DistanceUnit unit, double distanceMiles)
        //{
        //    switch (unit)
        //    {
        // Assumes base unit is miles which it is not now, it is meters
        //        case DistanceUnit.Miles:
        //            return new Distance
        //            {
        //                Value = distanceMiles,
        //                Unit = DistanceUnit.Miles
        //            };
        //        case DistanceUnit.Kilometers:
        //            return new Distance
        //            {
        //                Value = distanceMiles * 1.609344D,
        //                Unit = DistanceUnit.Kilometers
        //            };
        //        case DistanceUnit.NauticalMiles:
        //            return new Distance
        //            {
        //                Value = distanceMiles * 0.8684D,
        //                Unit = DistanceUnit.NauticalMiles
        //            };
        //        default:
        //            throw new Exception("Unknown distance unit: " + unit);
        //    }
        //}
    }
}