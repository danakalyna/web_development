using CommunityCenterApi.Models;

namespace CommunityCenterApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User newUser);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> UpdateUserAsync(Guid userId, User updatedUser);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
