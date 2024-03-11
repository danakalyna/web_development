using CommunityCenterApi.Log;
using CommunityCenterApi.Models;
using CommunityCenterApi.Services.Interfaces;

namespace CommunityCenterApi.Services.Implementations
{
    public class ActivityServiceLoggingDecorator : IActivityService
    {
        private readonly IActivityService _decoratedActivityService;
        private readonly IFileLoggerFactory _loggerFactory;

        public ActivityServiceLoggingDecorator(IActivityService decoratedActivityService, IFileLoggerFactory loggerFactory)
        {
            _decoratedActivityService = decoratedActivityService;
            _loggerFactory = loggerFactory;
        }

        public async Task<Activity> CreateActivityAsync(Activity newActivity)
        {
            using (var logger = _loggerFactory.CreateLogger())
            {
                try
                {
                    logger.WriteTextToFile("Creating a new activity.");
                    var result = await _decoratedActivityService.CreateActivityAsync(newActivity);
                    logger.WriteTextToFile($"Activity created successfully with ID: {result.ActivityId}.");
                    return result;

                }
                catch (Exception ex)
                {
                    logger.WriteTextToFile($"Error creating activity {ex}");
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            using (var logger = _loggerFactory.CreateLogger())
            {
                try
                {
                    logger.WriteTextToFile("Retrieving all activities.");
                    var result = await _decoratedActivityService.GetAllActivitiesAsync();
                    logger.WriteTextToFile($"Retrieved all activities successfully. Count: {result.Count()}.");
                    return result;
                }
                catch (Exception ex)
                {
                    logger.WriteTextToFile($"Error retrieving activities. {ex}");
                    throw;
                }
            }
        }

        public async Task<Activity> GetActivityByIdAsync(int activityId)
        {
            using (var logger = _loggerFactory.CreateLogger())
            {
                try
                {
                    logger.WriteTextToFile($"Retrieving activity with ID: {activityId}.");
                    var result = await _decoratedActivityService.GetActivityByIdAsync(activityId);
                    if (result != null)
                    {
                        logger.WriteTextToFile($"Activity retrieved successfully with ID: {result.ActivityId}.");
                    }
                    else
                    {
                        logger.WriteTextToFile($"Activity with ID: {activityId} not found.");
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    logger.WriteTextToFile($"Error retrieving activity with ID: {activityId}. {ex}");
                    throw;
                }
            }
        }

        public async Task<Activity> UpdateActivityAsync(int activityId, Activity updatedActivity)
        {
            using (var logger = _loggerFactory.CreateLogger())
            {
                try
                {
                    logger.WriteTextToFile($"Updating activity with ID: {activityId}.");
                    var result = await _decoratedActivityService.UpdateActivityAsync(activityId, updatedActivity);
                    if (result != null)
                    {
                        logger.WriteTextToFile($"Activity updated successfully with ID: {result.ActivityId}.");
                    }
                    else
                    {
                        logger.WriteTextToFile($"Activity with ID: {activityId} not found for update.");
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    logger.WriteTextToFile($"Error updating activity with ID: {activityId}. {ex}");
                    throw;
                }
            }
        }

        public async Task<bool> DeleteActivityAsync(int activityId)
        {
            using (var logger = _loggerFactory.CreateLogger())
            {
                try
                {
                    logger.WriteTextToFile($"Deleting activity with ID: {activityId}.");
                    var success = await _decoratedActivityService.DeleteActivityAsync(activityId);
                    if (success)
                    {
                        logger.WriteTextToFile($"Activity deleted successfully with ID: {activityId}.");
                    }
                    else
                    {
                        logger.WriteTextToFile($"Activity with ID: {activityId} not found for deletion.");
                    }
                    return success;
                }
                catch (Exception ex)
                {
                    logger.WriteTextToFile($"Error deleting activity with ID: {activityId}. {ex}");
                    throw;
                }
            }
        }
    }

}
