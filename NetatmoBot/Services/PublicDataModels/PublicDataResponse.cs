using System.Collections.Generic;

namespace NetatmoBot.Services.PublicDataModels
{
    public class PublicDataResponse
    {
        public string status { get; set; }
        public List<PublicDataResponseItem> body { get; set; }
        public string time_exec { get; set; }
        public string time_server { get; set; }
    }
}