using NUnit.Framework;
using Moq;
using CommunityCenterApi.Services.Implementations;
using CommunityCenterApi.Services.Interfaces;
using CommunityCenterApi.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CommunityCenterApi.Log;

namespace CommunityCenterApiTests.Services
{
    [TestFixture]
    public class ActivityServiceLoggingDecoratorTests
    {
        private Mock<IActivityService> _mockDecoratedService;
        private Mock<IFileLoggerFactory> _mockLoggerFactory;
        private Mock<IFileLogger> _mockLogger;
        private ActivityServiceLoggingDecorator _service;

        [SetUp]
        public void Setup()
        {
            _mockDecoratedService = new Mock<IActivityService>();
            _mockLoggerFactory = new Mock<IFileLoggerFactory>();
            _mockLogger = new Mock<IFileLogger>();

            _mockLoggerFactory.Setup(f => f.CreateLogger()).Returns(_mockLogger.Object);

            _service = new ActivityServiceLoggingDecorator(_mockDecoratedService.Object, _mockLoggerFactory.Object);
        }
        [Test]
        public async Task CreateActivityAsync_WhenCalled_LogsAndCreatesActivity()
        {
            var newActivity = new Activity { ActivityId = 1, ActivityName = "Hiking" };
            _mockDecoratedService.Setup(s => s.CreateActivityAsync(newActivity)).ReturnsAsync(newActivity);
            _mockLogger.Setup(l => l.WriteTextToFile(It.IsAny<string>()));

            var result = await _service.CreateActivityAsync(newActivity);

            Assert.AreEqual(newActivity.ActivityId, result.ActivityId);
            _mockLogger.Verify(l => l.WriteTextToFile("Creating a new activity."), Times.Once);
            _mockLogger.Verify(l => l.WriteTextToFile($"Activity created successfully with ID: {result.ActivityId}."), Times.Once);
        }
        [Test]
        public async Task GetAllActivitiesAsync_WhenCalled_LogsAndReturnsAllActivities()
        {
            var activities = new List<Activity> { new Activity { ActivityId = 1, ActivityName = "Running" } };
            _mockDecoratedService.Setup(s => s.GetAllActivitiesAsync()).ReturnsAsync(activities);
            _mockLogger.Setup(l => l.WriteTextToFile(It.IsAny<string>()));

            var result = await _service.GetAllActivitiesAsync();

            Assert.AreEqual(1, result.Count());
            _mockLogger.Verify(l => l.WriteTextToFile("Retrieving all activities."), Times.Once);
            _mockLogger.Verify(l => l.WriteTextToFile("Retrieved all activities successfully. Count: 1."), Times.Once);
        }
        [Test]
        public async Task GetActivityByIdAsync_ActivityExists_LogsAndReturnsActivity()
        {
            var activity = new Activity { ActivityId = 1, ActivityName = "Swimming" };
            _mockDecoratedService.Setup(s => s.GetActivityByIdAsync(1)).ReturnsAsync(activity);
            _mockLogger.Setup(l => l.WriteTextToFile(It.IsAny<string>()));

            var result = await _service.GetActivityByIdAsync(1);

            Assert.IsNotNull(result);
            _mockLogger.Verify(l => l.WriteTextToFile($"Retrieving activity with ID: 1."), Times.Once);
            _mockLogger.Verify(l => l.WriteTextToFile($"Activity retrieved successfully with ID: 1."), Times.Once);
        }

        [Test]
        public async Task GetActivityByIdAsync_ActivityDoesNotExist_LogsAndReturnsNull()
        {
            _mockDecoratedService.Setup(s => s.GetActivityByIdAsync(99)).ReturnsAsync((Activity)null);
            _mockLogger.Setup(l => l.WriteTextToFile(It.IsAny<string>()));

            var result = await _service.GetActivityByIdAsync(99);

            Assert.IsNull(result);
            _mockLogger.Verify(l => l.WriteTextToFile($"Retrieving activity with ID: 99."), Times.Once);
            _mockLogger.Verify(l => l.WriteTextToFile($"Activity with ID: 99 not found."), Times.Once);
        }
        [Test]
        public void CreateActivityAsync_WhenExceptionOccurs_LogsAndThrows()
        {
            var newActivity = new Activity { ActivityId = 1, ActivityName = "Skydiving" };
            _mockDecoratedService.Setup(s => s.CreateActivityAsync(newActivity)).ThrowsAsync(new Exception("Database error"));
            _mockLogger.Setup(l => l.WriteTextToFile(It.IsAny<string>()));

            Assert.ThrowsAsync<Exception>(() => _service.CreateActivityAsync(newActivity));

            _mockLogger.Verify(l => l.WriteTextToFile("Creating a new activity."), Times.Once);
            _mockLogger.Verify(l => l.WriteTextToFile("Error creating activity Database error"), Times.Once);
        }

    }
}
