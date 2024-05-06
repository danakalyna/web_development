using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Implementations;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using CommunityCenterApi.DB;

namespace CommunityCenterApiTests.Services
{
    [TestFixture]
    public class ActivityServiceTests
    {
        private ApplicationDbContext _context;
        private ActivityService _activityService;

        [SetUp]
        public void Setup()
        {
            // Use InMemory database for DbContext
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestActivitiesDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _activityService = new ActivityService(_context);
        }

        [TearDown]
        public void Teardown() 
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllActivitiesAsync_ReturnsAllActivities()
        {
            // Seed the database
            var activities = new List<Activity>
            {
                new Activity { ActivityId = 1, ActivityName = "Yoga",
                Description = "desc" },
                new Activity { ActivityId = 2, ActivityName = "Pilates",
                Description = "desc" }
            };

            _context.Activities.AddRange(activities);
            _context.SaveChanges();

            // Test service method
            var result = await _activityService.GetAllActivitiesAsync();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(a => a.ActivityName == "Yoga"));
        }

        [Test]
        public async Task CreateActivityAsync_ValidData_ReturnsCreatedActivity()
        {
            var newActivity = new Activity
            {
                ActivityName = "Yoga Session",
                Date = System.DateTime.Now,
                Description = "desc"
            };

            var result = await _activityService.CreateActivityAsync(newActivity);

            Assert.IsNotNull(result);
            Assert.AreEqual("Yoga Session", result.ActivityName);
            Assert.IsTrue(_context.Activities.Any(a => a.ActivityName == "Yoga Session"));
        }

        [Test]
        public async Task GetActivityByIdAsync_ExistingId_ReturnsActivity()
        {
            var activity = new Activity { ActivityId = 3, ActivityName = "Zumba", Description = "Fun dance workout." };
            _context.Activities.Add(activity);
            _context.SaveChanges();

            var result = await _activityService.GetActivityByIdAsync(3);

            Assert.IsNotNull(result);
            Assert.AreEqual("Zumba", result.ActivityName);
            Assert.AreEqual("Fun dance workout.", result.Description);
        }

        [Test]
        public async Task GetActivityByIdAsync_NonExistingId_ReturnsNull()
        {
            var result = await _activityService.GetActivityByIdAsync(99);
            Assert.IsNull(result);
        }
        [Test]
        public async Task UpdateActivityAsync_ActivityExists_UpdatesAndReturnsActivity()
        {
            var existingActivity = new Activity { ActivityId = 4, ActivityName = "Running", Description = "Morning running." };
            _context.Activities.Add(existingActivity);
            _context.SaveChanges();

            var updatedActivity = new Activity { ActivityId = 4, ActivityName = "Evening Running", Description = "Evening running session." };

            var result = await _activityService.UpdateActivityAsync(4, updatedActivity);

            Assert.IsNotNull(result);
            Assert.AreEqual("Evening Running", result.ActivityName);
            Assert.AreEqual("Evening running session.", result.Description);
        }

        [Test]
        public async Task UpdateActivityAsync_ActivityDoesNotExist_ReturnsNull()
        {
            var updatedActivity = new Activity { ActivityId = 99, ActivityName = "Swimming" };
            var result = await _activityService.UpdateActivityAsync(99, updatedActivity);
            Assert.IsNull(result);
        }
        [Test]
        public async Task DeleteActivityAsync_ActivityExists_DeletesAndReturnsTrue()
        {
            var activity = new Activity { ActivityId = 5, ActivityName = "Cycling", Description = "Long distance cycling." };
            _context.Activities.Add(activity);
            _context.SaveChanges();

            var result = await _activityService.DeleteActivityAsync(5);

            Assert.IsTrue(result);
            Assert.IsFalse(_context.Activities.Any(a => a.ActivityId == 5));
        }

        [Test]
        public async Task DeleteActivityAsync_ActivityDoesNotExist_ReturnsFalse()
        {
            var result = await _activityService.DeleteActivityAsync(99);
            Assert.IsFalse(result);
        }

    }
}
