using CommunityCenterApi.DB;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityCenterApi.Services.Implementations
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;

        public ActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Activity> CreateActivityAsync(Activity newActivity)
        {
            _context.Activities.Add(newActivity);
            await _context.SaveChangesAsync();
            return newActivity;
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _context.Activities.ToListAsync();
        }

        public async Task<Activity> GetActivityByIdAsync(int activityId)
        {
            return await _context.Activities.FindAsync(activityId);
        }

        public async Task<Activity> UpdateActivityAsync(int activityId, Activity updatedActivity)
        {
            var existingActivity = await _context.Activities.FindAsync(activityId);
            if (existingActivity == null)
            {
                return null;
            }

            existingActivity.ActivityName = updatedActivity.ActivityName;
            existingActivity.Description = updatedActivity.Description;
            // Update other properties as needed

            _context.Entry(existingActivity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingActivity;
        }

        public async Task<bool> DeleteActivityAsync(int activityId)
        {
            var activity = await _context.Activities.FindAsync(activityId);
            if (activity == null)
            {
                return false;
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
