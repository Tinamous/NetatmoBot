using System.Collections.Generic;

namespace NetatmoBot.Model
{
    public class User
    {
        public string Id { get; set; }
        public UserAdministrativeInfo UserAdministrativeInfo { get; set; }
        public List<string> DeviceIds { get; set; }
        public string Email { get; set; }
    }
}