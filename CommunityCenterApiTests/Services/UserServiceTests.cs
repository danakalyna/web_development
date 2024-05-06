using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using CommunityCenterApi.DB;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Implementations;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using CommunityCenterApi.Helpers;

namespace CommunityCenterApiTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private ApplicationDbContext _context;
        private UserService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestUserDb").Options;

            _context = new ApplicationDbContext(options);
            _service = new UserService(_context);

            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        [Test]
        public async Task CreateUserAsync_WhenCalled_AddsUserToDatabaseWithHashedPassword()
        {
            var newUser = new User { UserId = Guid.NewGuid(), Email = "test@example.com", PasswordHash = "password", FirstName = "2", LastName = "1" };

            var result = await _service.CreateUserAsync(newUser);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, _context.Users.Count());
            Assert.AreNotEqual("password", _context.Users.First().PasswordHash); // Ensure password is hashed
        }
        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            _context.Users.AddRange(
                new User { UserId = Guid.NewGuid(), Email = "user1@example.com", PasswordHash = "hash1", FirstName = "2", LastName = "1" },
                new User { UserId = Guid.NewGuid(), Email = "user2@example.com", PasswordHash = "hash2", FirstName = "2", LastName = "1" }
            );
            _context.SaveChanges();

            var result = await _service.GetAllUsersAsync();

            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public async Task GetUserByIdAsync_ExistingId_ReturnsUser()
        {
            var user = new User { UserId = Guid.NewGuid(), Email = "user@example.com", PasswordHash = "hash", FirstName = "2", LastName = "1" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var result = await _service.GetUserByIdAsync(user.UserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
        }
        [Test]
        public async Task UpdateUserAsync_ExistingUser_UpdatesFields()
        {
            var user = new User { UserId = Guid.NewGuid(), Email = "original@example.com", PasswordHash = "original", FirstName = "2", LastName = "1" };
            _context.Users.Add(user);
            _context.SaveChanges();

            user.Email = "updated@example.com";
            var result = await _service.UpdateUserAsync(user.UserId, user);

            Assert.IsNotNull(result);
            Assert.AreEqual("updated@example.com", result.Email);
        }
        [Test]
        public async Task DeleteUserAsync_ExistingUser_DeletesUser()
        {
            var user = new User { UserId = Guid.NewGuid(), Email = "delete@example.com", PasswordHash = "delete", FirstName = "2", LastName = "1" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var success = await _service.DeleteUserAsync(user.UserId);

            Assert.IsTrue(success);
            Assert.AreEqual(0, _context.Users.Count());
        }
        [Test]
        public async Task Authenticate_ValidCredentials_ReturnsUser()
        {
            var user = new User { UserId = Guid.NewGuid(), Email = "valid@example.com", PasswordHash = PasswordHasher.HashPassword("validPassword"), FirstName = "2", LastName = "1" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var result = await _service.Authenticate("valid@example.com", "validPassword");

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Email, result.Email);
        }

        [Test]
        public async Task Authenticate_InvalidCredentials_ReturnsNull()
        {
            var user = new User { UserId = Guid.NewGuid(), Email = "invalid@example.com", PasswordHash = PasswordHasher.HashPassword("password"), FirstName = "2", LastName = "1" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var result = await _service.Authenticate("invalid@example.com", "wrongPassword");

            Assert.IsNull(result);
        }
        [Test]
        public async Task UpdateUserAsync_NonExistentUser_ReturnsNull()
        {
            var user = new User { UserId = Guid.NewGuid(), Email = "doesnotexist@example.com", PasswordHash = "password", FirstName = "2", LastName = "1" };

            var result = await _service.UpdateUserAsync(user.UserId, user);

            Assert.IsNull(result);
        }
        [Test]
        public async Task DeleteUserAsync_NonExistentUser_ReturnsFalse()
        {
            var result = await _service.DeleteUserAsync(Guid.NewGuid());

            Assert.IsFalse(result);
        }
        [Test]
        public async Task Authenticate_NonExistentUser_ReturnsNull()
        {
            var result = await _service.Authenticate("nonexistent@example.com", "password");

            Assert.IsNull(result);
        }
    }
}
