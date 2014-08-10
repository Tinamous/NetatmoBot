using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetatmoBot.Services.DevicesModels
{
    public class ModuleResponseItem
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        public string firmware { get; set; }
        public string main_device { get; set; }
        public string module_name { get; set; }
        public bool public_ext_data { get; set; }

        // See NARadioRssiTreshold
        public int rf_status { get; set; }

        /// <summary>
        /// Module type
        /// </summary>
        /// <remarks>
        /// NAMain : for the base station
        /// NAModule1 : for the outdoor module
        /// NAModule4 : for the additionnal indoor module
        /// NAModule3 : for the rain gauge module
        /// NAPlug : for the thermostat relay/plug
        /// NATherm1 : for the thermostat module
        /// </remarks>
        public string type { get; set; }

        // timestamp
        public long  last_message { get; set; }
        // timestamp
        public long  last_seen { get; set; }

        
        

        // See NABatteryLevelModule and NABatteryLevelIndoorModule
        public int battery_vp { get; set; }

        // "last_data_store":{"K":1407683073,"a":17.2,"b":98},
        public JObject last_data_store { get; set; }

        // {"time_utc":1407683073,"Temperature":17.2,"Humidity":98,"date_max_temp":1407676358,"date_min_temp":1407645497,"min_temp":15,"max_temp":22.4},
        public JObject dashboard_data { get; set; }

        public IList<string> data_type { get; set; }
    }
}

class NARadioRssiTreshold
{
    const int RADIO_THRESHOLD_0 = 90;/*low signal*/
    const int RADIO_THRESHOLD_1 = 80;/*signal medium*/
    const int RADIO_THRESHOLD_2 = 70;/*signal high*/
    const int RADIO_THRESHOLD_3 = 60;/*full signal*/
}

/*for raingauge and outdoor module*/
class NABatteryLevelModule
{
    /* Battery range: 6000 ... 3600 */
    const int BATTERY_LEVEL_0 = 5500;/*full*/
    const int BATTERY_LEVEL_1 = 5000;/*high*/
    const int BATTERY_LEVEL_2 = 4500;/*medium*/
    const int BATTERY_LEVEL_3 = 4000;/*low*/
    /* below 4000: very low */
}

// Indoor module.
class NABatteryLevelIndoorModule
{
    /* Battery range: 6000 ... 4200 */
    const int INDOOR_BATTERY_LEVEL_0 = 5640;/*full*/
    const int INDOOR_BATTERY_LEVEL_1 = 5280;/*high*/
    const int INDOOR_BATTERY_LEVEL_2 = 4920;/*medium*/
    const int INDOOR_BATTERY_LEVEL_3 = 4560;/*low*/
    /* Below 4560: very low */
}