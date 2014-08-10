using System.Collections.Generic;
using NetatmoBot.Services.PublicDataModels;
using NetatmoBot.Services.UserDataModels;
using Newtonsoft.Json.Linq;

namespace NetatmoBot.Services.DevicesModels
{
    public class DeviceResponseItem
    {
        public string _id { get; set; }
        public string firmware { get; set; }
        public string ip { get; set; }

        /// <summary>
        /// timestamp
        /// </summary>
        public long last_fw_update { get; set; }

        public long last_radio_store { get; set; }
        public long last_status_store { get; set; }
        public long last_upgrade { get; set; }
        public string module_name { get; set; }
        public IList<string> modules { get; set; }
        public Place place { get; set; }
        public bool public_ext_data { get; set; }
        public string station_name { get; set; }
        public string type { get; set; }
        public IList<string> user_owner { get; set; }

        public string access_code { get; set; }

        //":{"default_alarm":[{"db_alarm_number":22}]},
        public JObject alarm_config { get; set; }

        public bool co2_calibrating { get; set; }

        public TimeStamp date_setup { get; set; }

        //public IList<MetroAlarm> meteo_alarms { get; set; }

        public int wifi_status { get; set; }

        // ":{"AbsolutePressure":993.9,"time_utc":1407683099,"Noise":37,"Temperature":25.5,"Humidity":61,"Pressure":995.4,"CO2":681,"date_max_temp":1407625491,"date_min_temp":1407657571,"min_temp":24.3,"max_temp":25.7},
        public JObject dashboard_data { get; set; }

        // ":["Temperature","Co2","Humidity","Noise","Pressure"]}
        public IList<string> data_type { get; set; }
    }
}