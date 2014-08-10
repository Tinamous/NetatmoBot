using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetatmoBot.Model
{
    [DebuggerDisplay("StationName: {StationName}, ModuleName: {ModuleName}")]
    public class Device
    {
        public string Id { get; set; }

        public string Firmware { get; set; }

        public string Ip { get; set; }

        public DateTime LastFirmwareUpdate { get; set; }
        public DateTime LastRadioStoredOn { get; set; }
        public DateTime LastStatusStoredOn { get; set; }
        public DateTime LastUpgradeOn { get; set; }

        public string StationName { get; set; }
        public string ModuleName { get; set; }

        public IList<string> ModuleIds { get; set; }

        public StationPlace Place { get; set; }
        
        public bool Public { get; set; }
        
        public string Type { get; set; }
        public IList<string> OwnerId { get; set; }

        public string AccessCode { get; set; }

        //":{"default_alarm":[{"db_alarm_number":22}]},
        //public JObject alarm_config { get; set; }

        public bool CO2Calibrating { get; set; }

        public DateTime SetupOn { get; set; }

        //public IList<MetroAlarm> meteo_alarms { get; set; }

        public int WiFiStatus { get; set; }

        // ":{"AbsolutePressure":993.9,"time_utc":1407683099,"Noise":37,"Temperature":25.5,"Humidity":61,"Pressure":995.4,"CO2":681,"date_max_temp":1407625491,"date_min_temp":1407657571,"min_temp":24.3,"max_temp":25.7},
        //public JObject dashboard_data { get; set; }

        // ":["Temperature","Co2","Humidity","Noise","Pressure"]}
        //public IList<string> data_type { get; set; }
    }
}