using System;
using System.Threading.Tasks;
using NetatmoBot.Model;
using NetatmoBot.Services.Mapping;
using NetatmoBot.Services.UserDataModels;
using NetatmoBot.Services.Wrappers;

namespace NetatmoBot.Services
{
    public class UserService
    {
        private readonly AuthenticationToken _authenticationToken;
        private readonly IHttpWrapper _httpWrapper;
        private readonly Uri _uri = new Uri("https://api.netatmo.net/api/getuser");

        public UserService(AuthenticationToken authenticationToken, IHttpWrapper httpWrapper)
        {
            if (authenticationToken == null) throw new ArgumentNullException("authenticationToken");
            if (httpWrapper == null) throw new ArgumentNullException("httpWrapper");
            _authenticationToken = authenticationToken;
            _httpWrapper = httpWrapper;
        }

        public async Task<User> Get()
        {
            string url = string.Format("{0}?access_token={1}",
                _uri,
                _authenticationToken.Token);

            var userResponse = await _httpWrapper.ReadGet<UserResponse>(url);

            return UserResponseMapper.Map(userResponse);
        }
    }
}