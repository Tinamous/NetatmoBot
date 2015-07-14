using System.Collections.Generic;

namespace NetatmoBot.Services.UserDataModels
{
    public class UserBody
    {
        public string _id { get; set; }
        public UserAdministrative administrative { get; set; }
        public List<string> devices { get; set; }
        public string mail { get; set; }
    }
}