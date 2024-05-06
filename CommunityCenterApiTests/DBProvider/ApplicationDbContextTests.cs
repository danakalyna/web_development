using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using CommunityCenterApi.DB;
using CommunityCenterApi.Models;
using System;

namespace CommunityCenterApiTests.DBProvider
{
    [TestFixture]
    public class ApplicationDbContextTests
    {
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            // Ensure the database is created
            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void EnsureCreationOfDatabase()
        {
            Assert.IsTrue(_context.Database.IsInMemory());
        }

        [Test]
        public void UserPermissions_ShouldConfigureCorrectly()
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "Test",        // Required field
                LastName = "User",         // Required field
                PasswordHash = "hash123"   // Required field
            };
            var permission = new UserPermission { UserPermissionId = 1, UserId = user.UserId, PermissionName = "Name" };

            // Ensure DbContext is tracking the user and user permission
            _context.Users.Add(user);
            _context.UserPermissions.Add(permission);
            _context.SaveChanges();

            var userFromDb = _context.Users.Include(u => u.UserPermissions).FirstOrDefault();

            Assert.IsNotNull(userFromDb, "User should not be null.");
            Assert.That(userFromDb.UserPermissions, Has.One.Items, "User should have exactly one permission.");
        }


        [Test]
        public void ActivityRelations_ShouldConfigureCorrectly()
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = "activityowner@example.com",
                FirstName = "ActivityOwner",
                LastName = "OwnerLast",
                PasswordHash = "password123"
            };

            var activity = new Activity
            {
                ActivityId = 1,
                UserId = user.UserId,
                ActivityName = "Yoga Class",
                User = user,
                Description = "Test"
            };

            _context.Users.Add(user);
            _context.Activities.Add(activity);
            _context.SaveChanges();

            var activityFromDb = _context.Activities.Include(a => a.User).FirstOrDefault();
            Assert.IsNotNull(activityFromDb, "Activity should not be null.");
            Assert.AreEqual(user.UserId, activityFromDb.User.UserId, "User associated with the activity must match.");
        }

        [Test]
        public void BookingRelations_ShouldConfigureCorrectly()
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = "booker@example.com",
                FirstName = "Booker",
                LastName = "BookerLast",
                PasswordHash = "booker123"
            };

            var activity = new Activity
            {
                ActivityId = 2,
                ActivityName = "Pilates",
                Description = "Test"
            };

            var booking = new Booking
            {
                BookingId = 1,
                UserId = user.UserId,
                ActivityId = activity.ActivityId,
                User = user,
                Activity = activity,
                Status = "1"
            };

            _context.Users.Add(user);
            _context.Activities.Add(activity);
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            var bookingFromDb = _context.Bookings.Include(b => b.User).Include(b => b.Activity).FirstOrDefault();
            Assert.IsNotNull(bookingFromDb, "Booking should not be null.");
            Assert.AreEqual(user.UserId, bookingFromDb.User.UserId, "User associated with the booking must match.");
            Assert.AreEqual(activity.ActivityId, bookingFromDb.Activity.ActivityId, "Activity associated with the booking must match.");
        }

        [Test]
        public void Users_ShouldHaveUniqueIds()
        {
            var id = Guid.NewGuid();
            var user1 = new User
            {
                UserId = id,
                Email = "unique@example.com",
                FirstName = "Unique",
                LastName = "User",
                PasswordHash = "hash456"
            };

            var user2 = new User
            {
                UserId = id,
                Email = "unique@example.com", // Duplicate email
                FirstName = "Another",
                LastName = "User",
                PasswordHash = "hash789"
            };

            _context.Users.Add(user1);
            _context.SaveChanges();


            Assert.Throws<InvalidOperationException>(() => _context.Users.Add(user2), "Should not allow duplicate emails.");
        }

        [Test]
        public void Activities_ShouldSupportMultipleBookings()
        {
            var activity = new Activity
            {
                ActivityId = 4,
                ActivityName = "Group Sport",
                Description = "test"
            };

            var booking1 = new Booking
            {
                Status = "1",
                BookingId = 3,
                ActivityId = activity.ActivityId,
                UserId = Guid.NewGuid() // Simulate different users
            };

            var booking2 = new Booking
            {
                Status = "1",
                BookingId = 4,
                ActivityId = activity.ActivityId,
                UserId = Guid.NewGuid() // Simulate different users
            };

            _context.Activities.Add(activity);
            _context.Bookings.AddRange(booking1, booking2);
            _context.SaveChanges();

            var activityFromDb = _context.Activities
                                         .Include(a => a.Bookings)
                                         .FirstOrDefault(a => a.ActivityId == activity.ActivityId);

            Assert.IsNotNull(activityFromDb, "Activity should not be null.");
            Assert.AreEqual(2, activityFromDb.Bookings.Count, "Activity should have exactly two bookings.");
        }


        [Test]
        public void ValidateUserFields()
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = "test@test.com",
                FirstName = "Test",        // Required field
                LastName = "User",         // Required field
                PasswordHash = "hash123"   // Required field
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            var retrievedUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            Assert.IsNotNull(retrievedUser, "User should be retrieved successfully.");
            Assert.AreEqual("Test", retrievedUser.FirstName);
            Assert.AreEqual("User", retrievedUser.LastName);
            Assert.AreEqual("test@test.com", retrievedUser.Email);
        }

        // Further tests could continue validating other model behaviors, relationships, and constraints.

    }
}

