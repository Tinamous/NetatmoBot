using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using NetatmoBot.Exceptions;
using NetatmoBot.Extensions;
using NetatmoBot.Model;
using NetatmoBot.Model.Modules;
using NetatmoBot.Services.DevicesModels;
using NetatmoBot.Services.Mapping;
using NetatmoBot.Services.PublicDataModels;
using NetatmoBot.Services.Wrappers;

namespace NetatmoBot.Services
{
    public class DevicesService
    {
        private readonly Uri _uri = new Uri("https://api.netatmo.net/api/devicelist");
        private readonly AuthenticationToken _authenticationToken;
        private readonly IHttpWrapper _httpWrapper;

        public DevicesService(AuthenticationToken authenticationToken, IHttpWrapper httpWrapper)
        {
            if (authenticationToken == null) throw new ArgumentNullException("authenticationToken");
            if (httpWrapper == null) throw new ArgumentNullException("httpWrapper");
            _authenticationToken = authenticationToken;
            _httpWrapper = httpWrapper;
        }

        public async Task<StationDevices> Get()
        {
            string url = string.Format("{0}?access_token={1}&app_type=app_station", _uri, _authenticationToken.Token);

            var deviceListResponce = await _httpWrapper.ReadGet<DeviceListResponse>(url);
            //var client = new HttpClient();
            //HttpResponseMessage response = client.GetAsync(url).Result;

            //if (!response.IsSuccessStatusCode)
            //{
            //    Trace.WriteLine("DevicesList Failed!");
            //    throw new NetatmoReadException("Failed to read devices. Status code: " + response.StatusCode);
            //}

            //var deviceListResponce = response.Content.ReadAsAsync<DeviceListResponse>().Result;

            return DeviceMapper.Map(deviceListResponce.body);
        }
    }
}