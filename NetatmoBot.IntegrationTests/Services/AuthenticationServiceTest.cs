using NetatmoBot.Model;
using NetatmoBot.Services;
using NUnit.Framework;

namespace NetatmoBot.IntegrationTests.Services
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        [Test]
        public void Authenticate_WithCorrectUserNameAndPassword_AuthenticatesUser()
        {
            // Arrange
            var authenticationService = new AuthenticationService();

            // Act
            // Use build in config lookup.
            AuthenticationToken token = authenticationService.AuthenticateUser();

            // Assert
            Assert.IsNotNull(token);
        }
    }
}