using System;
using System.Collections.Generic;
using NetatmoBot.Extensions;
using NetatmoBot.Model;
using NetatmoBot.Model.Modules;
using NetatmoBot.Services.DevicesModels;
using NetatmoBot.Services.PublicDataModels;

namespace NetatmoBot.Services.Mapping
{
    public static class DeviceMapper
    {
        public static StationDevices Map(DeviceListResponseBodyItem body)
        {
            //throw new NotImplementedException();
            var stationDevices = new StationDevices();
            stationDevices.Devices.AddRange(Map(body.devices));
            stationDevices.Modules.AddRange(Map(body.modules));
            return stationDevices;
        }

        private static IEnumerable<Module> Map(IEnumerable<ModuleResponseItem> modules)
        {
            var moduleList = new List<Module>();
            foreach (var moduleResponseItem in modules)
            {
                Module module = ModuleFactory.Create(moduleResponseItem.type);
                module.BatteryLevel = moduleResponseItem.battery_vp;
                module.Firmware = moduleResponseItem.firmware;
                module.Id = moduleResponseItem.Id;
                module.LastMessage = DateTime.Now.FromUnixTicks(moduleResponseItem.last_message);
                module.LastSeen = DateTime.Now.FromUnixTicks(moduleResponseItem.last_seen);
                module.Public = moduleResponseItem.public_ext_data;
                //module.Type = moduleResponseItem.type,
                module.MainDeviceId = moduleResponseItem.main_device;
                module.Name = moduleResponseItem.module_name;
                module.RfStatus = moduleResponseItem.rf_status;

                moduleList.Add(module);
            }
            return moduleList;
        }

        private static IEnumerable<Device> Map(IEnumerable<DeviceResponseItem> devices)
        {
            var deviceList = new List<Device>();
            foreach (var deviceResponseItem in devices)
            {
                deviceList.Add(new Device
                {
                    Id = deviceResponseItem._id,
                    AccessCode = deviceResponseItem.access_code,
                    CO2Calibrating = deviceResponseItem.co2_calibrating,
                    Firmware = deviceResponseItem.firmware,
                    Ip = deviceResponseItem.ip,
                    LastRadioStoredOn = DateTime.Now.FromUnixTicks(deviceResponseItem.last_radio_store),
                    LastStatusStoredOn = DateTime.Now.FromUnixTicks(deviceResponseItem.last_status_store),
                    LastUpgradeOn = DateTime.Now.FromUnixTicks(deviceResponseItem.last_upgrade),
                    LastFirmwareUpdate = DateTime.Now.FromUnixTicks(deviceResponseItem.last_fw_update),
                    ModuleIds = deviceResponseItem.modules,
                    ModuleName = deviceResponseItem.module_name,
                    OwnerId = deviceResponseItem.user_owner,
                    Public = deviceResponseItem.public_ext_data,
                    SetupOn = DateTime.Now.FromUnixTicks(deviceResponseItem.date_setup.sec),
                    StationName = deviceResponseItem.station_name,
                    Type = deviceResponseItem.type,
                    WiFiStatus = deviceResponseItem.wifi_status,
                    Place = StationPlaceMapper.Map(deviceResponseItem.place)
                });
            }
            return deviceList;
        }
    }
}