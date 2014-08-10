using System.Collections.Generic;

namespace NetatmoBot.Services.UserDataModels
{
    public class UserBody
    {
        public string _id { get; set; }
        public UserAdministrative administrative { get; set; }
        public TimeStamp date_creation { get; set; }
        public List<string> devices { get; set; }
        public bool facebook_like_displayed { get; set; }
        public string mail { get; set; }
        public int timeline_not_read { get; set; }
        public bool tmp_write { get; set; }
        public decimal usage_mark { get; set; }
    }
}