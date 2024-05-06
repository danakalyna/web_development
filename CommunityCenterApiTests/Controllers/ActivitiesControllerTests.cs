using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommunityCenterApi.Controllers;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommunityCenterApiTests.Controllers
{
    [TestFixture]
    public class ActivitiesControllerTests
    {
        private Mock<IActivityService> _activityService;
        private Mock<IBookingService> _bookingService;
        private ActivitiesController _controller;

        [SetUp]
        public void Setup()
        {
            _activityService = new Mock<IActivityService>();
            _bookingService = new Mock<IBookingService>();
            _controller = new ActivitiesController(_activityService.Object, _bookingService.Object);
        }

        [Test]
        public async Task CreateActivity_WithValidActivity_ReturnsCreatedAtActionResult()
        {
            var activity = new Activity { ActivityId = 1, ActivityName = "Yoga Class" };
            _activityService.Setup(x => x.CreateActivityAsync(It.IsAny<Activity>())).ReturnsAsync(activity);

            var result = await _controller.CreateActivity(activity);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetActivityById", createdResult.ActionName);
        }

        [Test]
        public async Task CreateActivity_WithNullActivity_ReturnsBadRequest()
        {
            Activity nullActivity = null;

            var result = await _controller.CreateActivity(nullActivity);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        [Test]
        public async Task GetAllActivities_ReturnsAllActivities()
        {
            var activities = new List<Activity>
            {
                new Activity { ActivityId = 1, ActivityName = "Yoga Class" },
                new Activity { ActivityId = 2, ActivityName = "Pilates" }
            };
            _activityService.Setup(x => x.GetAllActivitiesAsync()).ReturnsAsync(activities);

            var result = await _controller.GetAllActivities();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(2, (okResult.Value as IEnumerable<Activity>).Count());
        }

        [Test]
        public async Task GetActivityById_ExistingId_ReturnsActivity()
        {
            var activity = new Activity { ActivityId = 1, ActivityName = "Yoga Class" };
            _activityService.Setup(x => x.GetActivityByIdAsync(1)).ReturnsAsync(activity);

            var result = await _controller.GetActivityById(1);

            Assert.IsInstanceOf<Activity>(result.Value);
            var okResult = result.Value as Activity;
            Assert.AreEqual(activity, okResult);
        }

        [Test]
        public async Task GetActivityById_NonExistingId_ReturnsNotFound()
        {
            _activityService.Setup(x => x.GetActivityByIdAsync(1)).ReturnsAsync((Activity)null);

            var result = await _controller.GetActivityById(1);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task UpdateActivity_ExistingActivity_ReturnsNoContent()
        {
            var activity = new Activity { ActivityId = 1, ActivityName = "Yoga Class" };
            _activityService.Setup(x => x.UpdateActivityAsync(1, It.IsAny<Activity>())).ReturnsAsync(activity);

            var result = await _controller.UpdateActivity(1, activity);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task UpdateActivity_NonExistingActivity_ReturnsNotFound()
        {
            _activityService.Setup(x => x.UpdateActivityAsync(1, It.IsAny<Activity>())).ReturnsAsync((Activity)null);

            var result = await _controller.UpdateActivity(1, new Activity());

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public async Task DeleteActivity_ExistingId_ReturnsNoContent()
        {
            _activityService.Setup(x => x.DeleteActivityAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteActivity(1);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteActivity_NonExistingId_ReturnsNotFound()
        {
            _activityService.Setup(x => x.DeleteActivityAsync(1)).ReturnsAsync(false);

            var result = await _controller.DeleteActivity(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetAllActivitiesOrderedByDate_ReturnsActivitiesOrderedByDate()
        {
            var userId = Guid.NewGuid();
            var activities = new List<Activity>
            {
                new Activity { ActivityId = 1, ActivityName = "Yoga Class", Date = DateTime.Today.AddDays(1), UserId = userId },
                new Activity { ActivityId = 2, ActivityName = "Pilates", Date = DateTime.Today, UserId = userId }
            };
            _activityService.Setup(x => x.GetAllActivitiesAsync()).ReturnsAsync(activities);

            var result = await _controller.GetAllActivitiesOrderedByDate(Guid.NewGuid());

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (result.Result as OkObjectResult).Value as List<ActivityModel>;
            var orderedActivities = okResult.Select(a => a.ActivityId).ToList();
            Assert.AreEqual(2, orderedActivities.First());
        }

    }
}
