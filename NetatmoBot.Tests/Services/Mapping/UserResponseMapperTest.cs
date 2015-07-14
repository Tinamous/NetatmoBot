using NetatmoBot.Model;
using NetatmoBot.Services.Mapping;
using NetatmoBot.Services.UserDataModels;
using NetatmoBot.Tests.SampleData;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NetatmoBot.Tests.Services.Mapping
{
    [TestFixture]
    public class UserResponseMapperTest
    {
        [Test]
        public void Map_FromSampleData_ToUser()
        {
            // Arrange
            string sampleData = SampleDataHelper.LoadSampleData("Example.GetUser.json");
            var userResponse = JsonConvert.DeserializeObject<UserResponse>(sampleData);

            // Act
            User user = UserResponseMapper.Map(userResponse);

            // Assert
            Assert.IsNotNull(user);

            Assert.IsNotNull(user.DeviceIds, "DeviceIds");
            Assert.AreEqual("70:ee:50:03:78:3a", user.DeviceIds[0], "DeviceIds[0]");
            Assert.AreEqual("stephen.harrison@analysisuk.com", user.Email, "Email");
            Assert.AreEqual("537765721d7759e9f70c6609", user.Id, "Id");
            Assert.IsNotNull(user.UserAdministrativeInfo, "UserAdministrativeInfo");

            // UserAdministrativeInfo
            Assert.AreEqual("GB", user.UserAdministrativeInfo.Country, "UserAdministrativeInfo.Country");
            Assert.AreEqual(0, user.UserAdministrativeInfo.FeelLikeAlgorithm, "UserAdministrativeInfo.FeelLikeAlgorithm");
            Assert.AreEqual("en-US", user.UserAdministrativeInfo.Language, "UserAdministrativeInfo.Language");
            Assert.AreEqual(0, user.UserAdministrativeInfo.PressureUnit, "UserAdministrativeInfo.PressureUnit");
            Assert.AreEqual("en", user.UserAdministrativeInfo.RegLocale, "UserAdministrativeInfo.RegLocale");
            Assert.AreEqual(0, user.UserAdministrativeInfo.Unit, "UserAdministrativeInfo.Unit");
            Assert.AreEqual(1, user.UserAdministrativeInfo.WindUnit, "UserAdministrativeInfo.WindUnit");
        }
    }
}