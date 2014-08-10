using System;
using System.Collections.Generic;

namespace NetatmoBot.Model
{
    public class User
    {
        public string Id { get; set; }
        public UserAdministrativeInfo UserAdministrativeInfo { get; set; }
        public DateTime CreationDate { get; set; }
        public List<string> DeviceIds { get; set; }
        public bool FacebookLikeDisplayed { get; set; }
        public string Email { get; set; }
        public int TimelineNotRead { get; set; }
        public bool TempWrite { get; set; }
        public decimal UsageMark { get; set; }
    }
}