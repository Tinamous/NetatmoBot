using System;
using System.Diagnostics;
using System.Net.Http;
using NetatmoBot.Exceptions;
using NetatmoBot.Extensions;
using NetatmoBot.Model;
using NetatmoBot.Services.AuthenticationModels;
using NetatmoBot.Services.UserDataModels;

namespace NetatmoBot.Services
{
    public class UserService
    {
        private readonly AuthenticationToken _authenticationToken;
        private readonly Uri _uri = new Uri("https://api.netatmo.net/api/getuser");

        public UserService(AuthenticationToken authenticationToken)
        {
            if (authenticationToken == null) throw new ArgumentNullException("authenticationToken");
            _authenticationToken = authenticationToken;
        }

        public User Get()
        {
            string url = string.Format("{0}?access_token={1}",
                _uri,
                _authenticationToken.Token);

            var client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                Trace.WriteLine("User details Failed");
                throw new NetatmoReadException("Failed to read user details. Status code: " + response.StatusCode);
            }

            var publicDataResponse = response.Content.ReadAsAsync<UserResponse>().Result;

            return Map(publicDataResponse);

        }

        private User Map(UserResponse userResponse)
        {
            var body = userResponse.body;
            return new User
            {
                Id = body._id,
                UserAdministrativeInfo = Map(body.administrative),
                CreationDate = DateTime.Now.FromUnixTicks(body.date_creation.sec),
                DeviceIds = body.devices,
                FacebookLikeDisplayed = body.facebook_like_displayed,
                Email = body.mail,
                TimelineNotRead = body.timeline_not_read,
                TempWrite = body.tmp_write,
                UsageMark = body.usage_mark,
            };
        }

        private UserAdministrativeInfo Map(UserAdministrative userAdministrative)
        {
            return new UserAdministrativeInfo
            {
                Country = userAdministrative.country,
                FeelLikeAlgorithm = userAdministrative.feel_like_algo,
                Language = userAdministrative.lang,
                PressureUnit = userAdministrative.pressureunit,
                RegLocale = userAdministrative.reg_locale,
                Unit = userAdministrative.unit,
                WindUnit = userAdministrative.windunit
            };
        }
    }
}