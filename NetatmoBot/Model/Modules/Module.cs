using System;
using System.Diagnostics;

namespace NetatmoBot.Model.Modules
{
    [DebuggerDisplay("Name: {Name}")]
    public class Module
    {
        public string Id { get; set; }
        public string Firmware { get; set; }
        public string MainDeviceId { get; set; }
        public string Name { get; set; }
        public bool Public { get; set; }

        // See NARadioRssiTreshold
        public int RfStatus { get; set; }

        ///// <summary>
        ///// Module type
        ///// </summary>
        ///// <remarks>
        ///// NAMain : for the base station
        ///// NAModule1 : for the outdoor module
        ///// NAModule4 : for the additionnal indoor module
        ///// NAModule3 : for the rain gauge module
        ///// NAPlug : for the thermostat relay/plug
        ///// NATherm1 : for the thermostat module
        ///// </remarks>
        //public string Type { get; set; }

        // timestamp
        public DateTime LastMessage { get; set; }

        // timestamp
        public DateTime LastSeen { get; set; }




        // See NABatteryLevelModule and NABatteryLevelIndoorModule
        public int BatteryLevel { get; set; }

        // "last_data_store":{"K":1407683073,"a":17.2,"b":98},
        //public JObject last_data_store { get; set; }

        // {"time_utc":1407683073,"Temperature":17.2,"Humidity":98,"date_max_temp":1407676358,"date_min_temp":1407645497,"min_temp":15,"max_temp":22.4},
        //public JObject dashboard_data { get; set; }

        //public IList<string> data_type { get; set; }
    }
}