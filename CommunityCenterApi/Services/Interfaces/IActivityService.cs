using CommunityCenterApi.Models;

namespace CommunityCenterApi.Services.Interfaces
{
    public interface IActivityService
    {
        Task<Activity> CreateActivityAsync(Activity newActivity);
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        Task<Activity> GetActivityByIdAsync(int activityId);
        Task<Activity> UpdateActivityAsync(int activityId, Activity updatedActivity);
        Task<bool> DeleteActivityAsync(int activityId);
    }
}
