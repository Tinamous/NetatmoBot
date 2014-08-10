using System.Collections.Generic;

namespace NetatmoBot.Services.MeasurementsModels
{
    public class MeasureResponse
    {
        public string status { get; set; }
        public List<MeasureBodyItem> body { get; set; }
    }
}