using System.Collections.Generic;

namespace NetatmoBot.Services.PublicDataModels
{
    public class PublicDataResponse
    {
        public string status { get; set; }

        public List<PublicDataResponseItem> body { get; set; }
    }
}