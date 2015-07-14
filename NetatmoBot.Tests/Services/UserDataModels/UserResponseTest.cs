using System.Collections.Generic;
using NetatmoBot.Services.UserDataModels;
using NetatmoBot.Tests.SampleData;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NetatmoBot.Tests.Services.UserDataModels
{
    /// <summary>
    /// Ensure that data returned from Netatmo api can be correctly loaded into UserResponse object.
    /// </summary>
    [TestFixture]
    public class UserResponseTest
    {
        [Test]
        [TestCase("Example.GetUser.json")]
        public void DeserializeSampeData_SetsExpectedProperties(string fileName)
        {
            // Arrange
            string sampleData = SampleDataHelper.LoadSampleData(fileName);

            // Act
            var userResponse = JsonConvert.DeserializeObject<UserResponse>(sampleData);

            // Assert
            Assert.AreEqual("ok", userResponse.status, "status");
            AssertBody(userResponse.body);
        }

        private void AssertBody(UserBody body)
        {
            Assert.IsNotNull(body);
            Assert.AreEqual("537765721d7759e9f70c6609", body._id, "_id");
            Assert.AreEqual("stephen.harrison@analysisuk.com", body.mail, "mail");
            AssertDevices(body.devices);
            AssertAdministrative(body.administrative);
        }

        private void AssertDevices(List<string> devices)
        {
            Assert.IsNotNull(devices);
            Assert.AreEqual(1, devices.Count, "Devices.Count");
            var device = devices[0];
            // Main unit?
            Assert.AreEqual("70:ee:50:03:78:3a", device, "devices[0]");
        }

        private void AssertAdministrative(UserAdministrative administrative)
        {
            Assert.IsNotNull(administrative, "administrative");

            Assert.AreEqual("GB", administrative.country, "country");
            Assert.AreEqual(0, administrative.feel_like_algo, "feel_like_algo");
            Assert.AreEqual("en-US", administrative.lang, "lang");
            Assert.AreEqual(0, administrative.pressureunit, "pressureunit");
            Assert.AreEqual("en", administrative.reg_locale, "reg_locale");
            Assert.AreEqual(0, administrative.unit, "unit");
            Assert.AreEqual(1, administrative.windunit, "windunit");
        }
    }
}