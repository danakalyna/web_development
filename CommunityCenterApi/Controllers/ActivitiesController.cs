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

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
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
    }
}
