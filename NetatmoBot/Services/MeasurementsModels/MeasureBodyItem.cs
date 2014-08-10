using System.Collections.Generic;

namespace NetatmoBot.Services.MeasurementsModels
{
    public class MeasureBodyItem
    {
        public long beg_time { get; set; }
        public int step_time { get; set; }
        public IList<IList<decimal?>> value { get; set; }
    }
}