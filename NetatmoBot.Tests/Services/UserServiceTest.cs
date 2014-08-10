using NetatmoBot.Model;
using NetatmoBot.Services;
using NUnit.Framework;

namespace NetatmoBot.Tests.Services
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
        public void GetUser_ReturnsUser()
        {
            // Arrange
            var userService = new UserService(_authenticationToken);

            // Act
            User user = userService.Get();

            // Assert
            Assert.IsNotNull(user);
        } 
    }
}