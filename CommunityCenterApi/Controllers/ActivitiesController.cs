using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommunityCenterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly IBookingService bookingService;

        public ActivitiesController(IActivityService activityService, IBookingService bookingService)
        {
            _activityService = activityService;
            this.bookingService = bookingService;
        }

        [HttpPost]
        public async Task<ActionResult<Activity>> CreateActivity(Activity activity)
        {
            if (activity == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdActivity = await _activityService.CreateActivityAsync(activity);
            return CreatedAtAction(nameof(GetActivityById), new { id = createdActivity.ActivityId }, createdActivity);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetAllActivities()
        {
            var activities = await _activityService.GetAllActivitiesAsync();
            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivityById(int id)
        {
            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return activity;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(int id, Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedActivity = await _activityService.UpdateActivityAsync(id, activity);
            if (updatedActivity == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var success = await _activityService.DeleteActivityAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("GetAllActivitiesOrderedByDate/{userId}")]
        public async Task<ActionResult<IEnumerable<ActivityModel>>> GetAllActivitiesOrderedByDate(Guid userId)
        {
            var activities = await _activityService.GetAllActivitiesAsync();
            var orderedActivities = activities.OrderBy(a => a.Date).ToList();

            var models = orderedActivities.Select(a => new ActivityModel
            {
                // Assuming ActivityModel inherits from Activity and you want to copy all properties
                ActivityId = a.ActivityId,
                ActivityName = a.ActivityName,
                Date = a.Date,
                Description = a.Description,
                Bookings = a.Bookings,
                IsBooked = a.Bookings == null ? false : a.Bookings.Any(b => b.UserId == userId) // Set IsBooked here
            }).ToList();

            return Ok(models);
        }
    }
    public class ActivityModel : Activity
    {
        public bool IsBooked {  get; set; }
    }
}
