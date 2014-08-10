using System.Collections.Generic;
using NetatmoBot.Model.Modules;

namespace NetatmoBot.Model
{
    /// <summary>
    /// Details about the station.
    /// </summary>
    public class StationDevices
    {
        private readonly List<Device> _devices = new List<Device>();
        private readonly List<Module> _modules = new List<Module>();

        public List<Device> Devices
        {
            get { return _devices; }
        }

        public List<Module> Modules
        {
            get { return _modules; }
        }
    }
}