using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CommunityCenterApiTests.Controllers
{
    [TestFixture]
    public class UserPermissionsControllerTests
    {
        private Mock<IUserPermissionService> _mockUserPermissionService;
        private UserPermissionsController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserPermissionService = new Mock<IUserPermissionService>();
            _controller = new UserPermissionsController(_mockUserPermissionService.Object);
        }

        [Test]
        public async Task PostUserPermission_ValidData_ReturnsCreatedAtActionResult()
        {
            var userPermission = new UserPermission { UserPermissionId = 1 };
            _mockUserPermissionService.Setup(x => x.CreateUserPermissionAsync(It.IsAny<UserPermission>())).ReturnsAsync(userPermission);

            var result = await _controller.PostUserPermission(userPermission);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.AreEqual("GetUserPermission", createdResult.ActionName);
            Assert.AreEqual(userPermission, createdResult.Value);
        }
        [Test]
        public async Task GetUserPermissions_ReturnsAllUserPermissions()
        {
            var userPermissions = new List<UserPermission> { new UserPermission { UserPermissionId = 1 }, new UserPermission { UserPermissionId = 2 } };
            _mockUserPermissionService.Setup(x => x.GetAllUserPermissionsAsync()).ReturnsAsync(userPermissions);

            var result = await _controller.GetUserPermissions();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(userPermissions, okResult.Value);
        }
        [Test]
        public async Task GetUserPermission_ExistingId_ReturnsUserPermission()
        {
            var userPermission = new UserPermission { UserPermissionId = 1 };
            _mockUserPermissionService.Setup(x => x.GetUserPermissionByIdAsync(1)).ReturnsAsync(userPermission);

            var result = await _controller.GetUserPermission(1);

            Assert.AreEqual(userPermission, result.Value);
        }

        [Test]
        public async Task GetUserPermission_NonExistingId_ReturnsNotFound()
        {
            _mockUserPermissionService.Setup(x => x.GetUserPermissionByIdAsync(1)).ReturnsAsync((UserPermission)null);

            var result = await _controller.GetUserPermission(1);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }


        [Test]
        public async Task PutUserPermission_IdMismatch_ReturnsBadRequest()
        {
            var userPermission = new UserPermission { UserPermissionId = 2 };

            var result = await _controller.PutUserPermission(1, userPermission);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task DeleteUserPermission_ExistingId_ReturnsNoContent()
        {
            _mockUserPermissionService.Setup(x => x.DeleteUserPermissionAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteUserPermission(1);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteUserPermission_NonExistingId_ReturnsNotFound()
        {
            _mockUserPermissionService.Setup(x => x.DeleteUserPermissionAsync(1)).ReturnsAsync(false);

            var result = await _controller.DeleteUserPermission(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

    }
}
