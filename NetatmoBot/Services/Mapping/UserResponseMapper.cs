using NetatmoBot.Model;
using NetatmoBot.Services.UserDataModels;

namespace NetatmoBot.Services.Mapping
{
    public static class UserResponseMapper
    {
        public static User Map(UserResponse userResponse)
        {
            var body = userResponse.body;
            var user = new User
            {
                Id = body._id,
                UserAdministrativeInfo = Map(body.administrative),
                DeviceIds = body.devices,
                Email = body.mail,
            };

            return user;
        }

        private static UserAdministrativeInfo Map(UserAdministrative userAdministrative)
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