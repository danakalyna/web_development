using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using CommunityCenterApi.DB;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Implementations;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CommunityCenterApiTests.Services
{
    [TestFixture]
    public class UserPermissionServiceTests
    {
        private ApplicationDbContext _context;
        private UserPermissionService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestUserPermissionDb").Options;

            _context = new ApplicationDbContext(options);
            _service = new UserPermissionService(_context);

            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        [Test]
        public async Task CreateUserPermissionAsync_WhenCalled_AddsUserPermissionToDatabase()
        {
            var newUserPermission = new UserPermission { UserPermissionId = 1, PermissionName = "Admin" };

            var result = await _service.CreateUserPermissionAsync(newUserPermission);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, _context.UserPermissions.Count());
            Assert.AreEqual("Admin", result.PermissionName);
        }
        [Test]
        public async Task GetAllUserPermissionsAsync_ReturnsAllUserPermissions()
        {
            _context.UserPermissions.AddRange(
                new UserPermission { PermissionName = "Admin" },
                new UserPermission { PermissionName = "Editor" }
            );
            _context.SaveChanges();

            var result = await _service.GetAllUserPermissionsAsync();

            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public async Task GetUserPermissionByIdAsync_ExistingId_ReturnsUserPermission()
        {
            var userPermission = new UserPermission { UserPermissionId = 1, PermissionName = "Admin" };
            _context.UserPermissions.Add(userPermission);
            _context.SaveChanges();

            var result = await _service.GetUserPermissionByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Admin", result.PermissionName);
        }
        [Test]
        public async Task UpdateUserPermissionAsync_ExistingUserPermission_UpdatesFields()
        {
            var userPermission = new UserPermission { UserPermissionId = 1, PermissionName = "Admin" };
            _context.UserPermissions.Add(userPermission);
            _context.SaveChanges();

            userPermission.PermissionName = "Updated Admin";
            var result = await _service.UpdateUserPermissionAsync(1, userPermission);

            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Admin", result.PermissionName);
        }
        [Test]
        public async Task DeleteUserPermissionAsync_ExistingUserPermission_DeletesUserPermission()
        {
            var userPermission = new UserPermission { UserPermissionId = 1, PermissionName = "Admin" };
            _context.UserPermissions.Add(userPermission);
            _context.SaveChanges();

            var success = await _service.DeleteUserPermissionAsync(1);

            Assert.IsTrue(success);
            Assert.AreEqual(0, _context.UserPermissions.Count());
        }
        [Test]
        public async Task GetUserPermissionByIdAsync_NonExistingId_ReturnsNull()
        {
            var result = await _service.GetUserPermissionByIdAsync(99); // Assuming ID 99 does not exist

            Assert.IsNull(result);
        }
        [Test]
        public async Task UpdateUserPermissionAsync_NonExistingUserPermission_ReturnsNull()
        {
            var updatedUserPermission = new UserPermission { UserPermissionId = 99, PermissionName = "Non-Existent" };

            var result = await _service.UpdateUserPermissionAsync(99, updatedUserPermission);

            Assert.IsNull(result);
        }
        [Test]
        public async Task DeleteUserPermissionAsync_NonExistingUserPermission_ReturnsFalse()
        {
            var result = await _service.DeleteUserPermissionAsync(99); // Assuming ID 99 does not exist

            Assert.IsFalse(result);
        }
        [Test]
        public async Task GetAllUserPermissionsAsync_WithDependencies_ReturnsPermissionsWithUsers()
        {
            var user = new User { UserId = Guid.NewGuid(), Email = "user@example.com", FirstName = "1", LastName = "1", PasswordHash = "1" };
            _context.Users.Add(user);
            _context.UserPermissions.Add(new UserPermission { UserPermissionId = 1, PermissionName = "Admin", UserId = user.UserId });
            _context.SaveChanges();

            var result = await _service.GetAllUserPermissionsAsync();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("user@example.com", result.First().User.Email); // Assuming User is eagerly loaded
        }

    }
}
