using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CommunityCenterApi.Controllers;
using CommunityCenterApi.Services.Interfaces;
using CommunityCenterApi.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CommunityCenterApiTests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService.Object);
        }

        [Test]
        public async Task GetUsers_ReturnsAllUsers()
        {
            var users = new List<User>
            {
                new User { UserId = Guid.NewGuid(), Email = "user@example.com" },
                new User { UserId = Guid.NewGuid(), Email = "user2@example.com" }
            };
            _mockUserService.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

            var result = await _controller.GetUsers();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(users, okResult.Value);
        }
        [Test]
        public async Task GetUser_ExistingId_ReturnsUser()
        {
            var user = new User { UserId = Guid.NewGuid(), Email = "user@example.com" };
            _mockUserService.Setup(x => x.GetUserByIdAsync(user.UserId)).ReturnsAsync(user);

            var result = await _controller.GetUser(user.UserId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(user, okResult.Value);
        }

        [Test]
        public async Task GetUser_NonExistingId_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _controller.GetUser(userId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task CreateUser_ValidUser_ReturnsCreatedUser()
        {
            var newUser = new User { Email = "newuser@example.com" };
            _mockUserService.Setup(x => x.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(newUser);

            var result = await _controller.CreateUser(newUser);

            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.AreEqual(newUser, createdResult.Value);
        }

        [Test]
        public async Task CreateUser_InvalidModel_ReturnsBadRequest()
        {
            _controller.ModelState.AddModelError("Email", "Required");

            var newUser = new User { Email = "" }; // Invalid data
            var result = await _controller.CreateUser(newUser);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task UpdateUser_ValidUpdate_ReturnsUpdatedUser()
        {
            var updatedUser = new User { UserId = Guid.NewGuid(), Email = "updated@example.com" };
            _mockUserService.Setup(x => x.UpdateUserAsync(updatedUser.UserId, It.IsAny<User>())).ReturnsAsync(updatedUser);

            var result = await _controller.UpdateUser(updatedUser.UserId, updatedUser);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(updatedUser, okResult.Value);
        }

        [Test]
        public async Task UpdateUser_NonExistingUser_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.UpdateUserAsync(userId, It.IsAny<User>())).ReturnsAsync((User)null);

            var result = await _controller.UpdateUser(userId, new User());

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task DeleteUser_ExistingUser_ReturnsNoContent()
        {
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.DeleteUserAsync(userId)).ReturnsAsync(true);

            var result = await _controller.DeleteUser(userId);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteUser_NonExistingUser_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            _mockUserService.Setup(x => x.DeleteUserAsync(userId)).ReturnsAsync(false);

            var result = await _controller.DeleteUser(userId);



        }
    }
}
