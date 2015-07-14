using System.Threading.Tasks;
using NetatmoBot.Model;
using NetatmoBot.Services;
using NetatmoBot.Services.Wrappers;
using NUnit.Framework;

namespace NetatmoBot.IntegrationTests.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private AuthenticationToken _authenticationToken;

        [TestFixtureSetUp]
        public void SetUp()
        {
            // Arrange
            var authenticationService = new AuthenticationService();
            _authenticationToken = authenticationService.AuthenticateUser();   
        }

        [Test]
        public async Task GetUser_ReturnsUser()
        {
            // Arrange
            var userService = new UserService(_authenticationToken, new HttpWrapper());

            // Act
            User user = await userService.Get();

            // Assert
            Assert.IsNotNull(user);
        } 
    }
}