using System.Collections.Generic;

namespace NetatmoBot.Services.DevicesModels
{
    public class DeviceListResponseBodyItem
    {
        public IList<DeviceResponseItem> devices { get; set; }
        public IList<ModuleResponseItem> modules { get; set; }
    }
}