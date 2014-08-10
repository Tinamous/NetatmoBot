namespace NetatmoBot.Services.DevicesModels
{
    public class MetroAlarm
    {
        public string _id { get; set; }
        public string area { get; set; }
        public long begin { get; set; }
        public long end { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string text_field { get; set; }
        public int level { get; set; }
        public string descr { get; set; }
        public string status { get; set; }
        public long alarm_id { get; set; }
        public int max_level { get; set; }
        public long time_generated { get; set; }
        public string origin { get; set; }
    }
}