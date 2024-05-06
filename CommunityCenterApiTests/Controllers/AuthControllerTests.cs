using CommunityCenterApi.Controllers;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityCenterApiTests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<IConfiguration> _mockConfiguration;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("this_is_a_very_secret_key");

            _controller = new AuthController(_mockConfiguration.Object, _mockUserService.Object);
        }

        [Test]
        public async Task Login_WithCorrectCredentials_ReturnsToken()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Email = "user@example.com" };
            _mockUserService.Setup(x => x.Authenticate("validUser", "validPass")).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(new UserLoginRequest { Username = "validUser", Password = "validPass" });

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var okValue = okResult.Value; // Already an object, no need to cast again

            Assert.IsNotNull(okValue);
            // Reflection to access properties
            var tokenProp = okValue.GetType().GetProperty("Token");
            var usernameProp = okValue.GetType().GetProperty("Username");

            Assert.IsNotNull(tokenProp, "Token property not found.");
            Assert.IsNotNull(usernameProp, "Username property not found.");

            var tokenValue = tokenProp.GetValue(okValue) as string;
            var usernameValue = usernameProp.GetValue(okValue) as string;

            Assert.IsNotNull(tokenValue, "Token value is null.");
            Assert.AreEqual(user.Email, usernameValue, "Username does not match.");
        }



        [Test]
        public async Task Login_WithIncorrectCredentials_ReturnsBadRequest()
        {
            // Arrange
            _mockUserService.Setup(x => x.Authenticate("invalidUser", "invalidPass")).ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(new UserLoginRequest { Username = "invalidUser", Password = "invalidPass" });

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult, "Expected a BadRequestObjectResult.");

            // Accessing the response using a dictionary
            var response = badRequestResult.Value;

            var tokenProp = response.GetType().GetProperty("message");
            var messageValue = tokenProp.GetValue(response) as string;

            bool hasMessage = !string.IsNullOrEmpty(messageValue);

            Assert.IsTrue(hasMessage, "Expected 'message' key to be present in the response.");
            Assert.AreEqual("Username or password is incorrect", messageValue as string, "The error message did not match the expected value.");
        }

    }
}
